using System.Linq;
using PlatHak.Common.Maths;
using PlatHak.Common.Network;
using PlatHak.Common.World;
using PlatHak.Server.Network;

namespace PlatHak.Server.Common
{
    public class WorldManager
    {
        public WorldManager(ref WebSocketServer socketServer, World world)
        {
            Server = socketServer;
            Server.OnPacketReceived += Server_OnPacketReceived;
            World = world;
        }
        public World World;
        public WebSocketServer Server;
        private void Server_OnPacketReceived(UserClient client, Packet packet)
        {
            if (packet is ChunkRequestPacket)
            {
                var pos = packet.Cast<ChunkRequestPacket>().ChunkPosistion;
                if (pos.X >= 0 && pos.X <= World.WorldConfig.WorldSize.Width && pos.Y >= 0 &&
                    pos.Y <= World.WorldConfig.WorldSize.Height)
                {
                    client.Send(new ChunkPacket(World.Grids[pos.X, pos.Y]));
                }
                return;
            }
            if (packet is MoveRequest)
            {
                var moveRequest = packet.Cast<MoveRequest>();
                var newPosChunkIn = World.GetChunkFromPosistion(moveRequest.NewPosistion);
                if(newPosChunkIn == null) return;
                //TODO: Add Colision checks, and distance checks
                UpdatePlayerPosistion(client.Player, moveRequest.NewPosistion);
                return;
            }
        }

        public void UpdatePlayerPosistion(Player player, VectorInt2 newPos)
        {
            if (World.Players.Contains(player))
            {
                World.Players[World.Players.IndexOf(player)].Posistion = newPos;
                Server.Broadcast(new PlayerMovePacket(player.Username, newPos));
            }
        }


        public void Update()
        {
            
        }
    }
}