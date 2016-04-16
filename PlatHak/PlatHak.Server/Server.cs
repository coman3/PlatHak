using System;
using System.Collections.Generic;
using System.Linq;
using PlatHak.Common;
using PlatHak.Common.Maths;
using PlatHak.Common.Network;
using PlatHak.Common.World;
using PlatHak.Server.Common;
using PlatHak.Server.Network;
using SuperSocket.SocketBase.Config;

namespace PlatHak.Server
{
    //178.32.217.118
    public class Server
    {
        public WebSocketServer SocketServer;
        public WorldManager WorldManager { get; set; }
        public Server(string[] args)
        {
            SocketServer = new WebSocketServer(new ServerConfig
            {
                Port = 3344
            });
            
            SocketServer.OnLogOutput += Server_OnLogOutput;
            SocketServer.OnSetup += Server_OnSetup;
            SocketServer.OnClientLoaded += SocketServer_OnClientLoaded;
            SocketServer.Start();
        }


        private void SocketServer_OnClientLoaded(UserClient session)
        {
            session.Send(new WorldPacket(WorldManager.World.WorldConfig));
            if (session.Player == null)
            {
                session.Player = new Player
                {
                    Posistion = new VectorInt2(50, 50),
                    Username = session.Username,
                };
                WorldManager.World.Players.Add(session.Player);
                SocketServer.Broadcast(new PlayerPacket(session.Player));
                Console.WriteLine("Sent new player to all clients!");
            }
            foreach (var player in WorldManager.World.Players.Where(x => x != session.Player))
            {
                session.Send(new PlayerPacket(player));
            }
            var chunkIn = WorldManager.World.GetChunkFromPosistion(session.Player.Posistion);
            var chunkDistance = 1;
            for (int x = Math.Max(0, chunkIn.Bounds.X - (chunkDistance));
                x < Math.Min(chunkIn.Bounds.X + chunkDistance + 1, WorldManager.World.WorldConfig.WorldSize.Width);
                x++)
            {
                for (int y = Math.Max(0, chunkIn.Bounds.Y - (chunkDistance));
                    y < Math.Min(chunkIn.Bounds.Y + chunkDistance + 1, WorldManager.World.WorldConfig.WorldSize.Height);
                    y++)
                {
                    session.Send(new ChunkPacket(WorldManager.World.Grids[x, y]));
                }
            }
        }

        public void ProcessCommand(string data)
        {
            Console.WriteLine(data);
            
        }
        private void Server_OnSetup(WebSocketServerEventArgs args)
        {
            WorldManager =
                new WorldManager(ref SocketServer, new World(new WorldConfig(new Size(1024, 1024), new Size(8, 8), new Size(32, 32))));
            for (int x = 0; x < WorldManager.World.WorldConfig.WorldSize.Width; x++)
            {
                for (int y = 0; y < WorldManager.World.WorldConfig.WorldSize.Height; y++)
                {
                    var grid = WorldManager.World.Grids[x, y] = new WorldGrid(new VectorInt2(x, y), WorldManager.World.WorldConfig);
                    for (int cx = 0; cx < WorldManager.World.WorldConfig.ChunkSize.Width; cx++)
                    {
                        for (int cy = 0; cy < WorldManager.World.WorldConfig.ChunkSize.Height; cy++)
                        {
                            grid.AddGridItem(cx, cy, new BlockGridItem
                            {
                                IsSolid = cx == 0 || cy == 0 || cx == WorldManager.World.WorldConfig.ChunkSize.Width || cy == WorldManager.World.WorldConfig.ChunkSize.Height,
                            });
                        }
                    }
                }
            }
        }

        private static void Server_OnLogOutput(string message)
        {
            Console.WriteLine(message);
        }
    }
}