using PlatHak.Client.Common;
using PlatHak.Client.Common.Interfaces;
using SharpDX.Direct2D1;
using RectangleF = PlatHak.Common.Maths.RectangleF;

namespace PlatHack.Game.Surfaces.Menus
{
    public class MainMenuBackground : IDrawSurface, IInitializedSurface, IUpdatedSurface
    {
        public Bitmap ParalaxBackground { get; set; }
        public RectangleF ViewPort { get; set; }

        public MainMenuBackground(RectangleF viewPort)
        {
            ViewPort = viewPort;
        }
        public void Draw(RenderTarget target, GameTime time)
        {
            target.DrawBitmap(ParalaxBackground, ViewPort.RawRectangleF, 1, BitmapInterpolationMode.Linear, ViewPort.RawRectangleF);
        }

        public void OnUpdate(GameTime time)
        {

        }


        public void OnInitialize(RenderTarget target, Factory factory, SharpDX.DirectWrite.Factory factoryDr)
        {
            ParalaxBackground = Helpers.GetContent(target, "ParalaxBackground_0.gif");
        }
    }
}