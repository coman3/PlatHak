using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlatHak.Common.World;
using PlatHak.Server.Network;
using SuperSocket.SocketBase.Config;

namespace PlatHak.Server
{
    class Program
    {
        public static Server Server { get; set; }
        
        static void Main(string[] args)
        {
            Server = new Server(args);
            while (true)
            {
                Server.ProcessCommand(Console.ReadLine());
            }
        }
    }
}
