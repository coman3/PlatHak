using System;
using System.Security.AccessControl;
using PlatHak.Common.Maths;

namespace PlatHak.Common.Network
{
    [Serializable]
    public class PlayerMovePacket : Packet
    {
        public string Username { get; set; }
        public VectorInt2 NewVelocity { get; set; }
        public VectorInt2 ServerPosistion { get; set; }
        public PlayerMovePacket(string username, VectorInt2 newVelocity) : base(PacketId.PlayerMove)
        {
            Username = username;
            NewVelocity = newVelocity;
        }
    }
}