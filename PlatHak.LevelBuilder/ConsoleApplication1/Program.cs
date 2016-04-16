using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        /// <summary>
        /// Chunk X Count
        /// </summary>
        public static int WorldWidth { get; set; }
        /// <summary>
        /// Chunk Y Count
        /// </summary>
        public static int WorldHeight { get; set; }
        /// <summary>
        /// Chunk Size
        /// </summary>
        public static int WorldScale { get; set; } = 64;

        public static bool[,] LandMap { get; set; }

        static void Main(string[] args)
        {
            var waterColor = Color.FromArgb(255, 179, 226, 238);
            var bitmap = Properties.Resources.World_map_without_dots;
            LandMap = new bool[bitmap.Width, bitmap.Height];
            WorldWidth = bitmap.Width;
            WorldHeight = bitmap.Height;
            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    LandMap[x, y] = bitmap.GetPixel(x, y) != waterColor;
                    var isLand = LandMap[0, 0];
                    var chunk = new Chunk(WorldScale);
                    for (int bx = 0; bx < WorldScale; bx++)
                    {
                        for (int by = 0; by < WorldScale; by++)
                        {
                            chunk.Blocks[bx, by] = new ChunkBlock(isLand ? 1 : 0, null);
                        }
                    }
                    var formater = new BinaryFormatter();
                    var fileStream = new FileStream(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), $"Chunks\\Chunk_{x}_{y}.phcd"), FileMode.Create);
                    formater.Serialize(fileStream, chunk);
                    fileStream.Close();
                }
            }
            

        }
    }
    [Serializable]
    class Chunk
    {
        public ChunkBlock[,] Blocks { get; set; }
        public int Size { get; set; }

        public Chunk(int size)
        {
            Size = size;
            Blocks = new ChunkBlock[size, size];
        }
    }

    [Serializable]
    class ChunkBlock
    {
        public int Id { get; set; }
        public ChunkBlockData Data { get; set; }

        public ChunkBlock(int id, ChunkBlockData data)
        {
            Id = id;
            Data = data;
        }
    }
    [Serializable]
    abstract class ChunkBlockData
    {
         
    }
}
