using System;
using WebSocket4Net;

namespace PlatHak.Client.Network
{
    public class WebSocketErrorEventArgs : WebSocketEventArgs
    {
        public Exception Exception { get; }
        public WebSocketErrorEventArgs(WebSocket socket, Exception ex) : base(socket)
        {
            Exception = ex;
        }
    }
}