using Box2DX.Common;

namespace PlatHak.Common.Maths
{
    public static class Extentions
    {
        public static Vector2 GetVector2(this Vec2 vec2)
        {
            return new Vector2(vec2.X, vec2.Y);
        }

        public static Rotation GetRotation(this XForm xform)
        {
            return new Rotation(xform.Position.GetVector2(), xform.R.GetAngle());
        }
    }
}