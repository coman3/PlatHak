using System.IO;
using PlatHak.Common.World;

namespace PlatHak.Common.Interfaces
{
    public interface ISerialize
    {
        void ToStream(Stream stream);
        void FromStream(Stream stream);
    }
}