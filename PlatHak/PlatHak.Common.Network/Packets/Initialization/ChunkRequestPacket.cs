using System;
using PlatHak.Common.Maths;

namespace PlatHak.Common.Network
{
    /// <summary>
    /// Requests for a chunk from the server, at the given Chunk Cord.
    /// </summary>
    [Serializable]
    public class ChunkRequestPacket : Packet
    {
        public VectorInt2 ChunkPosistion { get; set; }
    }
}