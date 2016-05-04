using System;
using SharpDX;

namespace PlatHak.Common.Maths
{
    [Serializable]
    public struct Size
    {

        public static readonly Size Zero = new Size(0, 0);
        public static readonly Size Empty = Zero;

        public int Width { get; set; }
        public int Height { get; set; }

        public Size(int width, int height)
        {
            Width = width;
            Height = height;
        }
        public SizeF ToSizeF()
        {
            return new SizeF(Width, Height);
        }
        public VectorInt2 ToVectorInt2()
        {
            return new VectorInt2(Width, Height);
        }
        public Vector2 ToVector2()
        {
            return new Vector2(Width, Height);
        }

        public static Size operator *(Size one, Size two)
        {
            return new Size(one.Width * two.Width, one.Height * two.Width);
        }
        public static Size operator /(Size one, Size two)
        {
            return new Size(one.Width / two.Width, one.Height / two.Width);
        }
        public static Size operator +(Size one, Size two)
        {
            return new Size(one.Width + two.Width, one.Height + two.Width);
        }
        public static Size operator -(Size one, Size two)
        {
            return new Size(one.Width - two.Width, one.Height - two.Width);
        }

    }
}