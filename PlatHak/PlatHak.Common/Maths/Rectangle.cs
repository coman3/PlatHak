using System;
using SharpDX;
using SharpDX.Mathematics.Interop;

namespace PlatHak.Common.Maths
{
    [Serializable]
    public struct Rectangle
    {
        public int X => Posistion.X;
        public int Y => Posistion.Y;
        public VectorInt2 Posistion { get; set; }

        public int Width => Size.Width;
        public int Height => Size.Height;
        public Size Size { get; set; }

        public int Top => Y;
        public int Bottom => Y + Height;
        public int Left => X;
        public int Right => X + Width;

        public VectorInt2 TopLeft => Posistion;
        public VectorInt2 TopRight => new VectorInt2(Right, Top);
        public VectorInt2 BottomLeft => new VectorInt2(Left, Bottom);
        public VectorInt2 BottomRight => new VectorInt2(Right, Bottom);

        public VectorInt2 TopCenter  => new VectorInt2(Center.X, Top);
        public VectorInt2 BottomCenter  => new VectorInt2(Center.X, Bottom);
        public VectorInt2 LeftCenter  => new VectorInt2(Left, Center.Y);
        public VectorInt2 RightCenter  => new VectorInt2(Right, Center.Y);

        public VectorInt2 Center => new VectorInt2(X + Width / 2, Y + Height / 2);


        public RawRectangleF RawRectangleF => new RawRectangleF(Left, Top, Right, Bottom);
        public Rectangle(int x, int y, int width, int height) : this(new VectorInt2(x, y), new Size(width, height)) { }
        public Rectangle(VectorInt2 posistion, Size size) : this()
        {
            Posistion = posistion;
            Size = size;

        }

        public bool Contains(VectorInt2 point, int margin = 0)
        {
            var top = Top - margin;
            var bottom = Bottom + margin;
            var left = Left - margin;
            var right = Right + margin;
            return point.X > left && point.X < right && point.Y > top && point.Y < bottom;
        }

    }
}