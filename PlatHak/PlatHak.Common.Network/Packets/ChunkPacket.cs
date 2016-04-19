using System;
using PlatHak.Common.World;

namespace PlatHak.Common.Network
{
    [Serializable]
    public class ChunkPacket : Packet
    {
        public Chunk Chunk { get; set; }
        public ChunkPacket(Chunk chunk) : base(PacketId.Chunk)
        {
            Chunk = chunk;
        }
    }
}