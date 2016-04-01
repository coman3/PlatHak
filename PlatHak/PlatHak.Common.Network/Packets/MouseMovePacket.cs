using System;
using System.Drawing;

namespace PlatHak.Common.Network
{
    [Serializable]
    public class MouseMovePacket : Packet
    {
        public Point Point { get; set; }
        public MouseMovePacket(Point pos) : base(PacketId.MouseMove)
        {
            Point = pos;
        }
    }
}