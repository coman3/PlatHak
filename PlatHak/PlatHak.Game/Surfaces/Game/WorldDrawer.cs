using System;
using System.Collections.Generic;
using System.Linq;
using PlatHak.Client.Common;
using PlatHak.Client.Common.Interfaces;
using PlatHak.Common.Maths;
using PlatHak.Common.Network;
using PlatHak.Common.Objects;
using PlatHak.Common.World;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using Factory = SharpDX.Direct2D1.Factory;

namespace PlatHack.Game.Surfaces.Game
{
    public class WorldDrawer : IDrawSurface, IInitializedSurface, IPacketReciverSurface
    {
        private ClientWorldManager WorldManager { get; set; }
        public World World => WorldManager.World;
        public WorldConfig WorldConfig => World.WorldConfig;
        public Player Player => WorldManager.Player;
        public RectangleF ViewPort { get; set; }
        public float ZoomScale { get; set; }
        public Size DrawRange { get; set; }
        public ContentManager ContentManager { get; set; }

        public Dictionary<Block, Bitmap> Bitmaps; 
        public SolidColorBrush SolidColorBrushWhite { get; set; }
        public SolidColorBrush SolidColorBrushGreen { get; set; }
        public SolidColorBrush SolidColorBrushBlue { get; set; }
        public SolidColorBrush SolidColorBrushRed { get; set; }
        public TextFormat DefaultFont { get; set; }
        public Bitmap Grass { get; set; }
        public Bitmap Water { get; set; }

        public Size BlockPixelSize => WorldConfig.ItemSize;
        public Size ChunkPixelSize => WorldConfig.ItemSize * WorldConfig.ChunkSize;
        public Vector2 CameraOffset { get; set; }
        public SizeF ChunkPer => ViewPort.Size / ChunkPixelSize.ToSizeF();
        public WorldDrawer(ref ClientWorldManager worldManager, RectangleF viewPort)
        {
            WorldManager = worldManager;
            ZoomScale = 1f;
            ViewPort = viewPort;
            DrawRange = new Size(10, 6);
        }

        #region Draw
        public void Draw(RenderTarget target, GameTime time)
        {
            if (World == null || Player == null) return;
            var playerPos = Player.Position;
            var chunkIn = World.GetChunkCordsFromPosition(Player.Position);
            var blockInPos = World.GetBlockCordsFromPosistion(playerPos);
            var blockIn = World.GetBlockFromPosistion(playerPos);
            //CameraOffset = new Vector2(
            //    -(chunkIn.X * chunkPixelWidth + (blockInPos.X * width)) +
            //    chunkPerWidth / 2f * chunkPixelWidth,
            //    -(chunkIn.Y * chunkPixelHeight + blockInPos.Y * height) +
            //    chunkPerHeight / 2f * chunkPixelHeight);

            CameraOffset = (-chunkIn * ChunkPixelSize.ToVectorInt2() + blockInPos * BlockPixelSize).ToVector2() + (ChunkPer / 2f * ChunkPixelSize.ToSizeF()).ToVector2();

            DrawChunks(target, playerPos, chunkIn, blockIn);
            
            target.DrawText(playerPos.ToString(), DefaultFont,
                new SharpDX.RectangleF(ViewPort.BottomRight.X - 50, ViewPort.BottomRight.Y - 10, 50, 10),
                SolidColorBrushWhite);

        }
        private void DrawChunks(RenderTarget target, VectorInt2 playerPos, VectorInt2 chunkIn, Block blockIn)
        {
            for (int x = Math.Max(0, chunkIn.X - DrawRange.Width); x < Math.Min(World.WorldConfig.WorldSize.Width, chunkIn.X + DrawRange.Width); x++)
            {
                for (int y = Math.Max(0, chunkIn.Y - DrawRange.Height); y < Math.Min(World.WorldConfig.WorldSize.Height, chunkIn.Y + DrawRange.Height); y++)
                {
                    //foreach chunk
                    var chunk = World.Chunks[x, y];

                    //var offset = new Vector2(CameraOffset.X + ViewPort.X + x * chunkPixelWidth,
                    //    CameraOffset.Y + ViewPort.Y + y * chunkPixelHeight);
                    var chunkPos = new VectorInt2(x, y);
                    var offset = CameraOffset + ViewPort.Posistion + chunkPos.ToVector2() * ChunkPixelSize.ToVector2();
                    var rectChunk = new RectangleF(offset, ChunkPixelSize.ToSizeF());
                    if (!ViewPort.ContainsCorner(rectChunk) || chunk == null) continue;

                    DrawBlocks(target, chunk, offset);
                    target.DrawRectangle(rectChunk.RawRectangleF, SolidColorBrushRed);
                    target.DrawText(chunkPos.ToString(), DefaultFont, rectChunk.RawRectangleF, SolidColorBrushWhite);
                }
            }
        }
        private void DrawBlocks(RenderTarget target, Chunk chunk, Vector2 offset)
        {
            for (int x = 0; x < World.WorldConfig.ChunkSize.Width; x++)
            {
                for (int y = 0; y < World.WorldConfig.ChunkSize.Height; y++)
                {
                    //foreach block in chunk
                    var block = chunk.Items[x, y];
                    var blockPos = new VectorInt2(x, y);
                    //var rectBlock = new RectangleF(
                    //    offset.X + x * width,
                    //    offset.Y + y * height,
                    //    width,
                    //    height);
                    var currentOffset = offset;
                    if (block != null) currentOffset += block.DrawOffset.ToVector2();
                    var rectBlock = new RectangleF(currentOffset + blockPos.ToVector2() * BlockPixelSize.ToVector2(),
                        BlockPixelSize.ToSizeF());

                    if (!ViewPort.ContainsCorner(rectBlock)) continue;
                    //TODO: Implement Content Manager
                    target.DrawBitmap(block == null ? Water : Grass, rectBlock.RawRectangleF, 1, BitmapInterpolationMode.NearestNeighbor);
                }
            }
        }
        #endregion

        public void OnInitialize(RenderTarget target, Factory factory, SharpDX.DirectWrite.Factory factoryDr)
        {
            SolidColorBrushWhite = new SolidColorBrush(target, SharpDX.Color.White);
            SolidColorBrushGreen = new SolidColorBrush(target, SharpDX.Color.Green);
            SolidColorBrushBlue = new SolidColorBrush(target, SharpDX.Color.Blue);
            SolidColorBrushRed = new SolidColorBrush(target, SharpDX.Color.Red);
            Grass = Helpers.GetContent(target, "Grass.png");
            Water = Helpers.GetContent(target, "Water.png");
            DefaultFont = new TextFormat(factoryDr, "Courier New", 10)
            {
                TextAlignment = TextAlignment.Center, FlowDirection = FlowDirection.BottomToTop
            };
        }


        public void OnPacketRecived(Packet packet)
        {
            //TODO: Implment Content Manager
        }
    }
}