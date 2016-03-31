using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlatHak.Server.Network;
using SuperSocket.SocketBase.Config;

namespace PlatHak.Server
{
    class Program
    {
        public static WebSocketServer Server { get; set; }
        static void Main(string[] args)
        {
            Server = new WebSocketServer(new ServerConfig
            {
                Port = 3344
            });
            Server.OnLogOutput += Server_OnLogOutput;
            Server.Start();

            while (true)
            {
                Console.ReadLine();
            }
        }

        private static void Server_OnLogOutput(string message)
        {
            Console.WriteLine(message);
        }
    }
}
