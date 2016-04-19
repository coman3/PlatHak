using System;
using System.Windows.Forms;
using PlatHak.Client.Common;
using PlatHak.Client.Common.Interfaces;
using PlatHak.Client.Network;
using PlatHak.Common.Maths;
using PlatHak.Common.Network;
using PlatHak.Common.World;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using SharpDX.Mathematics.Interop;
using Factory = SharpDX.Direct2D1.Factory;
using RectangleF = PlatHak.Common.Maths.RectangleF;
using Vector2 = PlatHak.Common.Maths.Vector2;

namespace PlatHack.Game.Surfaces.Game
{
    public class Game : GameSurface, IDragableSurface
    {
        public ClientWorldManager WorldManager;
        
        private TextFormat CommonTextFormat { get; set; }
        private Brush CommonForgroundBrush { get; set; }

        public override void OnPacketRecived(Packet packet)
        {
            if (packet.DoIfIsType<PlayerPacket>(playerPacket =>
            {
                if (playerPacket.Player.Username != Client.Username) return false;

                WorldManager.Player = playerPacket.Player;
                if (!WorldManager.Loaded && WorldManager.World != null) LoadWorldContainers();
                return true;
            })) return;
            if (packet.DoIfIsType<WorldPacket>(worldPacket =>
            {
                WorldManager.World = new World(worldPacket.Config);
                if (!WorldManager.Loaded && WorldManager.Player != null) LoadWorldContainers();
                return true;
            })) return;

            if (WorldManager.Loaded) base.OnPacketRecived(packet);
        }

        private void LoadWorldContainers()
        {
            lock (Surfaces)
            {
                WorldManager.Drawer = new WorldDrawer(ref WorldManager, ViewPort);
                WorldManager.Drawer.OnInitialize(RenderTarget, Factory, DirectWriteFactory);
                Surfaces.Add(WorldManager.Drawer);
            }
        }

        public override void Draw(RenderTarget target, GameTime time)
        {
            base.Draw(target, time);
            if(WorldManager.Loaded) target.DrawText("World Container Loaded", CommonTextFormat, new RectangleF(10, 50, 200, 20).RawRectangleF, CommonForgroundBrush);
        }

        public Vector2 LastMousePos { get; set; }
        public float MouseWheelValue { get; set; }
        public bool Static { get; set; }
        public MoveState MoveState { get; set; }
        public override void OnInput(InputEventArgs args)
        {
            if (args.InputType == InputType.Keyboard)
            {
                var key = (Keys) args.ValueX;
                var state = args.ValueType == InputValue.KeyDown;
                if (key == Keys.W && MoveState.Up != state)
                {
                    Client.Send(new MoveRequest { MoveType = MoveType.Up, State = state });
                    MoveState.Up = state;
                }
                if (key == Keys.A && MoveState.Left != state)
                {
                    Client.Send(new MoveRequest { MoveType = MoveType.Left, State = state });
                    MoveState.Left = state;
                }
                if (key == Keys.S && MoveState.Down != state)
                {
                    Client.Send(new MoveRequest { MoveType = MoveType.Down, State = state });
                    MoveState.Down = state;
                }
                if (key == Keys.D && MoveState.Right != state)
                {
                    Client.Send(new MoveRequest { MoveType = MoveType.Right, State = state });
                    MoveState.Right = state;
                }
            }
        }

        public void OnDropItem(Vector2 posistion, DragItem item)
        {
           
        }

        public override void OnInitialize(RenderTarget target, Factory factory, SharpDX.DirectWrite.Factory factoryDr)
        {
            CommonTextFormat = new TextFormat(factoryDr, "Arial", FontWeight.Normal, FontStyle.Normal, 15);
            CommonForgroundBrush = new SolidColorBrush(target, new RawColor4(255, 255, 255, 75));

            base.OnInitialize(target, factory, factoryDr);
            Client.Send(new EventPacket(EventType.ClientLoaded));
        }

        public Game(RectangleF viewPort, WebSocketClient client, RenderTarget renderTarget, Factory factory, SharpDX.DirectWrite.Factory directWriteFactory) : base(viewPort, client, renderTarget, factory, directWriteFactory)
        {
            WorldManager = new ClientWorldManager(client);
            Surfaces.Add(WorldManager);
            MoveState = new MoveState();
        }
    }

    public class MoveState
    {
        public bool Up { get; set; }
        public bool Left { get; set; }
        public bool Right { get; set; }
        public bool Down { get; set; }

    }
}