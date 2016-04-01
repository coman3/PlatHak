using PlatHak.Common.Network;

namespace PlatHack.Game
{
    public interface IPacketReciverSurface
    {
        void OnPacketRecived(Packet packet);
    }
}