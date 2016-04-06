using SharpDX.Direct2D1;

namespace PlatHak.Client.Common.Interfaces
{
    public interface IDrawSurface
    {
        void Draw(RenderTarget target, GameTime time);
    }
}