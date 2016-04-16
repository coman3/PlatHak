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
    }
}