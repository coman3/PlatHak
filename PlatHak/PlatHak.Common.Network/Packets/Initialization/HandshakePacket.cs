using System;
using PlatHak.Common.Enums;
using PlatHak.Common.Maths;

namespace PlatHak.Common.Network
{
    /// <summary>
    /// A Request for the client to make to the server, to make sure the version of the client and server are the same, or if the client is invaild in some way.
    /// </summary>
    
    public class HandshakeRequestPacket : Packet
    {
        public Rectangle ViewPort { get; set; }
        public Device Device { get; set; }
        public Guid SoftwareId { get; set; }
        public HandshakeRequestPacket(Rectangle viewPort)
        {
            ViewPort = viewPort;
        }
    }

    public class HandshakeResponcePacket : Packet
    {
        public bool Valid { get; set; }
        public DateTime TimeValidated { get; set; }
    }
}
