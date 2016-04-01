using System;
using PlatHak.Client.Common.Helpers;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;

namespace PlatHack.Game.Drawers
{
    public class MainMenuBackground : IDrawSurface, IInitializedSurface, IUpdatedSurface
    {
        public Bitmap ParalaxBackground { get; set; }
        public RawRectangleF Screen { get; set; }
        public void Draw(RenderTarget target, GameTime time)
        {
            target.DrawBitmap(ParalaxBackground, Screen, 1, BitmapInterpolationMode.Linear, Screen);
        }

        public void OnInitialize(RenderTarget target)
        {
            ParalaxBackground = Helpers.GetContent(target, "ParalaxBackground_0.gif");
            Screen = new RawRectangleF(0, 0, target.Size.Width, target.Size.Height);
        }

        public void OnUpdate(GameTime time)
        {
            
        }
    }
}