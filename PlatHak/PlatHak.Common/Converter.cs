using System;
using System.Security.Cryptography;
using System.Text;

namespace PlatHak.Common
{
    public static class Converter
    {
        #region To Bytes

        public static byte[] ToBytes(this int value)
        {
            return BitConverter.GetBytes(value);
        }

        public static byte[] ToBytes(this bool value)
        {
            return BitConverter.GetBytes(value);
        }

        public static byte[] ToBytes(this float value)
        {
            return BitConverter.GetBytes(value);
        }

        public static byte[] ToBytes(this double value)
        {
            return BitConverter.GetBytes(value);
        }

        public static byte[] ToBytes(this string value)
        {
            return Encoding.UTF8.GetBytes(value);
        }

        public static string ToMd5(this string value)
        {
            return Encoding.Default.GetString(MD5.Create().ComputeHash(Encoding.Default.GetBytes(value)));
        }

        #endregion

        #region To Value

        public static int ToInt(this byte[] value, int startIndex = 0)
        {
            return BitConverter.ToInt32(value, startIndex);
        }

        public static float ToFloat(this byte[] value, int startIndex = 0)
        {
            return BitConverter.ToSingle(value, startIndex);
        }

        public static double ToDouble(this byte[] value, int startIndex = 0)
        {
            return BitConverter.ToDouble(value, startIndex);
        }

        public static bool ToBool(this byte[] value, int startIndex = 0)
        {
            return BitConverter.ToBoolean(value, startIndex);
        }

        public static string ToUtf8String(this byte[] value)
        {
            return Encoding.UTF8.GetString(value);
        }

        #endregion
    }
}