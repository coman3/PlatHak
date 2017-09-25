using System;
using PlatHak.Common.Network;
using PlatHak.Server.Sockets.Messaging;

namespace PlatHak.Client.Network
{
    public class WebSocketErrorEventArgs : WebSocketEventArgs
    {
        public Exception Exception { get; }
        public WebSocketErrorEventArgs(JsonProtocolTaskMessenger<Packet> session, Exception ex) : base(session)
        {
            Exception = ex;
        }
    }
}