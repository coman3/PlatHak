using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using PlatHak.Client.Common;
using PlatHak.Client.Common.Config;
using PlatHak.Client.Common.Helpers;
using PlatHak.Client.Common.Interfaces;
using PlatHak.Common.Maths;
using PlayHak.Client.Network;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using SharpDX.Mathematics.Interop;
using SharpDX.Windows;
using MainMenu = PlatHack.Game.Surfaces.MainMenu;

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
        }

        private void LoadSurfaces()
        {
            //Surfaces.Add(new MainMenu());
            Surfaces.Add(new Surfaces.Game(this));
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
            return form;
        }

        private void Form_MouseUp(object sender, MouseEventArgs e)
        {
            if (SelectedSurface.IsInputSurface)
            {
                var init = SelectedSurface as IInputSurface;
                var inputValue = InputValue.LeftMouse;

                if(e.Button.HasFlag(MouseButtons.Right)) inputValue = InputValue.RightMouse;
                if (e.Button.HasFlag(MouseButtons.Middle)) inputValue = InputValue.ScrollWheel;

                var inputArgs = new InputEventArgs(InputType.Mouse, inputValue, 0);
                init?.OnInput(inputArgs);
            }
        }

        private void Form_MouseDown(object sender, MouseEventArgs e)
        {
            if (SelectedSurface.IsInputSurface)
            {
                var init = SelectedSurface as IInputSurface;
                var inputValue = InputValue.LeftMouse;

                if (e.Button.HasFlag(MouseButtons.Right)) inputValue = InputValue.RightMouse;
                if (e.Button.HasFlag(MouseButtons.Middle)) inputValue = InputValue.ScrollWheel;

                var inputArgs = new InputEventArgs(InputType.Mouse, inputValue, 1);
                init?.OnInput(inputArgs);
            }
        }

        private void Form_MouseMove(object sender, MouseEventArgs e)
        {
            if (SelectedSurface.IsInputSurface)
            {
                var init = SelectedSurface as IInputSurface;
                var inputArgs = new InputEventArgs(InputType.Mouse, InputValue.MouseMove, e.X, e.Y);
                init?.OnInput(inputArgs);
            }
        }

        protected override void Initialize(GameConfiguration demoConfiguration)
        {
            base.Initialize(demoConfiguration);
            LoadSurfaces();
            if (Surfaces == null || Surfaces.Count == 0) throw new InvalidOperationException("No Surfaces Defined.");
            SelectedSurface = Surfaces.First();

            if (SelectedSurface.IsInitializedSurface)
            {
                var init = SelectedSurface as IInitializedSurface;
                init?.OnInitialize(RenderTarget2D, Factory2D);
            }

        }

        protected override void LoadContent()
        {

            CommonTextFormat = new TextFormat(FactoryDWrite, "Arial", FontWeight.Normal, FontStyle.Normal, 15);
            CommonForgroundBrush = new SolidColorBrush(RenderTarget2D, new RawColor4(255, 255, 255, 75));
        }

        protected override void Update(GameTime time)
        {
            if (SelectedSurface.IsUpdatedSurface)
            {
                var init = SelectedSurface as IUpdatedSurface;
                init?.OnUpdate(time);
            }
        }

        protected override void Draw(GameTime time)
        {
            SelectedSurface?.Draw(RenderTarget2D, time);

            if (Client.HandshakeFinished) RenderTarget2D.DrawText("Handshake Success!", CommonTextFormat, new RawRectangleF(10, 10, 300, 300), CommonForgroundBrush);
            if (Client.LoginFinished) RenderTarget2D.DrawText("Login Success!", CommonTextFormat, new RawRectangleF(10, 30, 300, 300), CommonForgroundBrush);

        }
        
    }
}