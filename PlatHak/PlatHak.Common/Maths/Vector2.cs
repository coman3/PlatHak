using System;
using Newtonsoft.Json;
using SharpDX.Mathematics.Interop;

namespace PlatHak.Common.Maths
{
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public struct Vector2 : IEquatable<Vector2>, IComparable<Vector2>
    {
        public static readonly Vector2 Zero = new Vector2(0, 0);
        public static readonly Vector2 One = new Vector2(1, 1);
        public static readonly Vector2 Up = new Vector2(0, 1);
        public static readonly Vector2 Down = new Vector2(0, -1);
        public static readonly Vector2 Left = new Vector2(-1, 0);
        public static readonly Vector2 Right = new Vector2(1, 0);
        [JsonProperty]
        public float X { get; set; }
        [JsonProperty]
        public float Y { get; set; }
        public RawVector2 RawVector2 => new RawVector2(X, Y);
        public VectorInt2 VectorInt2 => new VectorInt2((int) X, (int) Y);
        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        public float Distance(Vector2 point2)
        {
            return (float) Math.Sqrt(Math.Pow(X - point2.X, 2) + Math.Pow(Y - point2.Y, 2));
        }

        #region Operators

        public static bool operator ==(Vector2 one, Vector2 two)
        {
            return Math.Abs(one.X - two.X) < float.Epsilon && Math.Abs(one.Y - two.Y) < float.Epsilon;
        }

        public static bool operator !=(Vector2 one, Vector2 two)
        {
            return !(one == two);
        }

        public static bool operator >(Vector2 one, Vector2 two)
        {
            return one.X > two.X && one.Y > two.Y;
        }

        public static bool operator <(Vector2 one, Vector2 two)
        {
            return one.X < two.X && one.Y < two.Y;
        }

        public static bool operator <=(Vector2 one, Vector2 two)
        {
            return one.X <= two.X && one.Y <= two.Y;
        }

        public static bool operator >=(Vector2 one, Vector2 two)
        {
            return one.X >= two.X && one.Y >= two.Y;
        }

        public static Vector2 operator +(Vector2 one, Vector2 two)
        {
            return new Vector2(one.X + two.X, one.Y + two.Y);
        }

        public static Vector2 operator -(Vector2 one, Vector2 two)
        {
            return new Vector2(one.X - two.X, one.Y - two.Y);
        }

        public static Vector2 operator *(Vector2 one, Vector2 two)
        {
            return new Vector2(one.X*two.X, one.Y*two.Y);
        }

        public static Vector2 operator /(Vector2 one, Vector2 two)
        {
            return new Vector2(one.X/two.X, one.Y/two.Y);
        }

        #endregion

        #region Overrides

        public override string ToString()
        {
            return $"X:{X} Y:{Y}";
        }

        public bool Equals(Vector2 other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y);
        }

        public int CompareTo(Vector2 other)
        {
            if (other > this) return 1;
            if (other == this) return 0;
            return -1;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Vector2 && Equals((Vector2) obj);
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