using System;

namespace PlatHak.Common.Network
{
    [Serializable]
    public class EventPacket : Packet
    {
        public EventType EventType { get; }
        public EventPacket(EventType type) : base(PacketId.Event)
        {
            EventType = type;
        }
    }

    public enum EventType
    {
        LoginSuccess,
        ClientLoaded
    }
}