using System;
using System.Collections.Generic;
using System.Reflection;
using PlatHak.Common.Maths;
using PlatHak.Common.Network;
using PlatHak.Common.Network.Sockets.Messaging;
using PlatHak.Common.Objects;
using PlatHak.Common.World;
using Sockets.Plugin.Abstractions;

namespace PlatHak.Server.Network
{
    public abstract class UserClient
    {
        public bool LoginFinished { get; set; }
        public bool HandshakeFinished { get; set; }
        public bool ClientLoaded { get; set; }

        public Guid SessionId = Guid.NewGuid();
        public string Username { get; set; }
        public Player Player { get; set; }
        public ClientConfig ClientConfig { get; set; }
        public JsonProtocolTaskMessenger<Packet> Session { get; set; }
        public List<VectorLong2> SentChunks { get; set; }
        protected UserClient(ITcpSocketClient session)
        {
            Session = new JsonProtocolTaskMessenger<Packet>(session)
            {
                AdditionalTypeResolutionAssemblies = new List<Assembly>
                {
                    Assembly.Load(new AssemblyName(typeof(Packet).AssemblyQualifiedName?.Split(',')[1] ?? throw new ArgumentNullException(typeof(Packet).AssemblyQualifiedName)))
                }
            };
            Session.StartExecuting();
            SentChunks = new List<VectorLong2>();
        }

        public void Send(Packet packet)
        {
            Session.Send(packet);
        }

        public void Close()
        {
            Session.StopExecuting();
            Session.Disconnect(DisconnectionType.Graceful).Wait();
        }

        /// <summary>
        /// What does the server do when the user sends a handshake?
        /// </summary>
        /// <param name="packet">the handshake packet received</param>
        public abstract void HandleHandshake(HandshakeRequestPacket packet);

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

    public class ClientConfig
    {
        public Rectangle ViewPort { get; set; }

        public ClientConfig(Rectangle viewPort)
        {
            ViewPort = viewPort;
        }
    }
}