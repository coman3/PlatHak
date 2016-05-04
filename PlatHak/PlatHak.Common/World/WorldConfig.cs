using System;
using PlatHak.Common.Maths;
using ProtoBuf;

namespace PlatHak.Common.World
{
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

        /// <summary>
        /// The Max global cord (WorldSize * ChunkSize * ItemSize)
        /// </summary>
        public Size GlobalCoordinatesSize => WorldSize * ChunkSize * ItemSize;

        public TimeSpan UpdateTimeSpan { get; set; }

        public WorldConfig(Size worldSize, Size chunkSize, Size itemSize, TimeSpan updateTimeSpan)
        {
            WorldSize = worldSize;
            ChunkSize = chunkSize;
            ItemSize = itemSize;
            UpdateTimeSpan = updateTimeSpan;
        }
    }
}