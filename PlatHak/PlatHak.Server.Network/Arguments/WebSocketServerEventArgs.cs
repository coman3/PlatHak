using System;

namespace PlatHak.Server.Network
{
    public class WebSocketServerEventArgs : EventArgs
    {
        public SuperWebSocket.WebSocketServer Server { get; private set; }

        public WebSocketServerEventArgs(SuperWebSocket.WebSocketServer server)
        {
            Server = server;
        }
    }
}