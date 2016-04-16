using System;
using PlatHak.Common.Maths;

namespace PlatHak.Common.World
{
    [Serializable]
    public abstract class GridItem : IDisposable
    {
        /// <summary>
        /// The location within the <see cref="WorldGrid"/> and the <see cref="Size"/> of the block (in pixels)
        /// </summary>
        public Rectangle Bounds { get; set; }

        public abstract void Dispose();
    }

    [Serializable]
    public class BlockGridItem : GridItem
    {
        public bool IsSolid { get; set; }
        public override void Dispose()
        {
            
        }
    }
}