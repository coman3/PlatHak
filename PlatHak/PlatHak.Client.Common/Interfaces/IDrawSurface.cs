using SharpDX.Direct2D1;

namespace PlatHak.Client.Common.Interfaces
{
    public interface IDrawSurface : ISurface
    {
        void Draw(RenderTarget target, GameTime time);
    }
}