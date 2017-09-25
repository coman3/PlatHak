using System;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using PlatHak.Client.Common.EventArgs;
using PlatHak.Client.Common.Interfaces;
using PlatHak.Client.Common.Managers;

namespace PlatHak.Client.Common
{
    public abstract class Game : Microsoft.Xna.Framework.Game
    {
        internal event EventHandler<UpdateEventArgs> OnUpdated;
        internal event EventHandler<DrawEventArgs> OnDraw;
        internal event EventHandler<LoadEventArgs> OnLoad;
        public EventManager EventManager { get; set; }

        public Camera2D Camera2D { get; set; }
        public IPlatformManager PlatformManager { get; set; }

        public static SpriteFont DefaultFont { get; private set; }

        readonly GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;

        protected Game(IPlatformManager platformManager)
        {
            EventManager = new EventManager(this);
            PlatformManager = platformManager;
            EventManager.AddElement(PlatformManager.InputManager);
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            DefaultFont = Content.Load<SpriteFont>(Data.Content.Fonts.Roboto);
            OnLoad?.Invoke(this, new LoadEventArgs{ ContentManager = Content, GraphicsDevice = GraphicsDevice});
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            OnUpdated?.Invoke(this, new UpdateEventArgs{ GameTime = gameTime });
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            _spriteBatch.Begin();
            OnDraw?.Invoke(this, new DrawEventArgs() { GameTime = gameTime, SpriteBatch = _spriteBatch});
            _spriteBatch.End();
        }
    }
}
