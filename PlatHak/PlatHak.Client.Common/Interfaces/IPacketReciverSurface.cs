using PlatHak.Common.Network;

namespace PlatHak.Client.Common.Interfaces
{
    public interface IPacketReciverSurface
    {
        void OnPacketRecived(Packet packet);
    }
}