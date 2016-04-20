using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PlatHak.Common.Maths;
using PlatHak.Common.World;
using PlatHak.Server.Common;

namespace PlatHak.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            var wc = new WorldConfig(new Size(10, 10), new Size(10, 10), new Size(10, 10));
            var cluster = new ChunkCluster();
            cluster.Chunks = new Chunk[20,20];
            for (int cx = 0; cx < 20; cx++)
            {
                for (int cy = 0; cy < 20; cy++)
                {
                    cluster.Chunks[cx, cy] = new Chunk(VectorInt2.Zero, wc);
                    cluster.Chunks[cx, cy].Items = new Block[20, 20];
                    for (int x = 0; x < 20; x++)
                    {
                        for (int y = 0; y < 20; y++)
                        {
                            cluster.Tests[cx, cy].Floats[x, y] = null;
                        }
                    }
                }
            }
            
            var json = JsonConvert.SerializeObject(cluster);
            var obj = JsonConvert.DeserializeObject<cluster>(json);
        }
    }

}
