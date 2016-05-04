using System;
using SharpDX;

namespace PlatHak.Common.Maths
{
    [Serializable]
    public struct SizeF
    {

        public static readonly SizeF Zero = new SizeF(0, 0);
        public static readonly SizeF Empty = Zero;

        public float Width { get; set; }
        public float Height { get; set; }

        public SizeF(float width, float height)
        {
            Width = width;
            Height = height;
        }

        public Size ToSizeF()
        {
            return new Size ((int)Width, (int)Height);
        }
        public Vector2 ToVector2()
        {
            return new Vector2(Width, Height);
        }

        public static SizeF operator *(SizeF one, SizeF two)
        {
            return new SizeF(one.Width * two.Width, one.Height * two.Width);
        }
        public static SizeF operator /(SizeF one, SizeF two)
        {
            return new SizeF(one.Width / two.Width, one.Height / two.Width);
        }
        public static SizeF operator /(SizeF one, float two)
        {
            return new SizeF(one.Width / two, one.Height / two);
        }
        public static SizeF operator +(SizeF one, SizeF two)
        {
            return new SizeF(one.Width + two.Width, one.Height + two.Width);
        }
        public static SizeF operator -(SizeF one, SizeF two)
        {
            return new SizeF(one.Width - two.Width, one.Height - two.Width);
        }
    }
}