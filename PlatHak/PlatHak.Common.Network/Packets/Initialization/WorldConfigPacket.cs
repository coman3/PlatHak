using System;

namespace PlatHak.Common.Network
{
    /// <summary>
    /// Contains infomation of the world, for the client.
    /// </summary>
    [Serializable]
    public class WorldConfigPacket : Packet
    {
        /// <summary>
        /// The Config of the World.
        /// </summary>
        public World.WorldConfig Config { get; set; }

        public WorldConfigPacket(World.WorldConfig config)
        {
            Config = config;
        }
    }
}