using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using Geo;
using Newtonsoft.Json;
using PlatHak.Common.Maths;
using PlatHak.Common.World;
using Size = PlatHak.Common.Maths.Size;

namespace PlatHak.Server.Common
{
    public class WorldSaver
    {
        private World World { get; }
        private WorldConfig WorldConfig => World.WorldConfig;

        public WorldSaver(World world)
        {
            World = world;
        }

        public async Task<Cluster[]> LoadCluster(Dictionary<Coordinate, Bitmap> image)
        {
            return await Task.Run(() =>
            {
                var clusters = new List<Cluster>();
                Parallel.ForEach(image, bitmap =>
                //foreach (var bitmap in image)
                {
                    clusters.Add(LoadCluster(bitmap.Key, bitmap.Value));
                    //}
                });

                return clusters.ToArray();
            });
        }
        public Cluster LoadCluster(Coordinate cord, Bitmap image)
        {
            var clusterTileSize = WorldConfig.ClusterSize * WorldConfig.ChunkSize;
            if (clusterTileSize.Width != image.Width || clusterTileSize.Height != image.Height)
                return null;

            var globalPosistion =
                WorldConfig.GetGlobalPosistionFromLatLon(new Vector2((float)cord.Latitude, (float)cord.Longitude));
            var localBlockPosistion = WorldConfig.GetBlockLocalPosistion(globalPosistion);
            var localClusterPosistion = WorldConfig.GetClusterLocalPosistion(globalPosistion);

            var cluster = new Cluster(WorldConfig.GetClusterGlobalPosistion(localBlockPosistion, localClusterPosistion), WorldConfig);
            for (long x = cluster.Bounds.X; x < cluster.Bounds.Right; x++)
            {
                for (long y = cluster.Bounds.Y; y < cluster.Bounds.Bottom; y++)
                {
                    var globalTilePosistion = new VectorLong2(x, y);
                    var localChunkPosition = WorldConfig.GetChunkLocalPosistion(globalTilePosistion);

                    Chunk chunk = cluster.Chunks[localChunkPosition.X, localChunkPosition.Y];
                    if (chunk == null)
                    {
                        chunk = cluster.Chunks[localChunkPosition.X, localChunkPosition.Y] = new Chunk(WorldConfig.GetChunkGlobalPosistion(localBlockPosistion, localClusterPosistion, localChunkPosition), WorldConfig);
                    }

                    var pixel = image.GetPixel(Convert.ToInt32(x - cluster.Bounds.X), Convert.ToInt32(y - cluster.Bounds.Y));
                    var localTilePosition = WorldConfig.GetTileLocalPosistion(globalTilePosistion);
                    chunk.Tiles[localTilePosition.X, localTilePosition.Y] = new Tile(pixel.R, 0);
                }
            }

            return cluster;
        }

        public bool SaveWorld(string savePath)
        {
            try
            {
                foreach (var block in World.Blocks)
                {
                    using (var file = File.OpenWrite(Path.Combine(savePath, $"{block.Key.X}_{block.Key.Y}.wblf")))
                    using (var gzip = new GZipStream(file, CompressionLevel.Optimal))
                    {
                        block.Value.ToStream(gzip);
                    }
                }
                return true;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return false;
            }
        }
        public bool LoadWorld(string savePath)
        {
            try
            {
                World.Blocks = new Dictionary<VectorLong2, Block>();
                foreach (var file in Directory.EnumerateFiles(savePath))
                {
                    using (var fileStream = File.OpenRead(file))
                    using (var gzip = new GZipStream(fileStream, CompressionMode.Decompress))
                    {
                        var block = new Block(WorldConfig, gzip);
                        World.Blocks.Add(WorldConfig.GetBlockLocalPosistion(block.Bounds.Posistion), block);
                    }
                }
                return true;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return false;
            }
        }
    }
}