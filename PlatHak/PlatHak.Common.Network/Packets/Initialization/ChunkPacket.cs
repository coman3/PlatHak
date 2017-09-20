using System;
using PlatHak.Common.World;

namespace PlatHak.Common.Network
{
    /// <summary>
    /// Contains chunk data from the server
    /// </summary>
    public class ChunkPacket : Packet
    {
        public Chunk Chunk { get; set; }
        public ChunkPacket(Chunk chunk)
        {
            Chunk = chunk;
        }
    }
}