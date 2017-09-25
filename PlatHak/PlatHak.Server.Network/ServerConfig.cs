using Sockets.Plugin.Abstractions;

namespace PlatHak.Server.Network
{
    public struct ServerConfig
    {
        public int Port { get; set; }
        public ICommsInterface Interface { get; set; }
        public ServerConfig(int port)
        {
            Port = port;
            Interface = null;
        }

        public ServerConfig(int port, ICommsInterface @interface)
        {
            Port = port;
            Interface = @interface;
        }
    }
}