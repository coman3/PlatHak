using System.Collections.Generic;
using System.Linq;
using PlatHak.Client.Common;
using PlatHak.Client.Common.Interfaces;
using PlatHak.Client.Network;
using PlatHak.Common.Maths;
using PlatHak.Common.Network;
using PlatHak.Common.World;

namespace PlatHack.Game.Surfaces.Game
{
    public class ClientWorldManager :  IUpdatedSurface
    {
        public World World { get; set; }
        public WorldDrawer Drawer { get; set; }
        public Player Player { get; set; }
        public WebSocketClient Client { get; set; }
        public Chunk CurrentChunkIn { get; private set; }
        public bool Loaded => World != null && Drawer != null && Player != null;
        public int ChunkLoadRadius { get; set; }
        public List<VectorInt2> RequestedChunks { get; set; }
        public ClientWorldManager(WebSocketClient client)
        {
            Client = client;
            Client.OnPacketRecived += Client_OnPacketRecived;
            ChunkLoadRadius = 3;
            RequestedChunks = new List<VectorInt2>();
        }

        private void Client_OnPacketRecived(PacketEventArgs<Packet> args)
        {
            var packet = args.Packet;
            if (!Loaded) return;

            packet.DoIfIsType<ChunkPacket>(chunkPacket =>
            {
                World.SetChunk(chunkPacket.Chunk);
            });
            packet.DoIfIsType<PlayerMovePacket>(movePacket =>
            {
                if (Player.Username == movePacket.Username)
                {
                    Player.Velocity = movePacket.NewVelocity;
                    //Player.Posistion = movePacket.ServerPosistion;
                }
                else
                {
                    var player = World.Players.FirstOrDefault(x => x.Username == movePacket.Username);
                    if (player == null) return;
                    player.Velocity =
                        movePacket.NewVelocity;
                    player.Posistion = movePacket.ServerPosistion;
                }
            });

        }

        public void OnUpdate(GameTime time)
        {
            if(!Loaded) return;

            var pos = Player.GetNextPosistion(World.GlobalCoordinatesSize);
            var chunk = World.GetChunkCordsFromPosition(pos);
            var chunkItem = World.Chunks[chunk.X, chunk.Y];
            if (CurrentChunkIn != chunkItem)
            {

                RadialScan.AllPoints(chunk, ChunkLoadRadius, (x, y) =>
                {
                    if (World.Chunks[x, y] == null && !RequestedChunks.Any(c=> c.X == x && c.Y == y))
                    {
                        RequestedChunks.Add(new VectorInt2(x, y));
                        Client.Send(new ChunkRequestPacket { ChunkPosistion = new VectorInt2(x, y) });
                    }
                });
                CurrentChunkIn = chunkItem;
            }
            Player.UpdatePosistion(World.GlobalCoordinatesSize);
            foreach (var player in World.Players)
            {
                
                player.UpdatePosistion(World.GlobalCoordinatesSize);
            }
            
        }
    }
}