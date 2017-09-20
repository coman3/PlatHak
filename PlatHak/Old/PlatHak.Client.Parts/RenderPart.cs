using PlatHak.Client.Common.Objects;
using PlatHak.Common.Maths;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;

namespace PlatHak.Client.Parts
{
    public abstract class RenderPart : GameObject
    {
        protected RenderPart(Vector2 posistion) : base(posistion)
        {
        }

    }
}