﻿using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;
using PlatHak.Common;
using PlatHak.Common.Maths;
using PlatHak.Common.World;
using PlatHak.Server.Common;
using PlatHak.Server.Network;
using Color = System.Drawing.Color;
using Size = PlatHak.Common.Maths.Size;

namespace PlatHak.Server
{
    public class Server
    {
        public WebSocketServer SocketServer { get; set; }
        public NewServerWorldManager ServerWorldManager { get; set; }
        public Server(string[] args)
        {
            SocketServer = new WebSocketServer(new ServerConfig
            {
                Port = 3344
            });
            SocketServer.OnLogOutput += Console.WriteLine;
            SocketServer.OnSetup += Server_OnSetup;
            SocketServer.OnClientLoginHandshakeSuccess += SocketServer_OnClientLoginHandshakeSuccess;
            SocketServer.Start();

        }

        private void SocketServer_OnClientLoginHandshakeSuccess(UserClient session)
        {
            Console.WriteLine($"{session.SessionId} ({session.Username}) has connected.");
        }

      
        private void Server_OnSetup(WebSocketServerEventArgs args)
        {
            ServerWorldManager = new NewServerWorldManager(SocketServer, new World(WorldConfig.Default),
                new WorldManagerConfig(1, Path.Combine(Environment.CurrentDirectory, "WorldTileData"),
                    Path.Combine(Environment.CurrentDirectory, "Chunks")));
            Task.Run(ServerWorldManager.Initialize);
        }
    }
}