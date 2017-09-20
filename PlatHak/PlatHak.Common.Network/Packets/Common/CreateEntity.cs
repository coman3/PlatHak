using System;
using PlatHak.Common.World;

namespace PlatHak.Common.Network
{
   public class CreateEntityPacket : Packet
    {
        public Entity Entity { get; set; }

        public CreateEntityPacket(Entity entity)
        {
            Entity = entity;
        }
    }
}