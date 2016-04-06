using System;

namespace PlatHak.Common.Network
{
    [Serializable]
    public class HandshakePacket : Packet
    {
        public bool Processed { get; set; }
        public Guid ClientId { get; set; }
        public HandshakePacket() : base(PacketId.Handshake)
        {
            Processed = false;
        }
    }
}
