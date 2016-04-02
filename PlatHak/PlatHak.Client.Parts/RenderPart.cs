using PlatHak.Client.Common.Helpers;
using PlatHak.Client.Common.Objects;
using PlatHak.Common.Maths;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;

namespace PlatHak.Client.Parts
{
    public abstract class RenderPart : GameObject
    {
        protected Factory Factory { get; }
        protected RenderTarget RenderTarget { get;  }
        protected RenderPart(Vector2 posistion, RenderTarget renderTarget, Factory factory) : base(posistion)
        {
            RenderTarget = renderTarget;
            Factory = factory;

            Construct();
        }

        private void Construct()
        {
            ConstructRenderPart();
        }

        protected abstract void ConstructRenderPart();

    }
}