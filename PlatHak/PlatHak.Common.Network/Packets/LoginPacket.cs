using System;

namespace PlatHak.Common.Network
{
    [Serializable]
    public class LoginPacket : Packet
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public LoginPacket(string username, string password) : base(PacketId.Login)
        {
            Username = username;
            PasswordHash = password;
        }
    }
}