using System;

namespace PlatHak.Common.Network
{
    [Serializable]
    public class ErrorPacket : Packet
    {
        public ConnectionException Exception { get; set; }
        public ErrorPacket(ConnectionException exception) : base(PacketId.Error)
        {
            Exception = exception;
        }
    }
}