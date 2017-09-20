using System;
using PlatHak.Common.Maths;

namespace PlatHak.Common.Network
{
    /// <summary>
    /// Requests for a chunk from the server, at the given Chunk Cord.
    /// </summary>
    
    public class ChunkRequestPacket : Packet
    {
        public VectorLong2 ChunkPosistion { get; set; }
    }
}