using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using PlatHack.Game.Drawers;
using PlatHak.Client.Common;
using PlatHak.Client.Common.Config;
using PlatHak.Client.Common.Helpers;
using PlatHak.Common.Network;
using PlayHak.Client.Network;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectInput;
using SharpDX.DirectWrite;
using SharpDX.DXGI;
using SharpDX.Mathematics.Interop;
using SharpDX.WIC;
using AlphaMode = SharpDX.Direct2D1.AlphaMode;
using Bitmap = SharpDX.Direct2D1.Bitmap;
using BitmapInterpolationMode = SharpDX.Direct2D1.BitmapInterpolationMode;
using MainMenu = PlatHack.Game.Drawers.MainMenu;
using PixelFormat = SharpDX.Direct2D1.PixelFormat;

namespace PlatHack.Game
{
    public class MyGame : Game2D
    {
        public WebSocketClient Client { get; set; }
        private TextFormat CommonTextFormat { get; set; }
        private Brush CommonForgroundBrush { get; set; }

        public List<Surface> Surfaces { get; set; }
        public Surface SelectedSurface { get; set; }
        public MyGame(WebSocketClientConfig config)
        {
            Client = new WebSocketClient(config);
            Client.OnError += Client_OnError;
            Client.OnDisconnect += Client_OnDisconnect;
            Client.OpenConnection();
            Surfaces = new List<Surface>();

            LoadSurfaces();
            if (Surfaces == null || Surfaces.Count == 0) throw new InvalidOperationException("No Surfaces Defined.");
            SelectedSurface = Surfaces.First();
        }

        private void LoadSurfaces()
        {
            Surfaces.Add(new MainMenu(this));
            Surfaces.Add(new Drawers.Game(this));
        }

        private void Client_OnDisconnect(WebSocketEventArgs args)
        {
            
        }

        private void Client_OnError(WebSocketErrorEventArgs args)
        {
            MessageBox.Show(args.Exception.ToString());
            Process.GetCurrentProcess().Kill();

        }

        protected override void Initialize(GameConfiguration demoConfiguration)
        {
            base.Initialize(demoConfiguration);
            if (SelectedSurface.IsInitializedSurface)
            {
                var init = SelectedSurface as IInitializedSurface;
                init?.OnInitialize(RenderTarget2D);
            }
            RenderTarget2D.AntialiasMode = AntialiasMode.PerPrimitive;
        }

        protected override void LoadContent()
        {

            CommonTextFormat = new TextFormat(FactoryDWrite, "Arial", FontWeight.Normal, FontStyle.Normal, 15);
            CommonForgroundBrush = new SolidColorBrush(RenderTarget2D, new RawColor4(0, 0, 0, 255));
        }

        protected override void Update(GameTime time)
        {

        }

        protected override void Draw(GameTime time)
        {
            if (SelectedSurface.IsDrawSurface)
            {
                var init = SelectedSurface as IDrawSurface;
                init?.Draw(RenderTarget2D, time);
            }

            if (Client.HandshakeFinished) RenderTarget2D.DrawText("Handshake Success!", CommonTextFormat, new RawRectangleF(10, 10, 300, 300), CommonForgroundBrush);
            if (Client.LoginFinished) RenderTarget2D.DrawText("Login Success!", CommonTextFormat, new RawRectangleF(10, 30, 300, 300), CommonForgroundBrush);

        }
        
    }
}