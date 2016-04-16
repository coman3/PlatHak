using PlatHak.Common.Network;

namespace PlatHak.Client.Common.Interfaces
{
    public interface IPacketReciverSurface : ISurface
    {
        void OnPacketRecived(Packet packet);
    }
}