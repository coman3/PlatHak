using System;
using Newtonsoft.Json;
using PlatHak.Common.Maths;
using PlatHak.Common.World;

namespace PlatHak.Server.Common
{
    
    [JsonObject(MemberSerialization.OptIn)]
    public class ChunkCluster : IDisposable
    {
        [JsonProperty]
        public Chunk[,] Chunks { get; set; }
        [JsonProperty]
        public VectorLong2 LocalPosistion { get; set; }
        [JsonProperty]
        public Rectangle WorldPosistion { get; set; }

        public Chunk this[int x, int y]
        {
            get { return Chunks[x, y]; }
            set { Chunks[x, y] = value; }
        }
        
        public bool Full => false;

        public ChunkCluster()
        {
            
        }
        public ChunkCluster(Size size)
        {
            Chunks = new Chunk[size.Width, size.Height];
        }

        public ChunkCluster(Chunk[,] chunks, VectorLong2 localPosistion, Rectangle worldPosistion)
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