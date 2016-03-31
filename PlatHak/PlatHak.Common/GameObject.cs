using System;

namespace PlatHak.Common
{
    [Serializable]
    public abstract class GameObject
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Vector2 Posistion { get; private set; }

        protected GameObject(Guid id, string name, Vector2 posistion)
        {
            Id = id;
            Name = name;
            Posistion = posistion;
        }
        public virtual Vector2 SetPos(Vector2 value)
        {
            Posistion = value;
            return Posistion;
        }

        public virtual Vector2 Move(Vector2 newOffset)
        {
            Posistion = new Vector2(Posistion.X + newOffset.X, Posistion.Y + newOffset.Y);
            return Posistion;
        }
    }
}