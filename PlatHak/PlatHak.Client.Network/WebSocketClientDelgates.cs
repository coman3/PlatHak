using PlatHak.Common.Network;

namespace PlatHak.Client.Network
{
    public static class WebSocketClientDelgates
    {
        public delegate void OnConnect(WebSocketEventArgs args);
        public delegate void OnDisconnect(WebSocketEventArgs args);

        public delegate void OnError(WebSocketErrorEventArgs args);

        public delegate void OnHandshakeFinished(PacketEventArgs<HandshakeResponcePacket> args);
        public delegate void OnPacketRecived(PacketEventArgs<Packet> args);


        public delegate void OnLoginFinished(PacketEventArgs<EventPacket> args);
    }
}