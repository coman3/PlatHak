using System;
using System.Windows.Forms;
using PlatHak.Client.Common;
using PlatHak.Client.Common.Interfaces;
using PlatHak.Client.Network;
using PlatHak.Common.Maths;
using PlatHak.Common.Network;
using PlatHak.Common.Objects;
using PlatHak.Common.World;
using SharpDX.Direct2D1;
using SharpDX.DirectInput;
using SharpDX.DirectWrite;
using SharpDX.Mathematics.Interop;
using Factory = SharpDX.Direct2D1.Factory;
using RectangleF = PlatHak.Common.Maths.RectangleF;
using Vector2 = PlatHak.Common.Maths.Vector2;

namespace PlatHack.Game.Surfaces.Game
{
    public class Game : GameSurface
    {
        public ClientWorldManager WorldManager;
        
        private TextFormat CommonTextFormat { get; set; }
        private Brush CommonForgroundBrush { get; set; }
        private bool _sentLoaded;
        #region Events
        public override void OnInitialize(RenderTarget target, Factory factory, SharpDX.DirectWrite.Factory factoryDr)
        {
            CommonTextFormat = new TextFormat(factoryDr, "Arial", FontWeight.Normal, FontStyle.Normal, 15);
            CommonForgroundBrush = new SolidColorBrush(target, new RawColor4(255, 255, 255, 75));

            base.OnInitialize(target, factory, factoryDr);
        }
        public override void OnUpdate(GameTime time)
        {
            base.OnUpdate(time);
            if (!_sentLoaded && Client.HandshakeFinished && Client.LoginFinished)
            {
                Client.Send(new EventPacket(EventType.ClientLoaded));
                _sentLoaded = true;
            }
        }

        public override void OnPacketRecived(Packet packet)
        {
            if (WorldManager.Loaded)
            {
                base.OnPacketRecived(packet);
                return;
            }
            if (packet.DoIfIsType<CreateEntityPacket>(createEntityPacket =>
            {
                if (!(createEntityPacket.Entity is Player entity) || 
                    entity.Username != Client.Username)
                        return false;

                WorldManager.Player = entity;
                if (!WorldManager.Loaded && WorldManager.World != null) LoadWorldContainers();
                return true;
            })) return;

            packet.DoIfIsType<WorldConfigPacket>(worldPacket =>
            {
                if(WorldManager.World != null) return;
                WorldManager.World = new World(worldPacket.Config);
                if (!WorldManager.Loaded && WorldManager.Player != null) LoadWorldContainers();
            });


        }

        public override void Draw(RenderTarget target, GameTime time)
        {
            base.Draw(target, time);
            if (WorldManager.Loaded) target.DrawText("World Container Loaded", CommonTextFormat, new RectangleF(10, 50, 200, 20).RawRectangleF, CommonForgroundBrush);
        }

        public override void OnInput(InputEventArgs args)
        {
            if (args.InputType == InputType.Keyboard) ProcessKeyboardInput(args);
            if(args.InputType == InputType.Mouse) ProcessMouseInput(args);
        }
        #endregion
        private void LoadWorldContainers()
        {
            lock (Surfaces)
            {
                WorldManager.Drawer = new WorldDrawer(ref WorldManager, ViewPort);
                WorldManager.Drawer.OnInitialize(RenderTarget, Factory, DirectWriteFactory);
                Surfaces.Add(WorldManager.Drawer);
            }
        }

        private void ProcessKeyboardInput(InputEventArgs args)
        {
            if (args.InputType != InputType.Keyboard) return;
            if (args.ValueType == InputValue.KeyDown)
            {
                switch ((Key)args.ValueX)
                {
                    case Key.W:
                        WorldManager.SendCommand(new MovePlayerCommand());
                        break;
                }    
            }
            else if (args.ValueType == InputValue.KeyUp)
            {
                switch ((Key)args.ValueX)
                {
                    case Key.W:

                        break;
                }
            }
        }
        private void ProcessMouseInput(InputEventArgs args)
        {

        }
        public Game(RectangleF viewPort, WebSocketClient client, RenderTarget renderTarget, Factory factory, SharpDX.DirectWrite.Factory directWriteFactory) : base(viewPort, client, renderTarget, factory, directWriteFactory)
        {
            WorldManager = new ClientWorldManager(client);
            Surfaces.Add(WorldManager);
        }
    }
}