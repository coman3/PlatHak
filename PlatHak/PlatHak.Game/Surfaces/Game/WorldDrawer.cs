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
        public Bitmap Stick { get; set; }
        public WorldDrawer(ref ClientWorldManager worldManager, RectangleF viewPort)
        {
            ZoomScale = 1f;
            World = worldManager.World;
            ViewPort = viewPort;
            Player = worldManager.Player;
            var chunkPixelWidth = World.WorldConfig.ItemSize.Width * World.WorldConfig.ChunkSize.Width;
            var chunkPixelHeight = World.WorldConfig.ItemSize.Height * World.WorldConfig.ChunkSize.Height;
            var chunkPerWidth = (int)ViewPort.Width / chunkPixelWidth;
            var chunkPerHeight = (int)ViewPort.Height / chunkPixelHeight;
            DrawRange = new Size(chunkPerWidth + 1, chunkPerHeight + 1);
        }

        public void Draw(RenderTarget target, GameTime time)
        {
            if (World == null || Player == null) return;
            var playerPos = Player.Posistion;
            var chunkIn = World.GetChunkCordsFromPosition(Player.Posistion);
            var blockInPos = World.GetBlockCordsFromPosistion(playerPos);
            var width = World.WorldConfig.ItemSize.Width;
            var height = World.WorldConfig.ItemSize.Height;
            var chunkPixelWidth = width*World.WorldConfig.ChunkSize.Width;
            var chunkPixelHeight = height*World.WorldConfig.ChunkSize.Height;
            var chunkPerWidth = ViewPort.Width/chunkPixelWidth;
            var chunkPerHeight = ViewPort.Height/chunkPixelHeight;
            var cameraOffset = new Vector2( 0 - playerPos.X + ViewPort.Width / 2f,
                0 - playerPos.Y + ViewPort.Height / 2f);

            DrawChunks(target, chunkIn, width, height, chunkPixelWidth, chunkPixelHeight, cameraOffset);
            DrawPlayers(target, playerPos, World.Players, cameraOffset);
            target.DrawText(playerPos.ToString(), DefaultFont,
                new SharpDX.RectangleF(ViewPort.BottomRight.X - 50, ViewPort.BottomRight.Y - 10, 50, 10),
                SolidColorBrush);

        }

        private void DrawPlayers(RenderTarget target, VectorInt2 playerPos, List<Player> players, Vector2 cameraOffset)
        {
            var rect = new RectangleF(playerPos.Vector2 + cameraOffset,
                new SizeF(World.WorldConfig.ItemSize.Width, World.WorldConfig.ItemSize.Height));
            if(ViewPort.Contains(rect)) target.DrawBitmap(Stick, rect.RawRectangleF, 1, BitmapInterpolationMode.NearestNeighbor);
            foreach (var player in players)
            {
                var rectPos = new RectangleF(player.Posistion.Vector2 + cameraOffset,
                 new SizeF(World.WorldConfig.ItemSize.Width, World.WorldConfig.ItemSize.Height));
                if (ViewPort.Contains(rectPos)) target.DrawBitmap(Stick, rectPos.RawRectangleF, 1, BitmapInterpolationMode.NearestNeighbor);
            }
        }

        private void DrawChunks(RenderTarget renderTarget, VectorInt2 chunkPlayerIn, int blockWidth, int blockHeight, int chunkPixelWidth, int chunkPixelHeight, Vector2 cameraWorldOffset)
        {
            for (int x = Math.Max(0, chunkPlayerIn.X - DrawRange.Width); x < Math.Min(World.WorldConfig.WorldSize.Width, chunkPlayerIn.X + DrawRange.Width); x++)
            {
                for (int y = Math.Max(0, chunkPlayerIn.Y - DrawRange.Height); y < Math.Min(World.WorldConfig.WorldSize.Height, chunkPlayerIn.Y + DrawRange.Height); y++)
                {
                    //foreach chunk
                    var chunk = World.Chunks[x, y];
                    var offset = new Vector2(cameraWorldOffset.X + ViewPort.X + x * chunkPixelWidth,
                        cameraWorldOffset.Y + ViewPort.Y + y * chunkPixelHeight);
                    var rectChunk = new RectangleF(offset.X, offset.Y, chunkPixelWidth, chunkPixelHeight);
                    //TODO: Improve OnScreen Check
                    if (!ViewPort.Contains(rectChunk)) continue;

                    if(chunk != null) DrawBlocks(renderTarget, blockWidth, blockHeight, chunk, offset);

                    renderTarget.DrawText($"{x}:{y}", DefaultFont, rectChunk.RawRectangleF, SolidColorBrush);
                    renderTarget.DrawRectangle(rectChunk.RawRectangleF, chunk == null ? SolidColorBrushRed : SolidColorBrushGreen);
 
                }
            }
        }

        private void DrawBlocks(RenderTarget target, int width, int height, Chunk chunk, Vector2 offset)
        {
            for (int cx = 0; cx < World.WorldConfig.ChunkSize.Width; cx++)
            {
                for (int cy = 0; cy < World.WorldConfig.ChunkSize.Height; cy++)
                {
                    //foreach block in chunk
                    var block = chunk.Items[cx, cy];

                    var rectBlock = new RectangleF(
                        offset.X + cx*width,
                        offset.Y + cy*height,
                        width, height);
                    if (block == null)
                    {
                        target.DrawBitmap(Water, rectBlock.RawRectangleF, 1, BitmapInterpolationMode.NearestNeighbor);
                        continue;
                    }
                    //TODO: Improve OnScreen Check
                    if (!ViewPort.Contains(rectBlock)) continue;
                    target.DrawBitmap(Grass, rectBlock.RawRectangleF, 1, BitmapInterpolationMode.NearestNeighbor);

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
            Stick = Helpers.GetContent(target, "Stick.png");
            DefaultFont = new TextFormat(factoryDr, "Courier New", 13)
            {
                TextAlignment = TextAlignment.Center, FlowDirection = FlowDirection.BottomToTop
            };
        }

        
    }
}