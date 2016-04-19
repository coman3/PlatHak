namespace PlatHak.Client.Network
{
    public struct WebSocketClientConfig
    {
        public string ServerAddress { get; set; }

        public string Username { get; set; }
        public string PasswordHash { get; set; }
    }
}