using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PlatHak.Common.Maths;
using PlatHak.Common.World;

namespace PlatHak.Server.Common
{
    public class WorldSaver
    {
        public Size Size { get; }
        public Size ChunksPerFile { get; set; }

        private World World { get; set; }
        private WorldConfig Config { get; }
        
        public WorldSaver(World world, Size chunkPerFile)
        {
            World = world;
            Config = world.WorldConfig;
            ChunksPerFile = chunkPerFile;
            Size = new Size(Config.WorldSize.Width / chunkPerFile.Width, Config.WorldSize.Height / chunkPerFile.Height);
        }

        public void Save(int sx, int sy, ChunkCluster grids)
        {
            grids.LocalPosistion = new VectorInt2(sx, sy);
            grids.WorldPosistion = new Rectangle(grids[0, 0].Bounds.Posistion, ChunksPerFile);
            SaveChunk(grids);
        }

        public bool Exists(int sx, int sy)
        {
            var file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Chunks",
                $"Chunk_{sx}_{sy}.chd");
            return File.Exists(file);
        }


        private void SaveChunk(ChunkCluster grid)
        {
            var file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Chunks",
                $"Chunk_{grid.LocalPosistion.X}_{grid.LocalPosistion.Y}.chd");
            using (var fileStream = new FileStream(file, FileMode.Create))
            {
                using (var gzipStream = new GZipStream(fileStream, CompressionLevel.Fastest, false))
                {
                    using (var sw = new StreamWriter(gzipStream))
                    {
                        var result = JsonConvert.SerializeObject(grid);
                        sw.WriteLine(result);
                    }
                }

            }

        }

        public void LoadChunk(VectorInt2 chunkPosistion)
        {
            LoadChunkCluster(chunkPosistion / ChunksPerFile);
        }
        public void LoadChunkCluster(VectorInt2 chuckClusterPos)
        {
            var cluster = LoadCluster(chuckClusterPos);
            if (cluster == null)
                throw new InvalidOperationException("Cluster was not loaded!");
            foreach (var worldGrid in cluster.Chunks)
            {
                World.Chunks[worldGrid.Bounds.X, worldGrid.Bounds.Y] = worldGrid;
            }
        }

        private ChunkCluster LoadCluster(VectorInt2 pos)
        {
            var file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Chunks",
                $"Chunk_{pos.X}_{pos.Y}.chd");
            if (!File.Exists(file))
                throw new FileLoadException("File Not Found: " + file + ".\nTry Regenerating the world.");
            using (var fileStream = File.OpenRead(file))
            using (var gZipStream = new GZipStream(fileStream, CompressionMode.Decompress))
            {
                using (var streamReader = new StreamReader(gZipStream))
                {
                    Console.WriteLine($"Loading Cluster {pos}...");

                    try
                    {

                    
                    var json = streamReader.ReadToEnd();
                    var stopWatch = Stopwatch.StartNew();
                    
                    var result = JsonConvert.DeserializeObject<ChunkCluster>(json);
                    stopWatch.Stop();
                    Console.WriteLine(
                        $"Loaded Cluster {pos}. Took: {stopWatch.Elapsed.TotalSeconds} secconds");
                    return result;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        throw;
                    }
                }
            }
        }
    }
}