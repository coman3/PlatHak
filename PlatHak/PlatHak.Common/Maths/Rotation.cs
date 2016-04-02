using System;
using SharpDX;
using SharpDX.Mathematics.Interop;

namespace PlatHak.Common.Maths
{
    public struct Rotation
    {
        public static readonly Rotation Zero = new Rotation(new Vector2(0, 0), 0);

        public Vector2 Anchor { get; set; }
        public float Degrees { get; set; }

        public Matrix3x2 Matrix3X2 => Matrix3x2.Rotation(Radians, Anchor.RawVector2);
        public float Radians => ConvertToRadians(Degrees);

        public Rotation(Vector2 anchor, float degrees)
        {
            Anchor = anchor;
            Degrees = degrees;
        }

        private static float ConvertToRadians(double angle)
        {
            return (float)(Math.PI / 180 * angle);
        }

    }
}
