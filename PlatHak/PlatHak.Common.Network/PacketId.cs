using System;

namespace PlatHak.Common.Network
{
    [Serializable]
    public enum PacketId : byte
    {
        Handshake,
        Error,
        Multi,
        Event,
        Login,


        MouseMove
    }
}