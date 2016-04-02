using System;
using PlatHak.Client.Common.Helpers;
using PlatHak.Client.Common.Interfaces;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;
using RectangleF = PlatHak.Common.Maths.RectangleF;

namespace PlatHack.Game.Surfaces
{
    public class MainMenuBackground : IDrawSurface, IInitializedSurface, IUpdatedSurface
    {
        public Bitmap ParalaxBackground { get; set; }
        public RectangleF Screen { get; set; }
        public void Draw(RenderTarget target, GameTime time)
        {
            target.DrawBitmap(ParalaxBackground, Screen.RawRectangleF, 1, BitmapInterpolationMode.Linear, Screen.RawRectangleF);
        }

        public void OnUpdate(GameTime time)
        {

        }


        public void OnInitialize(RenderTarget target, Factory factory)
        {
            ParalaxBackground = Helpers.GetContent(target, "ParalaxBackground_0.gif");
            Screen = new RectangleF(0, 0, target.Size.Width, target.Size.Height);
        }
    }
}