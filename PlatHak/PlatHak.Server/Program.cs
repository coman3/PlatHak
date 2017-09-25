using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlatHak.Common.World;
using PlatHak.Server.Network;

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
                Console.ReadLine();
            }
        }
    }
}
