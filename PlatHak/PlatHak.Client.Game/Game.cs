using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using PlatHak.Client.Common.Managers;
using PlatHak.Client.Network;

namespace PlatHak.Client.Game
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class PlatHakGame : Common.Game
    {
        public WebSocketClient Client { get; set; }

        public PlatHakGame(InputManager inputManager) : base(inputManager)
        {
            Client = new WebSocketClient(new WebSocketClientConfig { ServerAddress = "127.0.0.1", Port = 3344 });
        }
        protected override void Initialize()
        {
            base.Initialize();
            // TODO: Add your initialization logic here

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();
            //TODO: Use Content to load your game content here 

        }
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            // TODO: Add your update logic here            

        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkMagenta);
            base.Draw(gameTime);
            //TODO: Add your drawing code here   

        }
    }
}

