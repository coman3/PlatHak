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
    
    [Serializable]
    public class ConnectionException
    {
        public ErrorType Type { get; set; }
        public string Message { get; set; }
        public ConnectionException(ErrorType type, string message)
        {
            Message = message;
            Type = type;
        }

        public override string ToString()
        {
            return $"Error: {Type}, {Message}";
        }
    }

    public enum ErrorType
    {
        NoHandshake,
        ObjectRemoved,
        PacketIdNotFound,
        UsernameInUse,
        PingToHigh
    }
}