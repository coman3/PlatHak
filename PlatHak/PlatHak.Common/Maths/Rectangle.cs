using System;
using System.IO;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using PlatHak.Common.Interfaces;

namespace PlatHak.Common.Maths
{
    
    [JsonObject(MemberSerialization.OptIn)]
    public class Rectangle : ISerialize
    {
        public long X => Posistion.X;
        public long Y => Posistion.Y;
        [JsonProperty]
        public VectorLong2 Posistion { get; set; }

        public long Width => Size.Width;
        public long Height => Size.Height;
        [JsonProperty]
        public Size Size { get; set; }

        public long Top => Y;
        public long Bottom => Y + Height;
        public long Left => X;
        public long Right => X + Width;

        public VectorLong2 TopLeft => Posistion;
        public VectorLong2 TopRight => new VectorLong2(Right, Top);
        public VectorLong2 BottomLeft => new VectorLong2(Left, Bottom);
        public VectorLong2 BottomRight => new VectorLong2(Right, Bottom);

        public VectorLong2 TopCenter  => new VectorLong2(Center.X, Top);
        public VectorLong2 BottomCenter  => new VectorLong2(Center.X, Bottom);
        public VectorLong2 LeftCenter  => new VectorLong2(Left, Center.Y);
        public VectorLong2 RightCenter  => new VectorLong2(Right, Center.Y);

        public VectorLong2 Center => new VectorLong2(X + Width / 2, Y + Height / 2);

        public Rectangle(long x, long y, long width, long height) : this(new VectorLong2(x, y), new Size(width, height)) { }
        public Rectangle(VectorLong2 posistion, Size size)
        {
            Posistion = posistion;
            Size = size;

        }

        public Rectangle(Stream stream)
        {
            FromStream(stream);
        }

        public bool Contains(VectorLong2 point, int margin = 0)
        {
            var top = Top - margin;
            var bottom = Bottom + margin;
            var left = Left - margin;
            var right = Right + margin;
            return point.X > left && point.X < right && point.Y > top && point.Y < bottom;
        }

        public void ToStream(Stream stream)
        {
            Posistion.ToStream(stream);
            Size.ToStream(stream);
        }

        public void FromStream(Stream stream)
        {
            Posistion = new VectorLong2(stream);
            Size = new Size(stream);
        }
    }
}