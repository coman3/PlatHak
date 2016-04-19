using System;
using PlatHak.Common.Maths;

namespace PlatHak.Common.Network
{
    [Serializable]
    public class MoveRequest : Packet
    {
        public MoveType MoveType { get; set; }
        //If the player is requesing to move(true) or to stop moving(false) in that direction
        public bool State { get; set; }
        public MoveRequest() : base(PacketId.MoveRequest)
        {
        }
    }

    public enum MoveType
    {
        Up,
        Down,
        Left,
        Right
    }
}