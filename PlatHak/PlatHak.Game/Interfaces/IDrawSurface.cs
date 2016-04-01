using PlatHak.Client.Common.Helpers;
using SharpDX.Direct2D1;

namespace PlatHack.Game
{
    public interface IDrawSurface
    {
        void Draw(RenderTarget target, GameTime time);
    }
}