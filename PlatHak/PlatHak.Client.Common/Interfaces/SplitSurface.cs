using PlatHak.Common.Maths;
using PlatHak.Common.Network;
using SharpDX.Direct2D1;
using SharpDX.DXGI;
using SharpDX.Mathematics.Interop;
using Factory = SharpDX.Direct2D1.Factory;

namespace PlatHak.Client.Common.Interfaces
{
    public class SplitSurface : Surface,  IInputSurface, IUpdatedSurface, IPacketReciverSurface
    {
        public Surface Surface1 { get; set; }
        public Surface Surface2 { get; set; }
        private Vector2 MousePosistion { get; set; }
        private bool DrawingDragItem { get; set; }
        public bool Surface1Focused { get; set; }

        public SplitSurface(Surface surface1, Surface surface2, RectangleF viewPort, RenderTarget renderTarget, Factory factory, SharpDX.DirectWrite.Factory directWriteFactory) : base(viewPort, renderTarget, factory, directWriteFactory)
        {
            Surface1 = surface1;
            Surface2 = surface2;
            HookSurfaceDragEvents(surface1, surface2);
        }

        public void SetHalf(bool vertical = true)
        {
            if (vertical)
            {
                var halfSize = new SizeF(ViewPort.Width / 2f, ViewPort.Height);
                Surface1.ViewPort = new RectangleF(ViewPort.Posistion, halfSize);
                Surface2.ViewPort = new RectangleF(ViewPort.TopCenter, halfSize);
            }
            else
            {
                var halfSize = new SizeF(ViewPort.Width, ViewPort.Height / 2f);
                Surface1.ViewPort = new RectangleF(ViewPort.Posistion, halfSize);
                Surface2.ViewPort = new RectangleF(ViewPort.LeftCenter, halfSize);
            }
        }

        #region Drag Event Management
        private void HookSurfaceDragEvents(Surface surface1, Surface surface2)
        {
            if (surface1.IsDragableSurface && surface2.IsDragableSurface)
            {
                if (surface1.HasDragableSurfaceObjects)
                {
                    var hookSurface1 = (IDragableSurfaceObjects)surface1;

                    hookSurface1.OnDrop += HookSurface1_OnDrop;
                    hookSurface1.OnStartDrag += HookSurface_OnStartDrag;
                    hookSurface1.OnCancelDrag += HookSurface_OnCancelDrag;
                }
                if (surface2.HasDragableSurfaceObjects)
                {
                    var hookSurface2 = (IDragableSurfaceObjects)surface2;
                    hookSurface2.OnDrop += HookSurface2_OnDrop;
                    hookSurface2.OnStartDrag += HookSurface_OnStartDrag;
                    hookSurface2.OnCancelDrag += HookSurface_OnCancelDrag;
                }

            }
        }

        private void HookSurface_OnCancelDrag(object sender, System.EventArgs args)
        {
            DrawingDragItem = false;
        }

        private void HookSurface_OnStartDrag(object sender, System.EventArgs args)
        {
            DrawingDragItem = true;
        }

        private void HookSurface2_OnDrop(object sender, DragItem item)
        {
            var surface1 = Surface1 as IDragableSurfaceObjects;
            surface1?.OnDropItem(MousePosistion, item);
            DrawingDragItem = false;
        }
        
        private void HookSurface1_OnDrop(object sender, DragItem item)
        {
            var surface2 = Surface2 as IDragableSurfaceObjects;
            surface2?.OnDropItem(MousePosistion, item);
            DrawingDragItem = false;
        }
        #endregion

        public override void Draw(RenderTarget target, GameTime time)
        {
            Surface2?.Draw(target, time);
            Surface1?.Draw(target, time);
            if (DrawingDragItem)
            {
                target.DrawRectangle(
                    new RectangleF(MousePosistion.X - 10, MousePosistion.Y - 10, 20, 20).RawRectangleF,
                    new SolidColorBrush(target, new RawColor4(255, 255, 255, 150)));
            }

        }
        public void OnInput(InputEventArgs args)
        {
            if (args.ValueType == InputValue.MouseMove && args.ValueY.HasValue)
            {
                MousePosistion = new Vector2(args.ValueX, args.ValueY.Value);
            }
            Surface1Focused = Surface1.ViewPort.Contains(MousePosistion);

            if (Surface1.IsInputSurface && (Surface1Focused || DrawingDragItem))
            {
                ((IInputSurface) Surface1)?.OnInput(args);
            }
            else if (Surface2.IsInputSurface && (!Surface1Focused || DrawingDragItem))
            {
                ((IInputSurface)Surface2)?.OnInput(args);
            }
        }

        public override void OnInitialize(RenderTarget target, Factory factory, SharpDX.DirectWrite.Factory factoryDr)
        {
            Surface1?.OnInitialize(target, factory, factoryDr);
            Surface2?.OnInitialize(target, factory, factoryDr);

        }

        public void OnUpdate(GameTime time)
        {
            if (Surface1.IsUpdatedSurface)
            {
                ((IUpdatedSurface)Surface1)?.OnUpdate(time);
            }
            if (Surface2.IsUpdatedSurface)
            {
                ((IUpdatedSurface)Surface2)?.OnUpdate(time);
            }
        }

        public void OnPacketRecived(Packet packet)
        {
            if (Surface1.IsPacketReciverSurface)
            {
                ((IPacketReciverSurface)Surface1)?.OnPacketRecived(packet);
            }
            if (Surface2.IsPacketReciverSurface)
            {
                ((IPacketReciverSurface)Surface2)?.OnPacketRecived(packet);
            }
        }


    }
}