using System;

namespace PlatHak.Common.Network
{
    /// <summary>
    /// Contains info about a server error that occured
    /// </summary>
    
    public class ErrorPacket : Packet
    {
        public ConnectionException Exception { get; set; }
        public ErrorPacket(ConnectionException exception)
        {
            Exception = exception;
        }
    }
    
    
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