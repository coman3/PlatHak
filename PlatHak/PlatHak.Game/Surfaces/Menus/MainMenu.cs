using System.Linq;
using PlatHak.Client.Common.Helpers;
using PlatHak.Client.Common.Interfaces;
using PlatHak.Client.Parts;
using PlatHak.Common.Maths;
using PlatHak.Common.Network;
using SharpDX.Direct2D1;

namespace PlatHack.Game.Surfaces
{
    public class MainMenu : MenuSurface, IUpdatedSurface
    {
        public override void Draw(RenderTarget target, GameTime time)
        {
            base.Draw(target, time);
        }

        public override void OnInput(InputEventArgs args)
        {
            //_item?.OnInput(args);
        }

        public override void OnPacketRecived(Packet packet)
        {
            
        }

        public override void Construct()
        {
            DrawSurfaces.Add(new MainMenuBackground());
        }

        public override void OnInitialize(RenderTarget target, Factory factory)
        {
            base.OnInitialize(target, factory);

            //_item = new FlowChartItem(new RectangleF(20, 20, 100, 100), target, factory);
            //DrawSurfaces.Add(_item);
        }

        public void OnUpdate(GameTime time)
        {
            foreach (var update in DrawSurfaces.OfType<IUpdatedSurface>())
            {
                update?.OnUpdate(time);
            }
        }
    }
}
