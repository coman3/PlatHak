using System;
using PlatHak.Common.Maths;

namespace PlatHak.Common.Objects
{
    [Serializable]
    public class CircleObject : GameObject
    {
        public int Radius { get; set; }
        public CircleObject(string name, Vector2 posistion, int radius) : base(Guid.NewGuid(), name, posistion)
        {
            Radius = radius;
        }
    }
}