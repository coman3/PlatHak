using System;
using System.IO;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using WebSocket4Net;

namespace PlatHak.Common.Network
{
    public interface IPacket
    {
        byte[] ToBytes();
    }

    
    public abstract class Packet : IPacket
    {
        public static T FromBytes<T>(byte[] data) where T : Packet
        {
            using (var stream = new MemoryStream(data))
            {
                return FromStream<T>(stream);
            }
        }

        public static Packet FromBytes(byte[] data)
        {
            return FromBytes<Packet>(data);
        }

        private static T FromStream<T>(Stream stream) where T : Packet
        {
            var formater = JsonSerializer.Create();
            try
            {
                return (T)formater.Deserialize(new JsonTextReader(new StreamReader(stream)));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return default(T);
            }
        }
        public override string ToString()
        {
            return GetType().Name;
        }

        protected virtual bool ToStream(Stream stream)
        {
            var formater = JsonSerializer.Create();
            try
            {
                formater.Serialize(new JsonTextWriter(new StreamWriter(stream)), this);
                stream.Flush();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
            
        }

        public virtual byte[] ToBytes()
        {
            try
            {
                using (var stream = new MemoryStream())
                {
                    return ToStream(stream) ? stream.ToArray() : null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public T Cast<T>() where T : Packet
        {
            return (T) this;
        }
        /// <summary>
        /// Execute the function if the packet is of T type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <returns>
        /// The result of the function if the packet is of T type, otherwise false
        /// </returns>
        public void DoIfIsType<T>(Action<T> action) where T : Packet
        {
            if (this is T)
            {
                action.Invoke(Cast<T>());
            }
        }
        /// <summary>
        /// Execute the function if the packet is of T type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <returns>
        /// The result of the function if the packet is of T type, otherwise false
        /// </returns>
        public bool DoIfIsType<T>(Func<T, bool> action) where T : Packet
        {
            if (this is T)
            {
                return action.Invoke(Cast<T>());
            }
            return false;
        }
    }
}