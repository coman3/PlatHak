using System;

namespace PlatHak.Client.Network
{
    public class WebSocketEventArgs : EventArgs
    {
        public WebSocket4Net.WebSocket Socket { get; }

        public WebSocketEventArgs(WebSocket4Net.WebSocket socket)
        {
            Socket = socket;
        }
         
    }
}