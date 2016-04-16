using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using PlatHak.Common.Network;
using SuperSocket.SocketBase.Config;
using SuperWebSocket;

namespace PlatHak.Server.Network
{
    public class WebSocketServer
    {
        private readonly IServerConfig _config;
        private readonly SuperWebSocket.WebSocketServer _socketServer;
        private readonly Timer _updateTimer;
        public List<UserClient> UserClients { get; set; }
        public event WebSocketServerDelegates.OnLogOutput OnLogOutput;
        public event WebSocketServerDelegates.OnSetup OnSetup;
        public event WebSocketServerDelegates.OnStart OnStart;
        public event WebSocketServerDelegates.OnUpdate OnUpdate;
        public event WebSocketServerDelegates.OnClientConnect OnClientConnect;
        public event WebSocketServerDelegates.OnPacketReceived OnPacketReceived;
        public event WebSocketServerDelegates.OnClientLoginHandshakeSuccess OnClientLoginHandshakeSuccess;
        public event WebSocketServerDelegates.OnClientLoaded OnClientLoaded;

        public WebSocketServer(IServerConfig config)
        {
            _config = config;
            _updateTimer = new Timer(1000/60d); //60 times a second
            _updateTimer.Elapsed += (sender, _) => OnUpdate?.Invoke();
            _socketServer = new SuperWebSocket.WebSocketServer();
        }

        private void Log(string message)
        {
            OnLogOutput?.Invoke(message);
        }

        public void Start()
        {
            if (_socketServer.Setup(_config))
            {
                ServerSetup();
                if (_socketServer.Start())
                {
                    ServerStart();
                }
            }
        }

        public void Broadcast(Packet packet)
        {
            foreach (var userClient in UserClients)
            {
                userClient.Send(packet);
            }
        }

        private UserClient GetUserClient(WebSocketSession session)
        {
            return UserClients.FirstOrDefault(x => x.SessionId == session.SessionID);
        }

        private void ServerSetup()
        {
            UserClients = new List<UserClient>();
            Log("Server Setup!");
            _socketServer.NewDataReceived += _socketServer_NewDataReceived;
            _socketServer.NewSessionConnected += _socketServer_NewSessionConnected;
            OnSetup?.Invoke(new WebSocketServerEventArgs(_socketServer));

        }

        private void ServerStart()
        {
            OnStart?.Invoke(new WebSocketServerEventArgs(_socketServer));
            Log("Server Started! Listening on Port: " + _config.Port);
            _updateTimer.Start();
        }

        private void _socketServer_NewSessionConnected(WebSocketSession session)
        {
            if (GetUserClient(session) != null) return; //Client already exists
            var userClient = OnClientConnect == null ? new BasicUserClient(session) : OnClientConnect.Invoke(session);
            if (userClient != null)
            {
                UserClients.Add(userClient);
                Log("User Connected!");
            }
        }

        private void _socketServer_NewDataReceived(WebSocketSession session, byte[] value)
        {
            var userClient = GetUserClient(session);
            if (userClient == null) return; //Client is not within UserClients

            var packet = Packet.FromBytes(value);
            Console.WriteLine(session.SessionID + ": " + packet);
            //Handle packet if we have already handled handshake and login (and the client is loaded)
            if (userClient.HandshakeFinished && userClient.LoginFinished && userClient.ClientLoaded)
            {
                if (!userClient.HandlePacket(packet))
                {
                    OnPacketReceived?.Invoke(userClient, packet);
                }
                return;
            }
            //Handle handshake
            if (!userClient.HandshakeFinished && packet is HandshakePacket)
            {
                userClient.HandleHandshake(packet.Cast<HandshakePacket>());
                return;
            }
            
            //Handle Login
            if (userClient.HandshakeFinished && !userClient.LoginFinished && packet is LoginPacket)
            {
                userClient.HandleLogin(packet.Cast<LoginPacket>());
                Log("User Logged In!");
                OnClientLoginHandshakeSuccess?.Invoke(userClient);
                return;
            }
            if (userClient.HandshakeFinished && userClient.LoginFinished && packet is EventPacket)
            {
                if (packet.Cast<EventPacket>().EventType == EventType.ClientLoaded)
                {
                    userClient.ClientLoaded = true;
                    OnClientLoaded?.Invoke(userClient);
                }
            }
        }

        private void HandlePacket(WebSocketSession session, Packet packet)
        {

        }
    }
}