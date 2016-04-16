using System;

namespace PlatHak.Common.Network
{
    [Serializable]
    public class WorldPacket : Packet
    {
        /// <summary>
        /// The Config of the World.
        /// </summary>
        public World.WorldConfig Config { get; set; }

        public WorldPacket(World.WorldConfig config) : base(PacketId.World)
        {
            Config = config;
        }
    }
}