using System.IO;

namespace PlatHak.Common
{
    public static class StreamHelpers
    {
        public static void Write(this MemoryStream stream, params byte[][] data)
        {
            foreach (var value in data)
            {
                stream.Write(value, 0, value.Length);
            }
        }
    }
}