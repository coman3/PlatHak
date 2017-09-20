using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using PlatHak.Common.Maths;

namespace PlatHak.Common.World
{
    
    public class World
    {
        public List<Entity> Entities { get; set; }
        public WorldConfig WorldConfig { get; set; }
        public Dictionary<VectorLong2, Block> Blocks { get; set; }

        public World(WorldConfig config)
        {
            Entities = new List<Entity>();
            WorldConfig = config;
            Blocks = new Dictionary<VectorLong2, Block>(Convert.ToInt32(WorldConfig.WorldSize.Width * WorldConfig.WorldSize.Height));
        }


        public Block SetBlock(Block block)
        {
            return SetBlock(block.Bounds.Posistion, block);
        }

        private Block SetBlock(VectorLong2 localPosition, Block block)
        {
            Blocks[localPosition] = block;
            return block;
        }

        public Block GetBlock(VectorLong2 globalPosistion, bool createIfNull = true)
        {
            var blockPos = WorldConfig.GetBlockLocalPosistion(globalPosistion);
            var globalBlockPosistion = WorldConfig.GetBlockGlobalPosistion(blockPos);
            if (Blocks.ContainsKey(blockPos))
                return Blocks[blockPos];
            if (createIfNull)
                return Blocks[blockPos] = new Block(globalBlockPosistion, WorldConfig);

            return null;
        }
        

        public Cluster SetCluster(Cluster cluster)
        {
            return SetCluster(cluster.Bounds.Posistion, cluster);
        }


        private Cluster SetCluster(VectorLong2 position, Cluster cluster)
        {
            var block = GetBlock(position);
            var localClusterPosistion = WorldConfig.GetClusterLocalPosistion(position);

            block.Clusters[localClusterPosistion] = cluster;
            return cluster;
        }

        public Cluster GetCluster(VectorLong2 globalPosistion, bool createIfNull = true)
        {
            var block = GetBlock(globalPosistion);

            var localClusterPosistion = WorldConfig.GetClusterLocalPosistion(globalPosistion);
            if (block.Clusters.ContainsKey(localClusterPosistion))
                return block.Clusters[localClusterPosistion];

            if (createIfNull)
                return block.Clusters[localClusterPosistion] = new Cluster(globalPosistion, WorldConfig);

            return null;
        }

        //public Chunk SetChunk(Chunk chunk)
        //{
        //    return SetChunk(chunk.Bounds.Posistion, chunk);
        //}


        private Chunk SetChunk(VectorLong2 position, Chunk chunk)
        {
            var cluster = GetCluster(position);
            var localChunkPosistion = WorldConfig.GetChunkLocalPosistion(position);
            cluster.Chunks[localChunkPosistion.X, localChunkPosistion.Y] = chunk;
            return chunk;
        }

        public Chunk GetChunk(VectorLong2 globalPosistion, bool createIfNull = true)
        {
            var cluster = GetCluster(globalPosistion);
            var localChunkPosistion = WorldConfig.GetChunkLocalPosistion(globalPosistion);

            if (createIfNull && cluster.Chunks[localChunkPosistion.X, localChunkPosistion.Y] == null)
                return cluster.Chunks[localChunkPosistion.X, localChunkPosistion.Y] = new Chunk(
                    WorldConfig.GetChunkGlobalPosistion(WorldConfig.GetBlockLocalPosistion(globalPosistion),
                        localChunkPosistion, WorldConfig.GetChunkLocalPosistion(globalPosistion)), WorldConfig);

            return cluster.Chunks[localChunkPosistion.X, localChunkPosistion.Y];
        }

    }
}