using System;
using System.Runtime.Serialization;
using PlatHak.Common.Maths;

namespace PlatHak.Common.World
{
    
    public class WorldConfig
    {
        /// <summary>
        /// A default world config that is setup for the entire world.
        /// </summary>
        public static readonly WorldConfig Default = new WorldConfig(new Size(675, 337), new Size(32, 32), new Size(277, 277), new Size(13, 13), new Size(1, 1), TimeSpan.FromSeconds(1 / 60f));

        
        /// <summary>
        /// The size of each Tile, in meters. 
        /// <example>1x1 meter</example>
        /// </summary>
        public Size ItemSize;

        /// <summary>
        /// The size of each chunk, in Tiles.
        /// </summary>
        public Size ChunkSize;

        /// <summary>
        /// The size of each Cluster, in Chunks.
        /// </summary>
        public Size ClusterSize;

        /// <summary>
        /// The size of each Block, in Clusters.
        /// </summary>
        public Size BlockSize;

        /// <summary>
        /// The size of the world, in blocks
        /// </summary>
        public Size WorldSize;

        /// <summary>
        /// The Max global cord (WorldSize * ChunkSize * ItemSize)
        /// </summary>
        public Size GlobalCoordinatesSize => WorldSize * BlockSize * ClusterSize * ChunkSize;

        public TimeSpan UpdateTimeSpan { get; set; }

        /// <summary>
        /// Creates a new World Config
        /// </summary>
        /// <param name="worldSize">size of the world in Blocks</param>
        /// <param name="blockSize">size of each block in clusters</param>
        /// <param name="clusterSize">size of each cluster in chunks</param>
        /// <param name="chunkSize">size of each chunk in items</param>
        /// <param name="itemSize">size of each item in meters</param>
        /// <param name="updateTimeSpan">interval for game update timer</param>
        private WorldConfig(Size worldSize, Size blockSize, Size clusterSize, Size chunkSize, Size itemSize, TimeSpan updateTimeSpan)
        {
            ItemSize = itemSize;
            ChunkSize = chunkSize;
            ClusterSize = clusterSize;
            BlockSize = blockSize;
            WorldSize = worldSize;

            UpdateTimeSpan = updateTimeSpan;
        }

        public VectorLong2 GetGlobalPosistionFromLatLon(Vector2 latLon)
        {
            var x = GlobalCoordinatesSize.Width * (180 + latLon.Y) / 360;
            var y = GlobalCoordinatesSize.Height * (90 - latLon.X) / 180;
            return new VectorLong2((long) Math.Floor(x), (long) Math.Floor(y));
        }

        public VectorLong2 GetBlockLocalPosistion(VectorLong2 globalPosistion)
        {
            var blockItemSize = BlockSize * ClusterSize * ChunkSize;
            return globalPosistion / blockItemSize;
        }
        public VectorLong2 GetBlockGlobalPosistion(VectorLong2 blockLocalPosistion)
        {
            var blockItemSize = BlockSize * ClusterSize * ChunkSize;
            return blockLocalPosistion * blockItemSize;
        }
        public VectorLong2 GetClusterLocalPosistion(VectorLong2 globalPosistion)
        {
            var localBlockPosistion = GetBlockLocalPosistion(globalPosistion);
            var blockItemSize = BlockSize * ClusterSize * ChunkSize;
            var clusterItemSize = ClusterSize * ChunkSize;
            return (globalPosistion - localBlockPosistion * blockItemSize) / clusterItemSize;
        }
        public VectorLong2 GetClusterGlobalPosistion(VectorLong2 blockLocalPosistion, VectorLong2 clusterLocalPosistion)
        {
            var globalBlockPosistion = GetBlockGlobalPosistion(blockLocalPosistion);
            var clusterItemSize = ClusterSize * ChunkSize;
            return globalBlockPosistion + clusterLocalPosistion * clusterItemSize;
        }

        public VectorLong2 GetChunkLocalPosistion(VectorLong2 globalPosistion)
        {
            var localBlockPosistion = GetBlockLocalPosistion(globalPosistion);
            var localClusterPosistion = GetClusterLocalPosistion(globalPosistion);
            var blockItemSize = BlockSize * ClusterSize * ChunkSize;
            var clusterItemSize = ClusterSize * ChunkSize;
            var chunkItemSize = ChunkSize;
            return ((globalPosistion - localClusterPosistion * clusterItemSize) - localBlockPosistion * blockItemSize) / chunkItemSize;
        }
        public VectorLong2 GetChunkGlobalPosistion(VectorLong2 blockLocalPosistion, VectorLong2 clusterLocalPosistion, VectorLong2 chunkLocalPosistion)
        {
            var globalClusterPosistion = GetClusterGlobalPosistion(blockLocalPosistion, clusterLocalPosistion);
            var chunkItemSize = ChunkSize;
            return globalClusterPosistion + chunkLocalPosistion * chunkItemSize;
        }

        public VectorLong2 GetTileLocalPosistion(VectorLong2 globalPosistion)
        {
            var localBlockPosistion = GetBlockLocalPosistion(globalPosistion);
            var localClusterPosistion = GetClusterLocalPosistion(globalPosistion);
            var localChunkPosistion = GetChunkLocalPosistion(globalPosistion);
            var blockItemSize = BlockSize * ClusterSize * ChunkSize;
            var clusterItemSize = ClusterSize * ChunkSize;
            var chunkItemSize = ChunkSize;
            return globalPosistion - localClusterPosistion * clusterItemSize - localBlockPosistion * blockItemSize - localChunkPosistion * chunkItemSize;
        }
        public VectorLong2 GetTileGlobalPosistion(VectorLong2 blockLocalPosistion, VectorLong2 clusterLocalPosistion, VectorLong2 chunkLocalPosistion, VectorLong2 tileLocalPosistion)
        {
            var globalChunkPosistion = GetChunkGlobalPosistion(blockLocalPosistion, clusterLocalPosistion, chunkLocalPosistion);
            return globalChunkPosistion + tileLocalPosistion;
        }
    }
}