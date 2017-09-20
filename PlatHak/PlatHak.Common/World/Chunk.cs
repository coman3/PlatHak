using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using PlatHak.Common.Interfaces;
using PlatHak.Common.Maths;

namespace PlatHak.Common.World
{
    
    public class Chunk : IDisposable, ISerialize
    {
        public Rectangle Bounds { get; set; }
        public Tile[,] Tiles { get; set; }
        //public Size Size => Bounds.Size;
        private WorldConfig WorldConfig { get; }
        public Chunk(VectorLong2 chunkPos, WorldConfig worldConfig)
        {
            WorldConfig = worldConfig;
            Bounds = new Rectangle(chunkPos, worldConfig.ChunkSize);
            Tiles = new Tile[worldConfig.ChunkSize.Width, worldConfig.ChunkSize.Height];
        }

        public Chunk(WorldConfig worldConfig, Stream stream)
        {
            WorldConfig = worldConfig;
            FromStream(stream);
        }

        public void Dispose()
        {
            Tiles = null;
        }

        public void ToStream(Stream stream)
        {
            Bounds.ToStream(stream);
            for (int x = 0; x < Tiles.GetLength(0); x++)
            {
                for (int y = 0; y < Tiles.GetLength(1); y++)
                {
                    Tiles[x, y].ToStream(stream);
                }
            }
        }

        public void FromStream(Stream stream)
        {
            Bounds = new Rectangle(stream);
            Tiles = new Tile[WorldConfig.ChunkSize.Width, WorldConfig.ChunkSize.Height];
            for (int x = 0; x < Tiles.GetLength(0); x++)
            {
                for (int y = 0; y < Tiles.GetLength(1); y++)
                {
                    Tiles[x, y].FromStream(stream);
                }
            }
        }
    }
}