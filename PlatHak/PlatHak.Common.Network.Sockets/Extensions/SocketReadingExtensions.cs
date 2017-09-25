﻿using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PlatHak.Common.Network.Sockets.Extensions
{
    public static class SocketReadingExtensions
    {
        public static async Task<byte> ReadByteAsync(this Stream s, CancellationToken canceller = default(CancellationToken))
        {
            var buf = new byte[1];
            await s.ReadAsync(buf, 0, 1, canceller);

            return buf[0];
        }

        public static async Task<byte[]> ReadBytesAsync(this Stream s, int byteCount, CancellationToken canceller = default(CancellationToken))
        {
            var buf = new byte[byteCount];
            await s.ReadAsync(buf, 0, byteCount, canceller);
            
            return buf;
        }

        public static int AsInt32(this byte[] bs)
        {
            return BitConverter.ToInt32(bs, 0);
        }

        public static byte[] AsByteArray(this int i)
        {
            return BitConverter.GetBytes(i);
        }

        public static string AsUTF8String(this byte[] bs)
        {
            return Encoding.UTF8.GetString(bs, 0, bs.Length);
        }

        public static byte[] AsUTF8ByteArray(this string s)
        {
            return Encoding.UTF8.GetBytes(s);
        }

        public static string AsJson(this object o)
        {
            return JsonConvert.SerializeObject(o);
        }

        public static byte[] AsUTF8JsonByteArray(this object o)
        {
            return o.AsJson().AsUTF8ByteArray();
        }
    }
}