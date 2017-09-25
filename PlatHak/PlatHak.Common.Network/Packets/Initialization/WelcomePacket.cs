using System;

namespace PlatHak.Common.Network
{
    public class WelcomePacket : Packet
    {
        public string Message { get; set; }
        public Version Version { get; set; }

        public WelcomePacket(string message, Version version)
        {
            Message = message;
            Version = version;
        }
    }
}