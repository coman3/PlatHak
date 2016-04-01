using SharpDX.Direct2D1;

namespace PlatHack.Game
{
    public interface IInitializedSurface
    {
        void OnInitialize(RenderTarget target);
    }
}