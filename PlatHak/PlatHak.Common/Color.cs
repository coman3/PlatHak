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
        public byte Alpha { get; set; }

        public Color(byte r, byte g, byte b, byte alpha)
        {
            R = r;
            G = g;
            B = b;
            Alpha = alpha;
        }

        public RawColor4 ToRawColor4()
        {
            return new RawColor4(R, G, B, Alpha);
        }
    }
}