using System;
using System.Collections.Generic;
using System.Linq;
using PlatHak.Client.Common;
using PlatHak.Client.Common.Interfaces;
using PlatHak.Common.Maths;
using PlatHak.Common.World;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using Factory = SharpDX.Direct2D1.Factory;

namespace PlatHack.Game.Surfaces.Game
{
    public class WorldDrawer : IDrawSurface, IInitializedSurface
    {
        public World World;
        public Player Player;
        public RectangleF ViewPort { get; set; }
        public float ZoomScale { get; set; }
        public Size DrawRange { get; set; }
        public Dictionary<Block, Bitmap> Bitmaps; 
        public SolidColorBrush SolidColorBrush { get; set; }
        public SolidColorBrush SolidColorBrushGreen { get; set; }
        public SolidColorBrush SolidColorBrushBlue { get; set; }
        public SolidColorBrush SolidColorBrushRed { get; set; }
        public TextFormat DefaultFont { get; set; }
        public Bitmap Grass { get; set; }
        public Bitmap Water { get; set; }
        public WorldDrawer(ref ClientWorldManager worldManager, RectangleF viewPort)
        {
            ZoomScale = 1f;
            World = worldManager.World;
            ViewPort = viewPort;
            Player = worldManager.Player;
            DrawRange = new Size(4, 3);
        }

        public void Draw(RenderTarget target, GameTime time)
        {
            if (World == null || Player == null) return;
            var playerPos = Player.Posistion;
            var chunkIn = World.GetChunkCordsFromPosition(Player.Posistion);
            var blockInPos = World.GetBlockCordsFromPosistion(playerPos);
            var blockIn = World.GetBlockFromPosistion(playerPos);
            var width = World.WorldConfig.ItemSize.Width;
            var height = World.WorldConfig.ItemSize.Height;
            var chunkPixelWidth = width * World.WorldConfig.ChunkSize.Width;
            var chunkPixelHeight = height * World.WorldConfig.ChunkSize.Height;
            var chunkPerWidth = ViewPort.Width / chunkPixelWidth;
            var chunkPerHeight = ViewPort.Height / chunkPixelHeight;
            var cameraOffset = new Vector2(
                -(chunkIn.X * chunkPixelWidth + (blockInPos.X * width)) +
                chunkPerWidth / 2f * chunkPixelWidth,
                -(chunkIn.Y * chunkPixelHeight + blockInPos.Y * height) +
                chunkPerHeight / 2f * chunkPixelHeight);

            var playersBlockIn = World.Players.Select(player => World.GetBlockFromPosistion(player.Posistion)).Where(block => block != null).ToList();

            DrawChunks(target, playerPos, chunkIn, blockIn, width, height, chunkPixelWidth, chunkPixelHeight, cameraOffset, playersBlockIn);

        }

        private void DrawChunks(RenderTarget target, VectorInt2 playerPos, VectorInt2 chunkIn, Block blockIn, int width, int height, int chunkPixelWidth, int chunkPixelHeight, Vector2 cameraOffset, List<Block> playersBlockIn)
        {
            for (int x = Math.Max(0, chunkIn.X - DrawRange.Width); x < Math.Min(World.WorldConfig.WorldSize.Width, chunkIn.X + DrawRange.Width); x++)
            {
                for (int y = Math.Max(0, chunkIn.Y - DrawRange.Height); y < Math.Min(World.WorldConfig.WorldSize.Height, chunkIn.Y + DrawRange.Height); y++)
                {
                    //foreach chunk
                    var chunk = World.Chunks[x, y];
                    var offset = new Vector2(cameraOffset.X + ViewPort.X + x * chunkPixelWidth,
                        cameraOffset.Y + ViewPort.Y + y * chunkPixelHeight);
                    var rectChunk = new RectangleF(
                        offset.X,
                        offset.Y,
                        chunkPixelWidth,
                        chunkPixelHeight);
                    //TODO: Improve OnScreen Check
                    if (!ViewPort.Contains(rectChunk.TopLeft) && !ViewPort.Contains(rectChunk.BottomRight) &&
                        !ViewPort.Contains(rectChunk.TopRight) && !ViewPort.Contains(rectChunk.BottomLeft)) continue;
                    if (chunk == null) continue;
                    DrawBlocks(target, blockIn, width, height, playersBlockIn, chunk, offset);

                    target.DrawText($"{x}:{y}", DefaultFont, rectChunk.RawRectangleF, SolidColorBrush);
                    target.DrawRectangle(rectChunk.RawRectangleF, SolidColorBrushRed);
                    target.DrawText(playerPos.ToString(), DefaultFont,
                        new SharpDX.RectangleF(ViewPort.BottomRight.X - 50, ViewPort.BottomRight.Y - 10, 50, 10),
                        SolidColorBrush);
                }
            }
        }

        private void DrawBlocks(RenderTarget target, Block blockIn, int width, int height, List<Block> playersBlockIn, Chunk chunk, Vector2 offset)
        {
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
                    if (block == null)
                    {
                        target.DrawBitmap(Water, rectBlock.RawRectangleF, 1, BitmapInterpolationMode.NearestNeighbor);
                        continue;
                    }
                    //TODO: Improve OnScreen Check
                    if (!ViewPort.Contains(rectBlock.TopLeft) && !ViewPort.Contains(rectBlock.BottomRight) &&
                        !ViewPort.Contains(rectBlock.TopRight) && !ViewPort.Contains(rectBlock.BottomLeft))
                        continue;
                    var brush = block == blockIn ? SolidColorBrushGreen : (playersBlockIn.Contains(block) ? SolidColorBrushBlue : SolidColorBrush);
                    
                    if (block == blockIn)
                    {
                        target.FillRectangle(rectBlock.RawRectangleF, SolidColorBrushGreen);
                    }
                    else if (playersBlockIn.Contains(block))
                    {
                        target.FillRectangle(rectBlock.RawRectangleF, SolidColorBrushBlue);
                    }
                    else
                    {
                        target.DrawBitmap(Grass, rectBlock.RawRectangleF, 1, BitmapInterpolationMode.NearestNeighbor);
                        target.DrawRectangle(rectBlock.RawRectangleF, brush);
                    }
                    

                }
            }
        }

        public void OnInitialize(RenderTarget target, Factory factory, SharpDX.DirectWrite.Factory factoryDr)
        {
            SolidColorBrush = new SolidColorBrush(target, SharpDX.Color.White);
            SolidColorBrushGreen = new SolidColorBrush(target, SharpDX.Color.Green);
            SolidColorBrushBlue = new SolidColorBrush(target, SharpDX.Color.Blue);
            SolidColorBrushRed = new SolidColorBrush(target, SharpDX.Color.Red);
            Grass = Helpers.GetContent(target, "Grass.png");
            Water = Helpers.GetContent(target, "Water.png");
            DefaultFont = new TextFormat(factoryDr, "Courier New", 8)
            {
                TextAlignment = TextAlignment.Center, FlowDirection = FlowDirection.BottomToTop
            };
        }

        
    }
}