using System.Collections.Generic;
using System.Linq;
using PlatHak.Common.Maths;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;

namespace PlatHak.Client.Common.Interfaces
{
    public abstract class Surface : IDrawSurface, IInitializedSurface
    {
        public List<ISurface> Surfaces { get; set; }
        public RectangleF ViewPort { get; set; }

        public RenderTarget RenderTarget { get; set; }
        public Factory Factory { get; set; }
        public SharpDX.DirectWrite.Factory DirectWriteFactory { get; set; }
        public bool IsUpdatedSurface => (this as IUpdatedSurface) != null;
        public bool IsDragableSurface => (this as IDragableSurface) != null;
        public bool HasDragableSurfaceObjects => (this as IDragableSurfaceObjects) != null;
        public bool IsInputSurface => (this as IInputSurface) != null;
        public bool IsPacketReciverSurface => (this as IPacketReciverSurface) != null;

        protected Surface(RectangleF viewPort, RenderTarget renderTarget, Factory factory, SharpDX.DirectWrite.Factory directWriteFactory)
        {
            RenderTarget = renderTarget;
            Factory = factory;
            DirectWriteFactory = directWriteFactory;
            ViewPort = viewPort;
            Surfaces = new List<ISurface>();
        }

        public virtual void Draw(RenderTarget target, GameTime time)
        {
            foreach (var result in Surfaces.OfType<IDrawSurface>())
            {
                result?.Draw(target, time);    
            }
        }

        public virtual void OnInitialize(RenderTarget target, Factory factory, SharpDX.DirectWrite.Factory factoryDr)
        {
            if (RenderTarget == null) RenderTarget = target;
            if (Factory == null) Factory = factory;
            if (DirectWriteFactory == null) DirectWriteFactory = factoryDr;
            foreach (var result in Surfaces.OfType<IInitializedSurface>())
            {
                result?.OnInitialize(target, factory, factoryDr);
            }

        }
    }
}