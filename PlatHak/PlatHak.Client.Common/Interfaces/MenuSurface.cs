using System.Collections.Generic;
using PlatHak.Client.Common.Helpers;
using PlatHak.Common.Network;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;

namespace PlatHak.Client.Common.Interfaces
{
    public abstract class MenuSurface : Surface, IInputSurface, IPacketReciverSurface, IInitializedSurface
    {
        
        protected MenuSurface()
        {
            FinishConstruct();
        }
        private void FinishConstruct()
        {
            Construct();
        }

        public abstract void OnInput(InputEventArgs args);
        public abstract void OnPacketRecived(Packet packet);

        public virtual void Construct() { }

        public virtual void OnInitialize(RenderTarget target, Factory factory)
        {
            foreach (var drawSurface in DrawSurfaces)
            {
                var init = drawSurface as IInitializedSurface;
                init?.OnInitialize(target, factory);
            }
        }
    }
}