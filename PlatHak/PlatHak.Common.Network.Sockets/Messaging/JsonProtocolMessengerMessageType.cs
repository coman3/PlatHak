namespace PlatHak.Common.Network.Sockets.Messaging
{
    public enum JsonProtocolMessengerMessageType : byte
    {
        StandardMessage = 0x0,
        DisconnectMessage = 0x10
    }
}