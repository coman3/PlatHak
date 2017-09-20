using System;
using System.Runtime.Serialization;
using PlatHak.Common.Maths;

namespace PlatHak.Common.World
{
    
    public class Entity
    {
        [DataMember]
        public Guid NetworkId { get; set; }
        [DataMember]
        public VectorLong2 Position { get; set; }
        [DataMember]
        public VectorLong2 Velocity { get; set; }
        private WorldConfig _config;

        public Entity()
        {
            NetworkId = Guid.NewGuid();
        }
        public Entity(WorldConfig config) : this()
        {
            _config = config;
        }

        public VectorLong2 GetNextPosition()
        {
            return GetNextPosition(_config.GlobalCoordinatesSize);
        }
        public VectorLong2 GetNextPosition(Size worldMaxSize)
        {
            var newPos = Position + Velocity;
            var worldRect = new Rectangle(new VectorLong2(0, 0), worldMaxSize);
            return !worldRect.Contains(newPos) ? Position : newPos;
        }

        public VectorLong2 UpdatePosition()
        {
            return Position = GetNextPosition();
        }

        public VectorLong2 UpdatePosition(Size worldMaxSize)
        {
            return Position = GetNextPosition(worldMaxSize);
        }
    }
}