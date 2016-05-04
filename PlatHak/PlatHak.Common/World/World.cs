using System;
using System.Collections.Generic;
using PlatHak.Common.Maths;

namespace PlatHak.Common.World
{
    [Serializable]
    public class World
    {
        public List<Entity> Entities { get; set; }
        public WorldConfig WorldConfig { get; set; }
        public Chunk[,] Chunks { get; set; }

        

        public World(WorldConfig config)
        {
            Entities = new List<Entity>();
            WorldConfig = config;
            Chunks = new Chunk[WorldConfig.WorldSize.Width, WorldConfig.WorldSize.Height];
        }

        public Chunk SetChunk(Chunk chunk)
        {
            return SetChunk(chunk.Bounds.Posistion, chunk);
        }
        public Chunk SetChunk(VectorInt2 posistion, Chunk chunk)
        {
            Chunks[posistion.X, posistion.Y] = chunk;
            return chunk;
        } 
        public VectorInt2 GetChunkCordsFromPosition(VectorInt2 posistion)
        {
            if (posistion.X > 0 && WorldConfig.GlobalCoordinatesSize.Width > posistion.X)
            {
                if (posistion.Y > 0 && WorldConfig.GlobalCoordinatesSize.Height > posistion.Y)
                {
                    var chunkPixelWidth = WorldConfig.ChunkSize.Width * WorldConfig.ItemSize.Width;
                    var chunkPixelHeight = WorldConfig.ChunkSize.Height * WorldConfig.ItemSize.Height;
                    var chunk = new VectorInt2(posistion.X / chunkPixelWidth, posistion.Y / chunkPixelHeight);
                    return chunk;
                }
            }
            throw new ArgumentOutOfRangeException(nameof(posistion));
        }

        public Chunk GetChunkFromPosistion(VectorInt2 posistion)
        {
            var chunk = GetChunkCordsFromPosition(posistion);
            return Chunks[chunk.X, chunk.Y];

        }
        /// <summary>
        /// Get the posistion of the block from a global posistion
        /// </summary>
        /// <param name="posistion"></param>
        /// <returns>
        /// the posistion of the block witin its chunk
        /// </returns>
        public VectorInt2 GetBlockCordsFromPosistion(VectorInt2 posistion)
        {
            var chunk = GetChunkCordsFromPosition(posistion);
            var block = new VectorInt2(posistion.X/WorldConfig.ItemSize.Width - WorldConfig.ChunkSize.Width * chunk.X,
                posistion.Y/WorldConfig.ItemSize.Height - WorldConfig.ChunkSize.Height * chunk.Y);
            return block;
        }

        public Block GetBlockFromPosistion(VectorInt2 posistion)
        {
            var chunk = GetChunkFromPosistion(posistion);
            if (chunk == null) return null;
            var block = GetBlockCordsFromPosistion(posistion);
            return chunk.Items[block.X, block.Y];
        }
    }
}