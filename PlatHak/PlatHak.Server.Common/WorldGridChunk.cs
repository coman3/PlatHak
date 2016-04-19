using System;
using PlatHak.Common.Maths;
using PlatHak.Common.World;

namespace PlatHak.Server.Common
{
    [Serializable]
    public class ChunkCluster : IDisposable
    {
        public Chunk[,] Chunks { get; set; }
        public VectorInt2 LocalPosistion { get; set; }
        public Rectangle WorldPosistion { get; set; }

        public Chunk this[int x, int y]
        {
            get { return Chunks[x, y]; }
            set { Chunks[x, y] = value; }
        }
        public bool Full => false;

        public ChunkCluster(Size size)
        {
            Chunks = new Chunk[size.Width, size.Height];
        }

        public ChunkCluster(Chunk[,] chunks, VectorInt2 localPosistion, Rectangle worldPosistion)
        {
            Chunks = chunks;
            LocalPosistion = localPosistion;
            WorldPosistion = worldPosistion;
        }

        public void Dispose()
        {
            if(Chunks == null) return;
            foreach (var worldGrid in Chunks)
            {
                worldGrid.Dispose();
            }
            Chunks = null;
        }
    }
}