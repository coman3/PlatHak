using System;
using System.Collections.Generic;
using System.Linq;

namespace PlatHak.Common.Network
{
    [Serializable]
    public class MultiPacket : Packet
    {
        public List<Packet> Packets { get; set; }
        public MultiPacket(params Packet[] packets) : base(PacketId.MultiPacket)
        {
            Packets = packets?.ToList() ?? new List<Packet>();
        }
    }
}