using System;
using SharpDX;
using SharpDX.Mathematics.Interop;

namespace PlatHak.Common.Maths
{
    [Serializable]
    public struct RectangleF
    {

        public float X => Posistion.X;
        public float Y => Posistion.Y;
        public Vector2 Posistion { get; set; }

        public float Width => Size.Width;
        public float Height => Size.Height;
        public SizeF Size { get; set; }

        public float Top => Y;
        public float Bottom => Y + Height;
        public float Left => X;
        public float Right => X + Width;

        public Vector2 TopLeft => Posistion;
        public Vector2 TopRight => new Vector2(Right, Top);
        public Vector2 BottomLeft => new Vector2(Left, Bottom);
        public Vector2 BottomRight => new Vector2(Right, Bottom);

        public Vector2 TopCenter  => new Vector2(Center.X, Top);
        public Vector2 BottomCenter  => new Vector2(Center.X, Bottom);
        public Vector2 LeftCenter  => new Vector2(Left, Center.Y);
        public Vector2 RightCenter  => new Vector2(Right, Center.Y);

        public Vector2 Center => new Vector2(X + Width / 2f, Y + Height / 2f);


        public RawRectangleF RawRectangleF => new RawRectangleF(Left, Top, Right, Bottom);
        public RectangleF(float x, float y, float width, float height) : this(new Vector2(x, y), new SizeF(width, height)) { }
        public RectangleF(Vector2 posistion, SizeF size) : this()
        {
            Posistion = posistion;
            Size = size;

        }
        /// <summary>
        /// Checks if the current rectangle contains all 4 corners of the specifyed rectangle
        /// </summary>
        /// <param name="rectangle"></param>
        /// <returns>True if all corners are within the bounds of the current rectangle</returns>
        public bool Contains(RectangleF rectangle)
        {
            return Contains(rectangle.TopLeft) && Contains(rectangle.BottomRight) &&
                   Contains(rectangle.TopRight) && Contains(rectangle.BottomLeft);
        }
        /// <summary>
        /// Checks if the current rectangle contains any corner of the specifyed rectangle
        /// </summary>
        /// <param name="rectangle"></param>
        /// <returns>True if a corner is within the bounds of the current rectangle</returns>
        public bool ContainsCorner(RectangleF rectangle)
        {
            return Contains(rectangle.TopLeft) || Contains(rectangle.BottomRight) ||
                   Contains(rectangle.TopRight) || Contains(rectangle.BottomLeft);
        }
        public bool Contains(Vector2 point, float margin = 0)
        {
            var top = Top - margin;
            var bottom = Bottom + margin;
            var left = Left - margin;
            var right = Right + margin;
            return point.X >= left && point.X <= right && point.Y >= top && point.Y <= bottom;
        }

    }
}