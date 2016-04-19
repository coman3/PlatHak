using System;
using Box2DX.Dynamics;
using PlatHak.Common.Maths;

namespace PlatHak.Common.World
{
    [Serializable]
    public class Player
    {
        public string Username { get; set; }
        public VectorInt2 Posistion { get; set; }
        public VectorInt2 Velocity { get; set; }

        public VectorInt2 GetNextPosistion(Size worldMaxSize)
        {
            var newPos = Posistion + Velocity;
            var worldRect = new Rectangle(new VectorInt2(0, 0), worldMaxSize);
            return !worldRect.Contains(newPos) ? Posistion : newPos;
        }
        public VectorInt2 UpdatePosistion(Size worldMaxSize)
        {
            var newPos = Posistion + Velocity;
            var worldRect = new Rectangle(new VectorInt2(0, 0), worldMaxSize);
            if (!worldRect.Contains(newPos)) return Posistion;
            return Posistion = newPos;
        }

    }
}