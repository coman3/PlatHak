using System.Collections.Generic;
using PlatHak.Common.Maths;
using PlatHak.Common.Network;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;

namespace PlatHak.Client.Common.Interfaces
{
    public abstract class MenuSurface : Surface, IInputSurface, IPacketReciverSurface
    {
        public Vector2 MousePosistion { get; set; }
        protected MenuSurface(RectangleF viewPort) : base(viewPort)
        {
            FinishConstruct();
        }
        private void FinishConstruct()
        {
            Construct();
        }

        public virtual void OnInput(InputEventArgs args)
        {
            if (args.ValueType == InputValue.MouseMove && args.ValueY.HasValue)
            {
                MousePosistion = new Vector2(args.ValueX, args.ValueY.Value);
            }
        }
        public abstract void OnPacketRecived(Packet packet);

        public virtual void Construct() { }

        public override void OnInitialize(RenderTarget target, Factory factory, SharpDX.DirectWrite.Factory factoryDr)
        {
            foreach (var drawSurface in DrawSurfaces)
            {
                var init = drawSurface as IInitializedSurface;
                init?.OnInitialize(target, factory, factoryDr);
            }
        }
    }
}