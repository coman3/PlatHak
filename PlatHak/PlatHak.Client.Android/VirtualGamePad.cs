using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using PlatHak.Client.Android.Managers;
using PlatHak.Client.Common.Interfaces;

namespace PlatHak.Client.Android
{
    internal class VirtualGamePad : IUpdate, IDraw, ILoad
    {
        public event EventHandler<EventArgs> DrawOrderChanged;
        public event EventHandler<EventArgs> VisibleChanged;

        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;
        public bool Enabled { get; set; }
        public int UpdateOrder => 0;

        public int DrawOrder => Common.Data.DrawOrder.ControlsTop;
        public bool Visible { get; set; }
        public bool Loaded { get; set; }

        private Rectangle _viewPort;
        private Texture2D _texture;

        public AndroidInputManager InputManager { get; set; }
        private float _secondsSinceLastInput;
        private float _opacity;

        public VirtualGamePad(AndroidInputManager inputManager)
        {
            InputManager = inputManager;
            _secondsSinceLastInput = float.MaxValue;
            Visible = true;
            Enabled = true;
        }

        public void NotifyPlayerIsMoving()
        {
            _secondsSinceLastInput = 0;
        }

        public void Update(GameTime gameTime)
        {
            if (!Enabled) return;
            var secondsElapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _secondsSinceLastInput += secondsElapsed;

            //If the player is moving, fade the controls out
            // otherwise, if they haven't moved in 4 seconds, fade the controls back in
            _opacity = _secondsSinceLastInput < 4 ? Math.Max(0, _opacity - secondsElapsed * 4) : Math.Min(1, _opacity + secondsElapsed * 2);
        }

        /// <summary>
        /// Generates a GamePadState based on the touch input provided (as applied to the on screen controls) and the gamepad state
        /// </summary>
        public GamePadState GetState(TouchCollection touchState, GamePadState gpState)
        {
            //Work out what buttons are pressed based on the touchState
            Buttons buttonsPressed = 0;

            foreach (var touch in touchState)
            {
                if (touch.State != TouchLocationState.Moved && touch.State != TouchLocationState.Pressed) continue;

                //Scale the touch position to be in _baseScreenSize coordinates
                Vector2 pos = touch.Position;

                if (pos.X < 128)
                    buttonsPressed |= Buttons.DPadLeft;
                else if (pos.X < 256)
                    buttonsPressed |= Buttons.DPadRight;
                else if (pos.X >= _viewPort.Right - 128)
                    buttonsPressed |= Buttons.A;
            }

            //Combine the buttons of the real gamepad
            var gpButtons = gpState.Buttons;
            buttonsPressed |= gpButtons.A == ButtonState.Pressed ? Buttons.A : 0;
            buttonsPressed |= gpButtons.B == ButtonState.Pressed ? Buttons.B : 0;
            buttonsPressed |= gpButtons.X == ButtonState.Pressed ? Buttons.X : 0;
            buttonsPressed |= gpButtons.Y == ButtonState.Pressed ? Buttons.Y : 0;

            buttonsPressed |= gpButtons.Start == ButtonState.Pressed ? Buttons.Start : 0;
            buttonsPressed |= gpButtons.Back == ButtonState.Pressed ? Buttons.Back : 0;

            buttonsPressed |= gpState.IsButtonDown(Buttons.DPadDown) ? Buttons.DPadDown : 0;
            buttonsPressed |= gpState.IsButtonDown(Buttons.DPadLeft) ? Buttons.DPadLeft : 0;
            buttonsPressed |= gpState.IsButtonDown(Buttons.DPadRight) ? Buttons.DPadRight : 0;
            buttonsPressed |= gpState.IsButtonDown(Buttons.DPadUp) ? Buttons.DPadUp : 0;

            buttonsPressed |= gpButtons.BigButton == ButtonState.Pressed ? Buttons.BigButton : 0;
            buttonsPressed |= gpButtons.LeftShoulder == ButtonState.Pressed ? Buttons.LeftShoulder : 0;
            buttonsPressed |= gpButtons.RightShoulder == ButtonState.Pressed ? Buttons.RightShoulder : 0;

            buttonsPressed |= gpButtons.LeftStick == ButtonState.Pressed ? Buttons.LeftStick : 0;
            buttonsPressed |= gpButtons.RightStick == ButtonState.Pressed ? Buttons.RightStick : 0;

            var buttons = new GamePadButtons(buttonsPressed);

            return new GamePadState(gpState.ThumbSticks, gpState.Triggers, buttons, gpState.DPad);
        }

        
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if(!Visible) return;
            var spriteCenter = new Vector2(64, 64);
            var color = Color.Multiply(Color.White, _opacity);

            spriteBatch.Draw(_texture, new Vector2(64, _viewPort.Bottom - 64), null, color, -MathHelper.PiOver2, spriteCenter, 1, SpriteEffects.None, 0);
            spriteBatch.Draw(_texture, new Vector2(192, _viewPort.Bottom - 64), null, color, MathHelper.PiOver2, spriteCenter, 1, SpriteEffects.None, 0);
            spriteBatch.Draw(_texture, new Vector2(_viewPort.Right - 128, _viewPort.Bottom - 128), null, color, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
        }

        

        public void LoadContent(GraphicsDevice graphicsDevice, ContentManager contentManager)
        {
            _viewPort = graphicsDevice.Viewport.TitleSafeArea;
            _texture = contentManager.Load<Texture2D>(Common.Data.Content.Sprites.VirtualControlArrow);
        }
    }
}