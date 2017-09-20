using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using PlatHak.Common.Interfaces;
using PlatHak.Common.Maths;

namespace PlatHak.Common.World
{
    
    public struct Tile : ISerialize
    {
        //// <summary>
        //// The location within the <see cref="Chunk"/> and the <see cref="Size"/> of the block (in pixels)
        //// </summary>
        //public Rectangle Bounds { get; set; }

        public byte Height { get; set; }
        public int BlockId { get; set; }

        public Tile(byte height, int blockId)
        {
            Height = height;
            BlockId = blockId;
        }

        public Tile(Stream stream)
        {
            Height = 0;
            BlockId = 0;
            FromStream(stream);
        }

        private const byte Bytes = 5;
        public void ToStream(Stream stream)
        {
            var bytes = new[] { Height };
            bytes = bytes.Concat(BitConverter.GetBytes(BlockId)).ToArray();
            stream.Write(bytes, 0, bytes.Length);
        }
        public void FromStream(Stream stream)
        {
            var readBytes = new byte[Bytes];
            stream.Read(readBytes, 0, Bytes);
            Height = readBytes[0];
            BlockId = BitConverter.ToInt32(readBytes, 1);
        }
    }
}