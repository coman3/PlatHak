using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using SuperWebSocket;
using WebSocket4Net;

namespace PlatHak.Common.Network
{
    public interface IPacket
    {
        byte[] ToBytes();
    }

    [Serializable]
    public abstract class Packet : IPacket
    {
        public PacketId Id { get; set; }
        protected Packet(PacketId id)
        {
            Id = id;
        }
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
            var formater = new BinaryFormatter();
            try
            {
                return (T)formater.Deserialize(stream);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return default(T);
            }
        }
        public override string ToString()
        {
            return "Packet: " + Id;
        }

        protected virtual bool ToStream(Stream stream)
        {
            var formater = new BinaryFormatter();
            try
            {
                formater.Serialize(stream, this);
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