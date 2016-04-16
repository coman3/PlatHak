using System;
using PlatHak.Common.Maths;

namespace PlatHak.Common.Network
{
    [Serializable]
    public class MoveRequest : Packet
    {
        public bool Accepted { get; set; }
        public VectorInt2 NewPosistion { get; set; }
        
        public MoveRequest() : base(PacketId.MoveRequest)
        {
        }
    }
}