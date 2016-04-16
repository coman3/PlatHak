using System;
using PlatHak.Common;
using PlatHak.Common.Maths;
using PlatHak.Common.World;

namespace PlatHak.Common.Network
{
    [Serializable]
    public class UpdateGridItem : Packet
    {
        /// <summary>
        /// The Location of the Chunk
        /// </summary>
        public VectorInt2 ChunkPosistion { get; set; }
        /// <summary>
        /// The location within the chunk
        /// </summary>
        public VectorInt2 BlockPosistion { get; set; }
        /// <summary>
        /// new new GridItem
        /// </summary>
        public World.GridItem GridItem { get; set; }

        public UpdateGridItem(VectorInt2 chunkPosistion, VectorInt2 blockPosistion, GridItem gridItem) : this()
        {
            ChunkPosistion = chunkPosistion;
            BlockPosistion = blockPosistion;
            GridItem = gridItem;
        }
        private UpdateGridItem() : base(PacketId.UpdateGridItem)
        {
        }
    }
}