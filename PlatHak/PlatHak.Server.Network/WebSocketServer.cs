using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using PlatHak.Common.Network;
using PlatHak.Server.Sockets.Messaging;
using Sockets.Plugin;
using Sockets.Plugin.Abstractions;

namespace PlatHak.Server.Network
{
    public class WebSocketServer
    {
        private readonly ServerConfig _config;
        private readonly TcpSocketListener _socketListener;
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

        public WebSocketServer(ServerConfig config)
        {
            _config = config;
            _updateTimer = new Timer(1000/60d); //60 times a second
            _updateTimer.Elapsed += (sender, args) => OnUpdate?.Invoke();
            _socketListener = new TcpSocketListener();
        }

        private void Log(string message)
        {
            OnLogOutput?.Invoke(message);
        }

        public void Start()
        {
            ServerSetup();
            _socketListener.StartListeningAsync(_config.Port, _config.Interface);
            ServerStart();
        }

        public void Broadcast(Packet packet)
        {
            foreach (var userClient in UserClients)
            {
                userClient.Send(packet);
            }
        }
        
        private void ServerSetup()
        {
            UserClients = new List<UserClient>();
            Log("Server Setup!");
            _socketListener.ConnectionReceived += SocketListenerOnConnectionReceived;
            OnSetup?.Invoke(new WebSocketServerEventArgs(_socketListener));

        }

        private void SocketListenerOnConnectionReceived(object o, TcpSocketListenerConnectEventArgs tcpSocketListenerConnectEventArgs)
        {
            var client = tcpSocketListenerConnectEventArgs.SocketClient;

            var userClient = OnClientConnect == null ? new BasicUserClient(client) : OnClientConnect.Invoke(client);
            if (userClient != null)
            {
                UserClients.Add(userClient);
                userClient.Session.MessageReceived += (sender, args) => _socketServer_NewDataReceived(userClient, args.Message);
                Task.Run(async () =>
                {
                    await Task.Delay(150);
                    userClient.Session.Send(new WelcomePacket("Hello!", Environment.Version));
                });
                
                Log($"User Connected! User: {client.RemoteAddress}:{client.RemotePort} ({userClient.SessionId})");
            }
            else
            {
                Log($"Error connecting client: {client.RemoteAddress}:{client.RemotePort}");
            }
        }

        private void ServerStart()
        {
            OnStart?.Invoke(new WebSocketServerEventArgs(_socketListener));
            Log("Server Started! Listening on Port: " + _config.Port);
            _updateTimer.Start();
        }

        private void _socketServer_NewDataReceived(UserClient userClient, Packet packet)
        {
            Log($"Message Recieved: {userClient.SessionId}. Type: {packet.GetType()}");
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
    }
}