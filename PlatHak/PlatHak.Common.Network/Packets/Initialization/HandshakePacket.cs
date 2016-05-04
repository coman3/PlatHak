using System;

namespace PlatHak.Common.Network
{
    /// <summary>
    /// A Request for the client to make to the server, to make sure the version of the client and server are the same, or if the client is invaild in some way.
    /// </summary>
    [Serializable]
    public class HandshakePacket : Packet
    {
        public bool Processed { get; set; }
        public Guid ClientId { get; set; }
        public HandshakePacket()
        {
            Processed = false;
        }
    }
}
