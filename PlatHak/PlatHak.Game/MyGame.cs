using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using PlatHak.Client.Common;
using PlatHak.Client.Common.Helpers;
using PlayHak.Client.Network;
using SharpDX.Direct2D1;
using SharpDX.DirectInput;
using SharpDX.DirectWrite;
using SharpDX.Mathematics.Interop;

namespace PlatHack.Game
{
    public class MyGame : Game2D
    {
        public WebSocketClient Client { get; set; }
        public RawColor4 SceneColor { get; set; }
        private TextFormat CommonTextFormat { get; set; }
        private Brush CommonForgroundBrush { get; set; }

        public MyGame(WebSocketClientConfig config)
        {
            Client = new WebSocketClient(config);
            Client.OnError += Client_OnError;
            Client.OnDisconnect += Client_OnDisconnect;
            Client.OpenConnection();
            var input = new DirectInput();
            var mouseControl = new Mouse(input);
            mouseControl.Acquire();
            input.RunControlPanel();
        }

        private void Client_OnDisconnect(WebSocketEventArgs args)
        {
            
        }

        private void Client_OnError(WebSocketErrorEventArgs args)
        {
            MessageBox.Show(args.Exception.ToString());
            Process.GetCurrentProcess().Kill();

        }

        protected override void LoadContent()
        {
            CommonTextFormat = new TextFormat(FactoryDWrite, "Arial", FontWeight.Normal, FontStyle.Normal, 15);
            CommonForgroundBrush = new SolidColorBrush(RenderTarget2D, new RawColor4(0, 0, 0, 255));
        }

        protected override void Draw(GameTime time)
        {
            RenderTarget2D.Clear(SceneColor);
            if(Client.HandshakeFinished) RenderTarget2D.DrawText("Handshake Success!", CommonTextFormat, new RawRectangleF(10, 10, 300, 300), CommonForgroundBrush);
            if (Client.LoginFinished) RenderTarget2D.DrawText("Login Success!", CommonTextFormat, new RawRectangleF(10, 30, 300, 300), CommonForgroundBrush);
        }
    }
}