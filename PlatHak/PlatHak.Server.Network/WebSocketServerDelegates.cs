using PlatHak.Common.Network;
using Sockets.Plugin.Abstractions;

namespace PlatHak.Server.Network
{
    public static class WebSocketServerDelegates
    {
        public delegate void OnSetup(WebSocketServerEventArgs args);
        public delegate void OnStart(WebSocketServerEventArgs args);

        public delegate void OnUpdate();
        public delegate void OnLogOutput(string message);
        public delegate UserClient OnClientConnect(ITcpSocketClient session);
        public delegate void OnClientLoginHandshakeSuccess(UserClient session);
        public delegate void OnClientLoaded(UserClient session);

        public delegate void OnPacketReceived(UserClient client, Packet packet);

    }
}