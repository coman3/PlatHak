using System;

namespace PlatHak.Server.Network
{
    public class WebSocketServerErrorArgs : WebSocketServerEventArgs
    {
        public Exception Exception { get; }
        public WebSocketServerErrorArgs(SuperWebSocket.WebSocketServer server, Exception exception) : base(server)
        {
            Exception = exception;
        }
    }
}