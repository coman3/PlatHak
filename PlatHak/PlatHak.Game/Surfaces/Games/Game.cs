using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using PlatHak.Client.Common;
using PlatHak.Client.Common.Interfaces;
using PlatHak.Client.Common.Objects;
using PlatHak.Common.Maths;
using PlatHak.Common.Network;
using PlatHak.Common.World;
using PlayHak.Client.Network;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using SharpDX.DXGI;
using SharpDX.Mathematics.Interop;
using AlphaMode = SharpDX.Direct2D1.AlphaMode;
using Color = PlatHak.Common.Color;
using Factory = SharpDX.Direct2D1.Factory;
using FlowDirection = SharpDX.DirectWrite.FlowDirection;
using GridItem = PlatHak.Common.World.GridItem;
using Math = System.Math;
using RectangleF = PlatHak.Common.Maths.RectangleF;
using Vector2 = PlatHak.Common.Maths.Vector2;

namespace PlatHack.Game.Surfaces.Games
{
    public class Game : GameSurface, IDragableSurface
    {
        public World World;
        public Player Player;
        public WorldLoader WorldLoader;
        public WorldDrawer WorldDrawer;
        
        public override void OnPacketRecived(Packet packet)
        {
            if (packet is PlayerPacket)
            {
                var player = packet.Cast<PlayerPacket>().Player;
                if (player.Username == Game.Client.Username)
                {
                    Player = player;
                    if (World != null && WorldLoader == null)
                    {
                        LoadWorldContainers();
                    }
                    return;
                }

            }
            if (packet is WorldPacket)
            {
                World = new World(packet.Cast<WorldPacket>().Config);
                if (Player == null) return;
                LoadWorldContainers();
                return;
            }
            
            WorldLoader?.OnPacketRecived(packet);

        }

        private void LoadWorldContainers()
        {
            WorldLoader = new WorldLoader(Game.Client, ref World, ref Player);
            WorldDrawer = new WorldDrawer(ref World, ref Player,  ViewPort);
            WorldDrawer.OnInitialize(Game.RenderTarget2D, Game.Factory2D, Game.FactoryDWrite);
            Surfaces.Add(WorldDrawer);
            Surfaces.Add(WorldLoader);
        }

        public Vector2 LastMousePos { get; set; }
        public float MouseWheelValue { get; set; }
        public bool Static { get; set; }
        public override void OnInput(InputEventArgs args)
        {
            if (args.InputType == InputType.Keyboard && args.ValueType == InputValue.KeyDown)
            {
                var key = (Keys) args.ValueX;
                if(key == Keys.D) Game.Client.Send(new MoveRequest { NewPosistion = Player.Posistion + new VectorInt2(32, 0)});
                if (key == Keys.A) Game.Client.Send(new MoveRequest { NewPosistion = Player.Posistion + new VectorInt2(-32, 0) });
                if (key == Keys.W) Game.Client.Send(new MoveRequest { NewPosistion = Player.Posistion + new VectorInt2(0, -32) });
                if (key == Keys.S) Game.Client.Send(new MoveRequest { NewPosistion = Player.Posistion + new VectorInt2(0, 32) });
            }
        }

        public void OnDropItem(Vector2 posistion, DragItem item)
        {
           
        }

        public Game(RectangleF viewPort, MyGame game) : base(viewPort, game)
        {
        }

        public override void OnInitialize(RenderTarget target, Factory factory, SharpDX.DirectWrite.Factory factoryDr)
        {
            
        }
    }

    public class WorldLoader : IUpdatedSurface, IPacketReciverSurface
    {
        public World World;
        public WebSocketClient Client;
        public Player Player;
        public WorldLoader( WebSocketClient client, ref World world, ref Player player)
        {
            World = world;
            Client = client;
            Player = player;
        }

        public void OnUpdate(GameTime time)
        {
            var chunkIn = World.GetChunkFromPosistion(Player.Posistion);
            if (chunkIn == null) return;
            var range = 20;
            for (int x = 0; x < World.WorldConfig.WorldSize.Width; x++)
            {
                for (int y = 0; y < World.WorldConfig.WorldSize.Height; y++)
                {
                    
                    if(World.Grids[x, y] == null) continue;
                    if ((x > chunkIn.Bounds.X + range || x < chunkIn.Bounds.X - range) || (y > chunkIn.Bounds.Y + range || y < chunkIn.Bounds.Y - range))
                    {
                        //Chunk is greater then 10 chunks away, we can clear it now...
                        
                        World.Grids[x, y].Dispose();
                        World.Grids[x, y] = null;
                    }
                }
            }
        }

        public void OnPacketRecived(Packet packet)
        {
            if (packet is ChunkPacket)
            {
                var chunkPacket = packet.Cast<ChunkPacket>();
                World.Grids[chunkPacket.Chunk.Bounds.X, chunkPacket.Chunk.Bounds.Y] = chunkPacket.Chunk;
            }
            if (packet is PlayerPacket)
            {
                World.Players.Add(packet.Cast<PlayerPacket>().Player);
            }
            if (packet is PlayerMovePacket)
            {
                var movePacket = packet.Cast<PlayerMovePacket>();
                if (Player.Username == movePacket.Username)
                {
                    Player.Posistion = movePacket.NewPosistion;
                    var chunkIn = World.GetChunkFromPosistion(Player.Posistion);
                    if (chunkIn == null) return;
                    var chunkDistance = 4;
                    for (int x = Math.Max(0, chunkIn.Bounds.X - (chunkDistance));
                        x < Math.Min(chunkIn.Bounds.X + chunkDistance + 1, World.WorldConfig.WorldSize.Width);
                        x++)
                    {
                        for (int y = Math.Max(0, chunkIn.Bounds.Y - (chunkDistance));
                            y < Math.Min(chunkIn.Bounds.Y + chunkDistance + 1, World.WorldConfig.WorldSize.Height);
                            y++)
                        {
                            if (World.Grids[x, y] == null)
                                Client.Send(new ChunkRequestPacket { ChunkPosistion = new VectorInt2(x, y) });
                        }
                    }
                }

                var item = World.Players.FirstOrDefault(x => x.Username == movePacket.Username);
                if (item != null) item.Posistion = movePacket.NewPosistion;
            }

        }
    }

    public class WorldDrawer : IDrawSurface, IInitializedSurface
    {
        public World World;
        public Player Player;
        public RectangleF ViewPort { get; set; }
        public float ZoomScale { get; set; }
        public Dictionary<GridItem, Bitmap> Bitmaps; 
        public SolidColorBrush SolidColorBrush { get; set; }
        public SolidColorBrush SolidColorBrushGreen { get; set; }
        public SolidColorBrush SolidColorBrushBlue { get; set; }
        public SolidColorBrush SolidColorBrushRed { get; set; }
        public TextFormat DefaultFont { get; set; }
        public WorldDrawer(ref World world, ref Player player, RectangleF viewPort)
        {
            ZoomScale = 1f;
            World = world;
            ViewPort = viewPort;
            Player = player;

        }

        public void Draw(RenderTarget target, GameTime time)
        {
            if (World == null) return;
            var playerPos = Player.Posistion;
            var chunkIn = World.GetChunkFromPosistion(Player.Posistion);
            var blockIn = World.GetBlockFromPosistion(playerPos);
            var playersBlockIn = new List<GridItem>();

            foreach (var player in World.Players)
            {
                var block = World.GetBlockFromPosistion(player.Posistion);
                if(block == null) continue;
                playersBlockIn.Add(block);
            }
            var width = World.WorldConfig.ItemSize.Width;
            var height = World.WorldConfig.ItemSize.Height;
            var chunkPixelWidth = width * World.WorldConfig.ChunkSize.Width;
            var chunkPixelHeight = height * World.WorldConfig.ChunkSize.Height;
            var cameraOffset = new Vector2(0, 0);

            if (chunkIn == null || blockIn == null) return;
            var chunkPerWidth = ViewPort.Width / chunkPixelWidth;
            var chunkPerHeight = ViewPort.Height / chunkPixelHeight;
            cameraOffset =
                new Vector2(
                    -(chunkIn.Bounds.X * chunkPixelWidth + (blockIn.Bounds.X * width)) +
                    chunkPerWidth / 2f * chunkPixelWidth,
                    -(chunkIn.Bounds.Y * chunkPixelHeight + blockIn.Bounds.Y * height) +
                    chunkPerHeight / 2f * chunkPixelHeight);

            for (int x = Math.Max(0, chunkIn.Bounds.X - 10);
                x < Math.Min(World.WorldConfig.WorldSize.Width, chunkIn.Bounds.X + 10);
                x++)
            {
                for (int y = Math.Max(0, chunkIn.Bounds.Y - 10); y < Math.Min(World.WorldConfig.WorldSize.Height, chunkIn.Bounds.Y + 10); y++)
                {
                    //foreach chunk
                    var chunk = World.Grids[x, y];
                    var offset = new Vector2(cameraOffset.X + ViewPort.X + x * chunkPixelWidth,
                        cameraOffset.Y + ViewPort.Y + y * chunkPixelHeight);
                    var rectChunk = new RectangleF(
                        offset.X,
                        offset.Y,
                        chunkPixelWidth,
                        chunkPixelHeight);
                    if (!ViewPort.Contains(rectChunk.TopLeft) && !ViewPort.Contains(rectChunk.BottomRight) &&
                        !ViewPort.Contains(rectChunk.TopRight) && !ViewPort.Contains(rectChunk.BottomLeft)) continue;
                    if (chunk == null) continue;

                    for (int cx = 0; cx < World.WorldConfig.ChunkSize.Width; cx++)
                    {
                        for (int cy = 0; cy < World.WorldConfig.ChunkSize.Height; cy++)
                        {
                            //foreach block in chunk
                            var block = chunk.Items[cx, cy];
                            var rectBlock = new RectangleF(
                                offset.X + cx * width,
                                offset.Y + cy * height,
                                width,
                                height);
                            if (!ViewPort.Contains(rectBlock.TopLeft) && !ViewPort.Contains(rectBlock.BottomRight) &&
                                !ViewPort.Contains(rectBlock.TopRight) && !ViewPort.Contains(rectBlock.BottomLeft))
                                continue;
                            var brush = blockIn == block ? SolidColorBrushGreen : (playersBlockIn.Contains(block) ? SolidColorBrushBlue : SolidColorBrush);

                            target.DrawRectangle(rectBlock.RawRectangleF, brush);

                        }
                    }

                    target.DrawText($"{x}:{y}", DefaultFont, rectChunk.RawRectangleF, SolidColorBrush);
                    target.DrawRectangle(rectChunk.RawRectangleF, SolidColorBrushRed);
                    target.DrawText(playerPos.ToString(), DefaultFont,
                        new SharpDX.RectangleF(ViewPort.BottomRight.X - 50, ViewPort.BottomRight.Y - 10, 50, 10),
                        SolidColorBrush);

                }
            }
            
        }

        public void OnInitialize(RenderTarget target, Factory factory, SharpDX.DirectWrite.Factory factoryDr)
        {
            SolidColorBrush = new SolidColorBrush(target, SharpDX.Color.White);
            SolidColorBrushGreen = new SolidColorBrush(target, SharpDX.Color.Green);
            SolidColorBrushBlue = new SolidColorBrush(target, SharpDX.Color.Blue);
            SolidColorBrushRed = new SolidColorBrush(target, SharpDX.Color.Red);

            DefaultFont = new TextFormat(factoryDr, "Courier New", 8)
            {
                TextAlignment = TextAlignment.Center, FlowDirection = FlowDirection.BottomToTop
            };
        }
    }
}