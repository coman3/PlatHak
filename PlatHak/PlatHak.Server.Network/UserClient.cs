using System;
using PlatHak.Common.Network;
using PlatHak.Common.World;
using SuperWebSocket;

namespace PlatHak.Server.Network
{
    public abstract class UserClient
    {
        public bool LoginFinished { get; set; }
        public bool HandshakeFinished { get; set; }
        public bool ClientLoaded { get; set; }

        public Guid Id => new Guid(SessionId);
        public string SessionId => Session.SessionID;
        public string Username { get; set; }
        public Player Player { get; set; }

        private WebSocketSession Session { get; set; }

        protected UserClient(WebSocketSession session)
        {
            Session = session;
        }

        public void Send(Packet packet)
        {
            var packetData = packet.ToBytes();
            Session.Send(packetData, 0, packetData.Length);
        }

        public void Send(byte[] packet)
        {
            Session.Send(packet, 0, packet.Length);
        }

        /// <summary>
        /// What does the server do when the user sends a handshake?
        /// </summary>
        /// <param name="packet">the handshake packet received</param>
        public abstract void HandleHandshake(HandshakePacket packet);
        /// <summary>
        /// What does the server do when the user wishes to login?
        /// </summary>
        /// <param name="packet">the login packet received</param>
        public abstract void HandleLogin(LoginPacket packet);
        /// <summary>
        /// What does the server do with this packet when it received?
        /// </summary>
        /// <param name="packet">the packet received from this <see cref="Session"/></param>
        /// <returns>true if the packet was handled, false if you wish the server to call the event <see cref="WebSocketServer.OnPacketReceived"/></returns>
        public abstract bool HandlePacket(Packet packet);
    }
}