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
    }
}