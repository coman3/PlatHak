using System;
using Sockets.Plugin;

namespace PlatHak.Server.Network
{
    public class WebSocketServerEventArgs : EventArgs
    {
        public TcpSocketListener Server { get; private set; }

        public WebSocketServerEventArgs(TcpSocketListener server)
        {
            Server = server;
        }
    }
}