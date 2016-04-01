using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlatHak.Client.Common.Helpers;
using PlatHak.Common.Network;
using SharpDX.Direct2D1;

namespace PlatHack.Game.Drawers
{
    public class MainMenu : MenuSurface
    {
        public Bitmap ButtonSheet { get; set; }
        public override void Draw(RenderTarget target, GameTime time)
        {
            base.Draw(target, time);

            target.DrawBitmap(ButtonSheet, 1, BitmapInterpolationMode.Linear);
        }

        public override void OnInput(InputEventArgs args)
        {
            
        }

        public override void OnPacketRecived(Packet packet)
        {
            
        }

        public override void Construct()
        {
            DrawSurfaces.Add(new MainMenuBackground());
            
        }

        public override void OnInitialize(RenderTarget target)
        {
            ButtonSheet = Helpers.GetContent(target, "ButtonSheet.png");
            base.OnInitialize(target);
        }

        public MainMenu(MyGame game) : base(game)
        {
        }
    }
}
