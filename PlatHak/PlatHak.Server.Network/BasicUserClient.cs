using System;
using PlatHak.Common.Network;
using Sockets.Plugin.Abstractions;

namespace PlatHak.Server.Network
{
    public class BasicUserClient : UserClient
    {
        public BasicUserClient(ITcpSocketClient session) : base(session)
        {

        }

        public override void HandleHandshake(HandshakeRequestPacket packet)
        {
           HandshakeFinished = true;
           Send(new HandshakeResponcePacket{ Valid = true, TimeValidated = DateTime.UtcNow });
        }

        public override void HandleLogin(LoginPacket packet)
        {
            Username = packet.Username;
            LoginFinished = true;
            Send(new EventPacket(EventType.LoginSuccess));
        }

        public override bool HandlePacket(Packet packet)
        {
            Console.WriteLine(SessionId + " Sent: " + packet.GetType());
            return false;
        }
    }


}