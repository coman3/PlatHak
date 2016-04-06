using System;
using PlatHak.Common.Network;
using WebSocket4Net;

namespace PlayHak.Client.Network
{
    public sealed class WebSocketClient
    {
        private readonly WebSocket _socket;
        private WebSocketClientConfig _config;

        public event WebSocketClientDelgates.OnConnect OnConnect;
        public event WebSocketClientDelgates.OnDisconnect OnDisconnect;
        public event WebSocketClientDelgates.OnError OnError;
        public event WebSocketClientDelgates.OnHandshakeFinished OnHandshakeFinished;
        public event WebSocketClientDelgates.OnLoginFinished OnLoginFinished;
        public event WebSocketClientDelgates.OnPacketRecived OnPacketRecived;

        public bool HandshakeFinished { get; set; }
        public bool LoginFinished { get; set; }
        public WebSocketClient(WebSocketClientConfig config)
        {
            _config = config;
            _socket = new WebSocket(_config.ServerAddress, "basic");

            _socket.Opened += _socket_Opened;
            _socket.Closed += _socket_Closed;

            _socket.DataReceived += _socket_DataReceived;

            _socket.Error += _socket_Error;
        }

        public void OpenConnection()
        {
            if(_socket != null && (_socket.State == WebSocketState.Closed || _socket.State == WebSocketState.None))
                _socket.Open();
        }

        private void _socket_Error(object sender, SuperSocket.ClientEngine.ErrorEventArgs e)
        {
            OnError?.Invoke(new WebSocketErrorEventArgs((WebSocket)sender, e.Exception));
        }

        private void _socket_DataReceived(object sender, DataReceivedEventArgs e)
        {
            var data = e.Data;
            if(e.Data == null || e.Data.Length == 0) return;

            var packet = Packet.FromBytes(data);
            var webSocket = (WebSocket)sender;

            if (HandshakeFinished && LoginFinished)
            {
                OnPacketRecived?.Invoke(new PacketEventArgs<Packet>(webSocket, packet));
                return;
            }

            //Login / Handshake handling
            if (!HandshakeFinished && packet is HandshakePacket)
            {
                if (packet.Cast<HandshakePacket>().Processed)
                {
                    HandshakeFinished = true;
                    OnHandshakeFinished?.Invoke(new PacketEventArgs<HandshakePacket>(webSocket,
                        packet.Cast<HandshakePacket>()));

                    SendToServer(webSocket, new LoginPacket(_config.Username, _config.PasswordHash));

                }
                else
                {
                    OnError?.Invoke(new WebSocketErrorEventArgs(webSocket, new Exception("Handshake Failed")));
                }
            }
            else if (!LoginFinished && packet is EventPacket)
            {
                if (packet.Cast<EventPacket>().EventType == EventType.LoginSuccess)
                {
                    LoginFinished = true;
                    OnLoginFinished?.Invoke(new PacketEventArgs<EventPacket>(webSocket,
                        packet.Cast<EventPacket>()));
                }
                else
                {
                    OnError?.Invoke(new WebSocketErrorEventArgs(webSocket, new Exception("Login Failed")));
                }
            }

        }

        private void _socket_Closed(object sender, EventArgs e)
        {
            OnDisconnect?.Invoke(new WebSocketEventArgs((WebSocket) sender));

        }

        private void _socket_Opened(object sender, EventArgs e)
        {
            var websocket = (WebSocket) sender;
            OnConnect?.Invoke(new WebSocketEventArgs(websocket));

            if (!HandshakeFinished)
            {
                //Time to handshake
                SendToServer(websocket, new HandshakePacket());

            }
        }

        private void SendToServer(WebSocket socket, Packet packet)
        {
            if(socket.State != WebSocketState.Open) throw new InvalidOperationException("Socket is not open");
            var packetData = packet.ToBytes();
            socket.Send(packetData, 0, packetData.Length);
        }

        public void Send(Packet packet)
        {
            SendToServer(_socket, packet);
        }
    }
}