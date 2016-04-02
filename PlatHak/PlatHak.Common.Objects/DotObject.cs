using System;
using PlatHak.Common.Maths;

namespace PlatHak.Common.Objects
{
    [Serializable]
    public class DotObject : CircleObject
    {
        public Color Color { get; set; }

        public DotObject(Vector2 posistion, Vector2 velocity, int radius, Color color) : base("World_Dot", posistion, radius)
        {
            Velocity = velocity;
            Color = color;
        }
    }
}