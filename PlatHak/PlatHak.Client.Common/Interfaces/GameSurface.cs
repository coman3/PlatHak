using System.Linq;
using PlatHak.Client.Network;
using PlatHak.Common.Maths;
using PlatHak.Common.Network;
using SharpDX.Direct2D1;

namespace PlatHak.Client.Common.Interfaces
{
    public abstract class GameSurface : Surface, IUpdatedSurface, IPacketReciverSurface, IInputSurface
    {
        public WebSocketClient Client { get; set; }
        public virtual void OnUpdate(GameTime time)
        {
            lock (Surfaces)
            {
                foreach (var result in Surfaces.OfType<IUpdatedSurface>())
                {
                    result?.OnUpdate(time);
                }
            }
        }

        public virtual void OnPacketRecived(Packet packet)
        {
            foreach (var result in Surfaces.OfType<IPacketReciverSurface>())
            {
                result?.OnPacketRecived(packet);
            }
        }

        public virtual void OnInput(InputEventArgs args)
        {
            foreach (var result in Surfaces.OfType<IInputSurface>())
            {
                result?.OnInput(args);
            }
        }


        protected GameSurface(RectangleF viewPort, WebSocketClient client, RenderTarget renderTarget, Factory factory,
            SharpDX.DirectWrite.Factory directWriteFactory) : base(viewPort, renderTarget, factory, directWriteFactory)
        {
            Client = client;
        }
    }

}