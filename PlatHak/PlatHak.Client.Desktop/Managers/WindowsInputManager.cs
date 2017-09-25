using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PlatHak.Client.Common.Interfaces;
using PlatHak.Client.Common.Managers;

namespace PlatHak.Client.Desktop.Managers
{
    public class WindowsInputManager : InputManager, IUpdate, IDraw
    {
        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;
        public event EventHandler<EventArgs> DrawOrderChanged;
        public event EventHandler<EventArgs> VisibleChanged;

        public bool Enabled { get; set; }
        public bool Visible { get; set; }

        public int UpdateOrder => 0;
        public int DrawOrder => Common.Data.DrawOrder.UiTop;

        private GameTime _lastGameTime;
        public WindowsInputManager()
        {
            GetGamePadStateAction = CreateGamePadState;
            Visible = true;
            Enabled = true;
        }

        private GamePadState CreateGamePadState()
        {
            var keyboard = Keyboard.GetState();
            Vector2 rightThumb = Vector2.Zero;
            if (keyboard.IsKeyDown(Keys.A))
            {
                rightThumb = new Vector2(-1, rightThumb.Y);
            }
            if (keyboard.IsKeyDown(Keys.D))
            {
                rightThumb = new Vector2(1, rightThumb.Y);
            }
            if (keyboard.IsKeyDown(Keys.W))
            {
                rightThumb = new Vector2(rightThumb.X, -1);
                if (rightThumb.X > 0 || rightThumb.X < 0)
                    rightThumb = rightThumb / 2;
            }
            if (keyboard.IsKeyDown(Keys.S))
            {
                rightThumb = new Vector2(rightThumb.X, 1);
                if (rightThumb.X > 0 || rightThumb.X < 0)
                    rightThumb = rightThumb / 2;
            }
            return new GamePadState(Vector2.Zero, rightThumb, 0, 0, 0);
        }

        public void Update(GameTime gameTime)
        {
            if (!Enabled) return;
            _lastGameTime = gameTime;
        }

 
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if(!Visible) return;
            spriteBatch.DrawString(Common.Game.DefaultFont, $"{GetGamePadState().ThumbSticks.Right}", new Vector2(10, 10), Color.White);
        }
    }
}