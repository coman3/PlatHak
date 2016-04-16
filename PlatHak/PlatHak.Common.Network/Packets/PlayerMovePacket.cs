using System;
using PlatHak.Common.Maths;

namespace PlatHak.Common.Network
{
    [Serializable]
    public class PlayerMovePacket : Packet
    {
        public string Username { get; set; }
        public VectorInt2 NewPosistion { get; set; }

        public PlayerMovePacket(string username, VectorInt2 newPosistion) : base(PacketId.PlayerMove)
        {
            Username = username;
            NewPosistion = newPosistion;
        }
    }
}