using System;
using System.IO;
using System.Runtime.Serialization;
using PlatHak.Common.Interfaces;

namespace PlatHak.Common.Maths
{
    
    public class Size : ISerialize
    {

        public static readonly Size Zero = new Size(0, 0);
        public static readonly Size Empty = Zero;

        public long Width { get; set; }
        public long Height { get; set; }

        public Size(long width, long height)
        {
            Width = width;
            Height = height;
        }

        public Size(Stream stream)
        {
            FromStream(stream);
        }

        public SizeF ToSizeF()
        {
            return new SizeF(Width, Height);
        }
        public Vector2 ToVector2()
        {
            return new Vector2(Width, Height);
        }

        public static Size operator *(Size one, Size two)
        {
            return new Size(one.Width * two.Width, one.Height * two.Width);
        }
        public static Size operator /(Size one, Size two)
        {
            return new Size(one.Width / two.Width, one.Height / two.Width);
        }
        public static Size operator +(Size one, Size two)
        {
            return new Size(one.Width + two.Width, one.Height + two.Width);
        }
        public static Size operator -(Size one, Size two)
        {
            return new Size(one.Width - two.Width, one.Height - two.Width);
        }

        public void ToStream(Stream stream)
        {
            stream.Write(BitConverter.GetBytes(Width), 0, sizeof(long));
            stream.Write(BitConverter.GetBytes(Height), 0, sizeof(long));
        }

        public void FromStream(Stream stream)
        {
            byte[] bytes = new byte[sizeof(long)];
            stream.Read(bytes, 0, sizeof(long));
            Width = BitConverter.ToInt64(bytes, 0);
            stream.Read(bytes, 0, sizeof(long));
            Height = BitConverter.ToInt64(bytes, 0);
        }
    }
}