using System;
using PlatHak.Common.World;

namespace PlatHak.Common.Network
{
    [Serializable]
    public class ChunkPacket : Packet
    {
        public WorldGrid Chunk { get; set; }
        public ChunkPacket(WorldGrid chunk) : base(PacketId.Chunk)
        {
            Chunk = chunk;
        }
    }
}