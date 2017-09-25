namespace PlatHak.Common.Network.Sockets.Discovery
{
    public interface IDiscoveryPayload
    {
        string RemoteAddress { get; set; }
        int RemotePort { get; set; }
    }
}