using System.Collections.Generic;
using PlatHak.Client.Common.Helpers;
using PlatHak.Common.Network;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;

namespace PlatHack.Game
{
    public abstract class MenuSurface : Surface, IDrawSurface, IInputSurface, IPacketReciverSurface, IInitializedSurface
    {
        public RawColor4 SceneColor { get; set; }
        public List<IDrawSurface> DrawSurfaces { get; set; }
        protected MenuSurface(MyGame game) : base(game)
        {
            DrawSurfaces = new List<IDrawSurface>();
            SceneColor = new RawColor4(0, 0, 0, 255);
            FinishConstruct();
        }
        private void FinishConstruct()
        {
            Construct();
        }

        public virtual void Draw(RenderTarget target, GameTime time)
        {
            target.Clear(SceneColor);
            DrawSurfaces.ForEach(x=> x.Draw(target, time));
        }
        public abstract void OnInput(InputEventArgs args);
        public abstract void OnPacketRecived(Packet packet);

        public virtual void Construct() { }

        public virtual void OnInitialize(RenderTarget target)
        {
            foreach (var drawSurface in DrawSurfaces)
            {
                var init = drawSurface as IInitializedSurface;
                init?.OnInitialize(target);
            }
        }
    }
}