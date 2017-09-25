using PlatHak.Common.Network;
using PlatHak.Common.Network.Sockets.Messaging;

namespace PlatHak.Client.Network
{
    public class PacketEventArgs<T> : WebSocketEventArgs where T : Packet 
    {
        public T Packet { get; }
        public PacketEventArgs(JsonProtocolTaskMessenger<Packet> session, T packet) : base(session)
        {
            Packet = packet;
        }
    }
}