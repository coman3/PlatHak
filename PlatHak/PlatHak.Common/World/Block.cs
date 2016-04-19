using System;
using PlatHak.Common.Maths;

namespace PlatHak.Common.World
{
    [Serializable]
    public abstract class Block : IDisposable
    {
        /// <summary>
        /// The location within the <see cref="Chunk"/> and the <see cref="Size"/> of the block (in pixels)
        /// </summary>
        public Rectangle Bounds { get; set; }

        public abstract void Dispose();
    }

    [Serializable]
    public class BlockBlock : Block
    {
        public bool IsSolid { get; set; }
        public override void Dispose()
        {
            
        }
    }
}