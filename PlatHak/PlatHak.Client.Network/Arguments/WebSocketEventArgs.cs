﻿using System;
using PlatHak.Common.Network;
using PlatHak.Common.Network.Sockets.Messaging;

namespace PlatHak.Client.Network
{
    public class WebSocketEventArgs : EventArgs
    {
        public JsonProtocolTaskMessenger<Packet> Session { get; }

        public WebSocketEventArgs(JsonProtocolTaskMessenger<Packet> session)
        {
            Session = session;
        }
         
    }
}