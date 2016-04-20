using System;
using System.Collections.Generic;
using PlatHak.Common.Maths;

namespace PlatHak.Common.World
{
    [Serializable]
    public class Chunk : IDisposable
    {
        public Rectangle Bounds { get; set; }
        public Block[,] Items { get; set; }
        public Size ItemSize { get; set; }

        public Chunk(VectorInt2 chunkPos, WorldConfig config)
        {
            Bounds = new Rectangle(chunkPos, config.ChunkSize);
            ItemSize = config.ItemSize;
            Items = new Block[Bounds.Width, Bounds.Height];
        }

        public T AddGridItem<T>(int x, int y, T gridItem) where T : Block
        {
            gridItem.Bounds = new Rectangle(x, y, ItemSize.Width, ItemSize.Height);
            if (Items[x, y] != null)
            {
                Items[x, y].Dispose();
            }
            Items[x, y] = gridItem;
            return gridItem;
        }

        public static Rectangle GetGridItemBounds(int x, int y, Size size)
        {
            if (x < 0) throw new IndexOutOfRangeException("x value out of range");
            if (y < 0) throw new IndexOutOfRangeException("y value out of range");

            return new Rectangle(new VectorInt2(x + x * size.Width, y + y * size.Height), size);
        }

        public void Dispose()
        {
            if(Items == null) return;
            foreach (var gridItem in Items)
            {
                gridItem?.Dispose();
            }
            Items = null;
        }
    }
}