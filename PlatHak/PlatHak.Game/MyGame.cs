using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using PlatHak.Client.Common;
using PlatHak.Client.Common.Interfaces;
using PlatHak.Common.Maths;
using PlatHak.Common.Network;
using PlayHak.Client.Network;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using SharpDX.Mathematics.Interop;
using SharpDX.Windows;
using MainMenu = PlatHack.Game.Surfaces.Menus.MainMenu;

namespace PlatHack.Game
{
    public class MyGame : Game2D
    {
        public WebSocketClient Client { get; set; }
        private TextFormat CommonTextFormat { get; set; }
        private Brush CommonForgroundBrush { get; set; }
        public RawColor4 SceneColor { get; set; }
        public List<Surface> Surfaces { get; set; }
        public Surface[] SelectedSurfaces { get; set; }

        public MyGame(WebSocketClientConfig config)
        {
            Client = new WebSocketClient(config);
            Client.OnError += Client_OnError;
            Client.OnDisconnect += Client_OnDisconnect;
            Client.OpenConnection();
            Client.OnPacketRecived += Client_OnPacketRecived;
            Surfaces = new List<Surface>();
            SelectedSurfaces = new Surface[0];
            SceneColor = new RawColor4(0, 0, 0 ,255);
        }


        private void Client_OnPacketRecived(PacketEventArgs<Packet> args)
        {
            foreach (var selectedSurface in SelectedSurfaces.OfType<IPacketReciverSurface>())
            {
                selectedSurface.OnPacketRecived(args.Packet);
            }
        }

        private void LoadSurfaces()
        {
            Surfaces.Add(new Surfaces.Games.Game(new RectangleF(0, 0, Config.Width, Config.Height), this));
            

            SelectedSurfaces = Surfaces.ToArray();
        }

        private void Client_OnDisconnect(WebSocketEventArgs args)
        {
            
        }

        private void Client_OnError(WebSocketErrorEventArgs args)
        {
            MessageBox.Show(args.Exception.ToString());
            Process.GetCurrentProcess().Kill();

        }

        protected override RenderForm CreateForm(GameConfiguration config)
        {
            var form =  base.CreateForm(config);
            form.MouseMove += Form_MouseMove;
            form.MouseDown += Form_MouseDown;
            form.MouseUp += Form_MouseUp;
            form.MouseWheel += Form_MouseWheel;
            form.KeyDown += Form_KeyDown;
            form.KeyUp += Form_KeyUp;
            return form;
        }

        private void Form_KeyUp(object sender, KeyEventArgs e)
        {
            foreach (var selectedSurface in SelectedSurfaces.OfType<IInputSurface>())
            {
                var inputValue = InputValue.KeyUp;

                var inputArgs = new InputEventArgs(InputType.Keyboard, inputValue, (int)e.KeyCode);
                selectedSurface?.OnInput(inputArgs);
            }
        }

        private void Form_KeyDown(object sender, KeyEventArgs e)
        {
            foreach (var selectedSurface in SelectedSurfaces.OfType<IInputSurface>())
            {
                var inputValue = InputValue.KeyDown;

                var inputArgs = new InputEventArgs(InputType.Keyboard, inputValue, (int)e.KeyCode);
                selectedSurface?.OnInput(inputArgs);
            }
        }

        private void Form_MouseWheel(object sender, MouseEventArgs e)
        {
            foreach (var selectedSurface in SelectedSurfaces.OfType<IInputSurface>())
            {
                var inputValue = InputValue.ScrollWheelValue;

                var inputArgs = new InputEventArgs(InputType.Mouse, inputValue, e.Delta);
                selectedSurface?.OnInput(inputArgs);
            }
        }

        private void Form_MouseUp(object sender, MouseEventArgs e)
        {
            foreach (var selectedSurface in SelectedSurfaces.OfType<IInputSurface>())
            {
                var inputValue = InputValue.LeftMouse;
                if (e.Button.HasFlag(MouseButtons.Right)) inputValue = InputValue.RightMouse;
                if (e.Button.HasFlag(MouseButtons.Middle)) inputValue = InputValue.ScrollWheel;

                var inputArgs = new InputEventArgs(InputType.Mouse, inputValue, 0);
                selectedSurface?.OnInput(inputArgs);
            }
        }

        private void Form_MouseDown(object sender, MouseEventArgs e)
        {
            foreach (var selectedSurface in SelectedSurfaces.OfType<IInputSurface>())
            {
                var inputValue = InputValue.LeftMouse;
                if (e.Button.HasFlag(MouseButtons.Right)) inputValue = InputValue.RightMouse;
                if (e.Button.HasFlag(MouseButtons.Middle)) inputValue = InputValue.ScrollWheel;

                var inputArgs = new InputEventArgs(InputType.Mouse, inputValue, 1);
                selectedSurface?.OnInput(inputArgs);

            }
        }

        private void Form_MouseMove(object sender, MouseEventArgs e)
        {
            foreach (var selectedSurface in SelectedSurfaces.OfType<IInputSurface>())
            {
                var inputArgs = new InputEventArgs(InputType.Mouse, InputValue.MouseMove, e.X, e.Y);
                selectedSurface?.OnInput(inputArgs);
            }
        }

        protected override void Initialize(GameConfiguration demoConfiguration)
        {
            base.Initialize(demoConfiguration);
            LoadSurfaces();
            if (Surfaces == null || Surfaces.Count == 0) throw new InvalidOperationException("No Surfaces Defined.");
            foreach (var selectedSurface in SelectedSurfaces)
            {
                selectedSurface?.OnInitialize(RenderTarget2D, Factory2D, FactoryDWrite);
            }
            
            
        }

        protected override void LoadContent()
        {

            CommonTextFormat = new TextFormat(FactoryDWrite, "Arial", FontWeight.Normal, FontStyle.Normal, 15);
            CommonForgroundBrush = new SolidColorBrush(RenderTarget2D, new RawColor4(255, 255, 255, 75));
        }

        protected override void Update(GameTime time)
        {
            foreach (var selectedSurface in SelectedSurfaces.OfType<IUpdatedSurface>())
            {
                selectedSurface?.OnUpdate(time);
            }
            if (Client.LoginFinished && Client.HandshakeFinished && !WarnedOfLoad)
            {
                Client.Send(new EventPacket(EventType.ClientLoaded));
                WarnedOfLoad = true;
            }
        }

        public bool WarnedOfLoad { get; set; }

        protected override void Draw(GameTime time)
        {
            RenderTarget2D.Clear(SceneColor);
            foreach (var selectedSurface in SelectedSurfaces)
            {
                selectedSurface?.Draw(RenderTarget2D, time);
            }

            if (Client.HandshakeFinished) RenderTarget2D.DrawText("Handshake Success!", CommonTextFormat, new RawRectangleF(10, 10, 300, 300), CommonForgroundBrush);
            if (Client.LoginFinished) RenderTarget2D.DrawText("Login Success!", CommonTextFormat, new RawRectangleF(10, 30, 300, 300), CommonForgroundBrush);
        }
        
    }
}