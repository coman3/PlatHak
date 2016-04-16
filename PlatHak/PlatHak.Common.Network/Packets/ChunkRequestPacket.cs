using System;
using PlatHak.Common.Maths;

namespace PlatHak.Common.Network
{
    [Serializable]
    public class ChunkRequestPacket : Packet
    {
        public VectorInt2 ChunkPosistion { get; set; }
        public ChunkRequestPacket() : base(PacketId.ChunkRequest)
        {
        }
    }
}