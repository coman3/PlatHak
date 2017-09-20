using System;
using System.IO;
using PlatHak.Common.Interfaces;
using PlatHak.Common.Maths;

namespace PlatHak.Common.World
{
    public class Cluster : IDisposable, ISerialize
    {
        public Rectangle Bounds { get; set; }
        public Chunk[,] Chunks { get; set; }
        public Size Size => Bounds.Size;

        private WorldConfig WorldConfig { get; }

        public Cluster(VectorLong2 globalPosition, WorldConfig worldConfig)
        {
            WorldConfig = worldConfig;
            Bounds = new Rectangle(globalPosition, worldConfig.ClusterSize * worldConfig.ChunkSize);
            Chunks = new Chunk[worldConfig.ClusterSize.Width, worldConfig.ClusterSize.Height];
        }

        public Cluster(WorldConfig worldConfig, Stream stream)
        {
            WorldConfig = worldConfig;
            FromStream(stream);
        }

        public void Dispose()
        {
            if (Chunks == null) return;
            foreach (var gridItem in Chunks)
            {
                gridItem?.Dispose();
            }
            Chunks = null;
        }

        public void ToStream(Stream stream)
        {
            Bounds.ToStream(stream);
            for (int x = 0; x < Chunks.GetLength(0); x++)
            {
                for (int y = 0; y < Chunks.GetLength(1); y++)
                {
                    Chunks[x, y].ToStream(stream);
                }
            }
        }

        public void FromStream(Stream stream)
        {
            Bounds = new Rectangle(stream);
            Chunks = new Chunk[WorldConfig.ClusterSize.Width, WorldConfig.ClusterSize.Height];
            for (int x = 0; x < Chunks.GetLength(0); x++)
            {
                for (int y = 0; y < Chunks.GetLength(1); y++)
                {
                    Chunks[x, y] = new Chunk(WorldConfig, stream);
                }
            }
        }
    }
}