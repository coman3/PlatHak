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

        //public bool SendToServer(WebSocket client)
        //{
        //    try
        //    {
        //        var bytes = ToBytes();
        //        client.Send(bytes, 0, bytes.Length);
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        // Ignored
        //        return false;
        //    }
        //}
        //public bool SendToClient(WebSocketSession client)
        //{
        //    try
        //    {
        //        var bytes = ToBytes();
        //        client.Send(bytes, 0, bytes.Length);
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        // Ignored
        //        return false;
        //    }
        //}
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
            catch (Exception)
            {
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
            catch (Exception)
            {
                return new byte[0];
            }
        }

        public T Cast<T>() where T : Packet
        {
            return (T) this;
        }
    }
}