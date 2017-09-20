using System;

namespace PlatHak.Common.Network
{
    /// <summary>
    /// A Request to login from the client to the server.
    /// Currently the password is ignored, but username still needed to give the player a username.
    /// </summary>
    
    public class LoginPacket : Packet
    {
        public string Username { get; set; }
        //public string PasswordHash { get; set; }
        public LoginPacket(string username)//, string password)
        {
            Username = username;
           // PasswordHash = password;
        }
    }
}