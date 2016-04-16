using System;
using System.Linq;
using PlatHak.Client.Common;
using PlatHak.Client.Common.Interfaces;
using PlatHak.Client.Common.Objects;
using PlatHak.Client.Parts;
using PlatHak.Common;
using PlatHak.Common.Maths;
using PlatHak.Common.Network;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;

namespace PlatHack.Game.Surfaces.Menus
{
    public class MainMenu : MenuSurface, IUpdatedSurface, IDragableSurfaceObjects
    {
        public override void OnInput(InputEventArgs args)
        {
            base.OnInput(args);
            if (args.ValueType == InputValue.LeftMouse && args.ValueX == 1)
            {
                OnStartDrag?.Invoke(this, EventArgs.Empty);
                return;
            }
            if (args.ValueType == InputValue.LeftMouse && args.ValueX == 0)
            {
                if (ViewPort.Contains(MousePosistion))
                {
                    OnCancelDrag?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    OnDrop?.Invoke(this, null);
                }
            }
        }

        public override void OnPacketRecived(Packet packet)
        {
            
        }

        public override void Construct()
        {
            Surfaces.Add(new MainMenuBackground(ViewPort));
            
        }

        public void OnUpdate(GameTime time)
        {
            foreach (var update in Surfaces.OfType<IUpdatedSurface>())
            {
                update?.OnUpdate(time);
            }
        }

        public override void OnInitialize(RenderTarget target, Factory factory, SharpDX.DirectWrite.Factory factoryDr)
        {
            Surfaces.Add(new ButtonRenderPart(new RectangleF(10, 200, this.ViewPort.Width - 20, 35), "Button 1", 2,
                new SolidColorBrush(target, SharpDX.Color.Black), new SolidColorBrush(target, SharpDX.Color.White)));
            base.OnInitialize(target, factory, factoryDr);
        }

        public MainMenu(RectangleF viewPort) : base(viewPort)
        {
        }

        public event DragableSurfaceObjectsDelegates.OnStartDrag OnStartDrag;
        public event DragableSurfaceObjectsDelegates.OnDrop OnDrop;
        public event DragableSurfaceObjectsDelegates.OnCancelDrag OnCancelDrag;
        public void OnDropItem(Vector2 posistion, DragItem item)
        {
            
        }
    }
}
