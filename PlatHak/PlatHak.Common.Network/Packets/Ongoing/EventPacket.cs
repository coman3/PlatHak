using System;

namespace PlatHak.Common.Network
{
    /// <summary>
    /// Contains an event that has occured, for the client or server to proccess.
    /// </summary>
    [Serializable]
    public class EventPacket : Packet
    {
        public EventType EventType { get; }
        public EventPacket(EventType type)
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