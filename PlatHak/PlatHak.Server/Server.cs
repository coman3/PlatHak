using System;
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
using PlatHak.Common.Network;
using PlatHak.Common.World;
using PlatHak.Server.Common;
using PlatHak.Server.Network;
using SuperSocket.SocketBase.Config;
using Color = System.Drawing.Color;
using Size = PlatHak.Common.Maths.Size;

namespace PlatHak.Server
{
    //178.32.217.118
    public class Server
    {
        public WebSocketServer SocketServer { get; set; }
        public ServerWorldManager ServerWorldManager { get; set; }
        public Server(string[] args)
        {
            SocketServer = new WebSocketServer(new ServerConfig
            {
                Port = 3344
            });
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
            var stopwatch = Stopwatch.StartNew();

            var bitmap = Properties.Resources.map;
            ServerWorldManager = new ServerWorldManager(SocketServer, new World(new WorldConfig(new Size(bitmap.Width, bitmap.Height), new Size(16, 16), new Size(4, 4), TimeSpan.FromMilliseconds(1000f / 60))));
            ServerWorldManager.Load(bitmap);

            stopwatch.Stop();
            Console.WriteLine($"Loaded world in {stopwatch.Elapsed.TotalSeconds}!");
        }
    }
}