using System;

namespace PlatHak.Common.Network
{
    [Serializable]
    public enum PacketId : byte
    {
        Handshake,
        Error,
        MultiPacket,

        CreateObject,
        DeleteObject,
        MoveObject,

        
        Input,
        Event,
        Login
    }
}