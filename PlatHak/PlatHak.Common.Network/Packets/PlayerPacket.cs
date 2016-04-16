using System;
using PlatHak.Common.World;

namespace PlatHak.Common.Network
{
    [Serializable]
    public class PlayerPacket : Packet
    {
        public Player Player { get; set; }
        public PlayerPacket(Player player) : base(PacketId.Player)
        {
            Player = player;
        }
    }
}