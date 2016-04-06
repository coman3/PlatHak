using System.Collections.Generic;
using System.Linq;
using PlatHak.Common.Maths;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;

namespace PlatHak.Client.Common.Interfaces
{
    public abstract class Surface : IDrawSurface, IInitializedSurface
    {
        public List<IDrawSurface> DrawSurfaces { get; set; }
        public RectangleF ViewPort { get; set; }
        public bool IsUpdatedSurface => (this as IUpdatedSurface) != null;
        public bool IsDragableSurface => (this as IDragableSurface) != null;
        public bool HasDragableSurfaceObjects => (this as IDragableSurfaceObjects) != null;
        public bool IsInputSurface => (this as IInputSurface) != null;
        public bool IsPacketReciverSurface => (this as IPacketReciverSurface) != null;

        protected Surface(RectangleF viewPort)
        {
            ViewPort = viewPort;
            DrawSurfaces = new List<IDrawSurface>();
        }

        public virtual void Draw(RenderTarget target, GameTime time)
        {
            DrawSurfaces.ForEach(x=> x?.Draw(target, time));
        }

        public abstract void OnInitialize(RenderTarget target, Factory factory, SharpDX.DirectWrite.Factory factoryDr);
    }
}