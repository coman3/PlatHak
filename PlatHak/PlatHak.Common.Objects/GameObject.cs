using System;

namespace PlatHak.Common.Objects
{
    [Serializable]
    public abstract class GameObject
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Vector2 Velocity { get; set; }
        public Vector2 Acceleration { get; set; }
        public Vector2 Posistion { get; set; }

        protected GameObject(Guid id, string name, Vector2 posistion)
        {
            Id = id;
            Name = name;
            Posistion = posistion;
        }
    }
}