using System;
using System.IO;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using PlatHak.Common.Interfaces;

namespace PlatHak.Common.Maths
{
    
    [JsonObject(MemberSerialization.OptIn)]
    public class VectorLong2 : IEquatable<VectorLong2>, IComparable<VectorLong2>, ISerialize
    {
        public static readonly VectorLong2 Negative = new VectorLong2(-1, -1);
        public static readonly VectorLong2 Zero = new VectorLong2(0, 0);
        public static readonly VectorLong2 One = new VectorLong2(1, 1);
        public static readonly VectorLong2 Up = new VectorLong2(0, 1);
        public static readonly VectorLong2 Down = new VectorLong2(0, -1);
        public static readonly VectorLong2 Left = new VectorLong2(-1, 0);
        public static readonly VectorLong2 Right = new VectorLong2(1, 0);

        [JsonProperty]
        public long X { get; set; }
        [JsonProperty]
        public long Y { get; set; }
        public VectorLong2(long x, long y)
        {
            X = x;
            Y = y;
        }

        public VectorLong2(Stream stream)
        {
            FromStream(stream);
        }

        public Vector2 ToVector2()
        {
            return new Vector2(X, Y);
        }

        public int Distance(VectorLong2 point2)
        {
            return (int) Math.Sqrt(Math.Pow(X - point2.X, 2) + Math.Pow(Y - point2.Y, 2));
        }
        #region Operators

        public static bool operator ==(VectorLong2 one, VectorLong2 two)
        {
            return one.X == two.X && one.Y == two.Y;
        }

        public static bool operator !=(VectorLong2 one, VectorLong2 two)
        {
            return !(one == two);
        }

        public static bool operator >(VectorLong2 one, VectorLong2 two)
        {
            return one.X > two.X && one.Y > two.Y;
        }

        public static bool operator <(VectorLong2 one, VectorLong2 two)
        {
            return one.X < two.X && one.Y < two.Y;
        }

        public static VectorLong2 operator +(VectorLong2 one, VectorLong2 two)
        {
            return new VectorLong2(one.X + two.X, one.Y + two.Y);
        }

        public static VectorLong2 operator -(VectorLong2 one, VectorLong2 two)
        {
            return new VectorLong2(one.X - two.X, one.Y - two.Y);
        }
        public static VectorLong2 operator -(VectorLong2 one)
        {
            return new VectorLong2(-one.X, -one.Y);
        }

        public static VectorLong2 operator *(VectorLong2 one, VectorLong2 two)
        {
            return new VectorLong2(one.X*two.X, one.Y*two.Y);
        }
        public static VectorLong2 operator *(VectorLong2 one, Size two)
        {
            return new VectorLong2(one.X * two.Width, one.Y * two.Height);
        }

        public static VectorLong2 operator /(VectorLong2 one, Size two)
        {
            return new VectorLong2(one.X/two.Width, one.Y/two.Height);
        }
        public static VectorLong2 operator /(VectorLong2 one, VectorLong2 two)
        {
            return new VectorLong2(one.X / two.X, one.Y / two.Y);
        }

        public static VectorLong2 operator +(VectorLong2 one, int two)
        {
            return new VectorLong2(one.X + two, one.Y + two);
        }

        public static VectorLong2 operator -(VectorLong2 one, int two)
        {
            return new VectorLong2(one.X - two, one.Y - two);
        }

        #endregion

        #region Overrides

        public override string ToString()
        {
            return $"X:{X} Y:{Y}";
        }

        public bool Equals(VectorLong2 other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y);
        }

        public int CompareTo(VectorLong2 other)
        {
            if (other > this) return 1;
            if (other == this) return 0;
            return -1;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is VectorLong2 && Equals((VectorLong2) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X.GetHashCode()*397) ^ Y.GetHashCode();
            }
        }

        #endregion

        public void ToStream(Stream stream)
        {
            stream.Write(BitConverter.GetBytes(X), 0, sizeof(long));
            stream.Write(BitConverter.GetBytes(Y), 0, sizeof(long));
        }

        public void FromStream(Stream stream)
        {
            byte[] bytes = new byte[sizeof(long)];
            stream.Read(bytes, 0, sizeof(long));
            X = BitConverter.ToInt64(bytes, 0);
            stream.Read(bytes, 0, sizeof(long));
            Y = BitConverter.ToInt64(bytes, 0);
        }
    }

}