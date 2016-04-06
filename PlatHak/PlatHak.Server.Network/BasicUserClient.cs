using System;
using PlatHak.Common.Network;
using PlatHak.Server.Common;
using SuperWebSocket;

namespace PlatHak.Server.Network
{
    public class BasicUserClient : UserClient
    {
        public BasicUserClient(WebSocketSession session) : base(session)
        {

        }

        public override void HandleHandshake(HandshakePacket packet)
        {
            packet.Processed = true;
            packet.ClientId = Guid.Parse(SessionId);
            HandshakeFinished = true;
            Send(packet);
        }

        public override void HandleLogin(LoginPacket packet)
        {
            LoginFinished = true;
            Send(new EventPacket(EventType.LoginSuccess));
        }

        public override bool HandlePacket(Packet packet)
        {
            Console.WriteLine(SessionId + " Sent: " + packet.Id);
            return true;
        }
    }
}