using System;
using PlatHak.Common.Maths;

namespace PlatHak.Common.World
{
    [Serializable]
    public class Entity
    {
        public Guid NetworkId { get; set; }
        public VectorInt2 Position { get; set; }
        public VectorInt2 Velocity { get; set; }
        [NonSerialized] private WorldConfig _config;

        public Entity()
        {
            NetworkId = Guid.NewGuid();
        }
        public Entity(WorldConfig config) : this()
        {
            _config = config;
        }

        public VectorInt2 GetNextPosition()
        {
            return GetNextPosition(_config.GlobalCoordinatesSize);
        }
        public VectorInt2 GetNextPosition(Size worldMaxSize)
        {
            var newPos = Position + Velocity;
            var worldRect = new Rectangle(new VectorInt2(0, 0), worldMaxSize);
            return !worldRect.Contains(newPos) ? Position : newPos;
        }

        public VectorInt2 UpdatePosition()
        {
            return Position = GetNextPosition();
        }

        public VectorInt2 UpdatePosition(Size worldMaxSize)
        {
            return Position = GetNextPosition(worldMaxSize);
        }
    }
}