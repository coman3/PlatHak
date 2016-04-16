using System;
using SharpDX.Mathematics.Interop;

namespace PlatHak.Common.Maths
{
    [Serializable]
    public struct VectorInt2 : IEquatable<VectorInt2>, IComparable<VectorInt2>
    {
        public static readonly VectorInt2 Zero = new VectorInt2(0, 0);
        public static readonly VectorInt2 One = new VectorInt2(1, 1);
        public static readonly VectorInt2 Up = new VectorInt2(0, 1);
        public static readonly VectorInt2 Down = new VectorInt2(0, -1);
        public static readonly VectorInt2 Left = new VectorInt2(-1, 0);
        public static readonly VectorInt2 Right = new VectorInt2(1, 0);

        public int X { get; set; }
        public int Y { get; set; }
        public RawVector2 RawVectorInt2 => new RawVector2(X, Y);

        public VectorInt2(int x, int y)
        {
            X = x;
            Y = y;
        }

        #region Operators

        public static bool operator ==(VectorInt2 one, VectorInt2 two)
        {
            return one.X == two.X && one.Y == two.Y;
        }

        public static bool operator !=(VectorInt2 one, VectorInt2 two)
        {
            return !(one == two);
        }

        public static bool operator >(VectorInt2 one, VectorInt2 two)
        {
            return one.X > two.X && one.Y > two.Y;
        }

        public static bool operator <(VectorInt2 one, VectorInt2 two)
        {
            return one.X < two.X && one.Y < two.Y;
        }

        public static VectorInt2 operator +(VectorInt2 one, VectorInt2 two)
        {
            return new VectorInt2(one.X + two.X, one.Y + two.Y);
        }

        public static VectorInt2 operator -(VectorInt2 one, VectorInt2 two)
        {
            return new VectorInt2(one.X - two.X, one.Y - two.Y);
        }

        public static VectorInt2 operator *(VectorInt2 one, VectorInt2 two)
        {
            return new VectorInt2(one.X*two.X, one.Y*two.Y);
        }

        public static VectorInt2 operator /(VectorInt2 one, VectorInt2 two)
        {
            return new VectorInt2(one.X/two.X, one.Y/two.Y);
        }

        #endregion

        #region Overrides

        public override string ToString()
        {
            return $"X:{X} Y:{Y}";
        }

        public bool Equals(VectorInt2 other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y);
        }

        public int CompareTo(VectorInt2 other)
        {
            if (other > this) return 1;
            if (other == this) return 0;
            return -1;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is VectorInt2 && Equals((VectorInt2) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X.GetHashCode()*397) ^ Y.GetHashCode();
            }
        }

        #endregion
    }

}