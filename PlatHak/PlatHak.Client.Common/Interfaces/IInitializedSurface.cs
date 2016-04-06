using SharpDX.Direct2D1;

namespace PlatHak.Client.Common.Interfaces
{
    public interface IInitializedSurface
    {
        void OnInitialize(RenderTarget target, Factory factory, SharpDX.DirectWrite.Factory factoryDr);
    }
}