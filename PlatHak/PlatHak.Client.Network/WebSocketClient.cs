using System;
using System.Collections.Generic;
using System.Reflection;
using PlatHak.Common.Network;
using PlatHak.Common.Network.Sockets.Messaging;
using Sockets.Plugin;

namespace PlatHak.Client.Network
{
    public sealed class WebSocketClient
    {
        private TcpSocketClient _socketClient;
        private readonly JsonProtocolTaskMessenger<Packet> _session;
        private WebSocketClientConfig _config;

        public event WebSocketClientDelgates.OnConnect OnConnect;
        public event WebSocketClientDelgates.OnDisconnect OnDisconnect;
        public event WebSocketClientDelgates.OnError OnError;
        public event WebSocketClientDelgates.OnHandshakeFinished OnHandshakeFinished;
        public event WebSocketClientDelgates.OnLoginFinished OnLoginFinished;
        public event WebSocketClientDelgates.OnPacketRecived OnPacketRecived;

        public bool HandshakeFinished { get; set; }
        public bool LoginFinished { get; set; }
        public bool Welcomed { get; set; }
        public string Username => _config.Username;
        public WebSocketClient(WebSocketClientConfig config)
        {
            _config = config;
            _socketClient = new TcpSocketClient();
            _session = new JsonProtocolTaskMessenger<Packet>(_socketClient)
            {
                AdditionalTypeResolutionAssemblies = new List<Assembly>
                {
                    Assembly.Load(new AssemblyName(typeof(Packet).AssemblyQualifiedName.Split(',')[1] ?? throw new ArgumentNullException(typeof(Packet).AssemblyQualifiedName)))
                }
            };
            _socketClient.ConnectAsync(config.ServerAddress, config.Port);
            _session.StartExecuting();
            _session.MessageReceived += (sender, args) => _socket_PacketReceived(_session, args.Message);
        }

        private void _socket_PacketReceived(JsonProtocolTaskMessenger<Packet> session, Packet packet)
        {
            if (HandshakeFinished && LoginFinished)
            {
                OnPacketRecived?.Invoke(new PacketEventArgs<Packet>(session, packet));
                return;
            }

            if (packet is WelcomePacket)
            {
                Welcomed = true;
                OnConnect?.Invoke(new WebSocketEventArgs(_session));
                Send(new HandshakeRequestPacket(_config.ViewPort));
                return;
            }

            if (!Welcomed) return;

            //Login / Handshake handling
            if (!HandshakeFinished && packet is HandshakeResponcePacket)
            {
                var handShakePacket = packet.Cast<HandshakeResponcePacket>();
                if (handShakePacket.Valid)
                {
                    HandshakeFinished = true;
                    OnHandshakeFinished?.Invoke(new PacketEventArgs<HandshakeResponcePacket>(session, handShakePacket));

                    Send(new LoginPacket(_config.Username));

                }
                else
                {
                    OnError?.Invoke(new WebSocketErrorEventArgs(session, new Exception("Handshake Failed")));
                }
            }
            else if (!LoginFinished && packet is EventPacket)
            {
                var eventPacket = packet.Cast<EventPacket>();
                if (eventPacket.EventType == EventType.LoginSuccess)
                {
                    LoginFinished = true;
                    OnLoginFinished?.Invoke(new PacketEventArgs<EventPacket>(session, eventPacket));
                }
                else
                {
                    OnError?.Invoke(new WebSocketErrorEventArgs(session, new Exception("Login Failed")));
                }
            }

        }

        //private void _socket_Closed(object sender, EventArgs e)
        //{
        //    OnDisconnect?.Invoke(new WebSocketEventArgs((WebSocket) sender));

        //}

        private void Send(Packet packet)
        {
            _session.Send(packet);
        }
    }
}