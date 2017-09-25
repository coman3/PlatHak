using PlatHak.Common.Network;
using WebSocket4Net;

namespace PlatHak.Client.Network
{
    public class PacketEventArgs<T> : WebSocketEventArgs where T : Packet 
    {
        public T Packet { get; }
        public PacketEventArgs(WebSocket socket, T packet) : base(socket)
        {
            Packet = packet;
        }
    }
}