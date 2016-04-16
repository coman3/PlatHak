using System;
using SharpDX.Mathematics.Interop;

namespace PlatHak.Common
{
    [Serializable]
    public struct Color
    {
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }
        public byte A { get; set; }

        public Color(byte r, byte g, byte b, byte alpha)
        {
            R = r;
            G = g;
            B = b;
            A = alpha;
        }

        public static Color From(System.Drawing.Color color)
        {
            return new Color(color.R, color.G, color.B, color.A);
        }
        public SharpDX.Color ToSharpDxColor()
        {
            return new SharpDX.Color(R, G, B, A);
        }
    }
}