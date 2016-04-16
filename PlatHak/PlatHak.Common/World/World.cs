using System;
using System.Collections.Generic;
using PlatHak.Common.Maths;

namespace PlatHak.Common.World
{
    [Serializable]
    public class World
    {
        public List<Player> Players { get; set; }
        public WorldConfig WorldConfig { get; set; }

        public WorldGrid[,] Grids { get; set; }
        public Size GlobalCoordinatesSize { get; set; }
        public World(WorldConfig config)
        {
            Players = new List<Player>();
            WorldConfig = config;
            Grids = new WorldGrid[WorldConfig.WorldSize.Width, WorldConfig.WorldSize.Height];
            GlobalCoordinatesSize =
                new Size(WorldConfig.WorldSize.Width*WorldConfig.ChunkSize.Width*WorldConfig.ItemSize.Width,
                    WorldConfig.WorldSize.Height*WorldConfig.ChunkSize.Height*WorldConfig.ItemSize.Height);
        }
        public WorldGrid GetChunkFromPosistion(VectorInt2 posistion)
        {
            if (posistion.X > 0 && GlobalCoordinatesSize.Width > posistion.X)
            {
                if (posistion.Y > 0 && GlobalCoordinatesSize.Height > posistion.Y)
                {
                    var chunkPixelWidth = WorldConfig.ChunkSize.Width * WorldConfig.ItemSize.Width;
                    var chunkPixelHeight = WorldConfig.ChunkSize.Height * WorldConfig.ItemSize.Height;
                    var chunk = new VectorInt2(posistion.X/chunkPixelWidth, posistion.Y/chunkPixelHeight);
                    return Grids[chunk.X, chunk.Y];
                }
            }
            return null;
        }
        public GridItem GetBlockFromPosistion(VectorInt2 posistion)
        {
            var chunk = GetChunkFromPosistion(posistion);
            if (chunk == null) return null;
            var block = new VectorInt2(posistion.X/WorldConfig.ItemSize.Width - chunk.Bounds.Width * chunk.Bounds.X,
                posistion.Y/WorldConfig.ItemSize.Height - chunk.Bounds.Height * chunk.Bounds.Y);
            return chunk.Items[block.X, block.Y];
        }
    }
    [Serializable]
    public struct WorldConfig
    {
        /// <summary>
        /// The size of the world, in chunks
        /// </summary>
        public Size WorldSize;

        /// <summary>
        /// The size of each chunk, in GridItems.
        /// </summary>
        public Size ChunkSize;
        /// <summary>
        /// The size of each GridItem (in pixels). 
        /// </summary>
        public Size ItemSize;

        public WorldConfig(Size worldSize, Size chunkSize, Size itemSize)
        {
            WorldSize = worldSize;
            ChunkSize = chunkSize;
            ItemSize = itemSize;
        }
    }
}