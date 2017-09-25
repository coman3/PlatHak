using System;
using Android.Text.Method;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using PlatHak.Client.Common.Data;
using PlatHak.Client.Common.Interfaces;
using PlatHak.Client.Common.Managers;

namespace PlatHak.Client.Android.Managers
{
    public class AndroidInputManager : InputManager, IElementContainer, ILoad
    {
        public bool Loaded { get; set; }

        
        private VirtualGamePad _virtualGamePad;

        public void LoadContent(GraphicsDevice graphicsDevice, ContentManager contentManager)
        {
            var gamePad = GamePad.GetCapabilities(PlayerIndex.One);
            if (gamePad.IsConnected && gamePad.HasRightXThumbStick && gamePad.HasRightYThumbStick)
            {
                GetGamePadStateAction = () => GamePad.GetState(PlayerIndex.One);
            }
            else
            {
                _virtualGamePad = new VirtualGamePad(this);
                CreateElement?.Invoke(this, new ElementCreateEventArgs(_virtualGamePad));
                GetGamePadStateAction = GetVirtualGamePadState;
            }
        }

        private GamePadState GetVirtualGamePadState()
        {
            return _virtualGamePad.GetState(TouchPanel.GetState(), GamePad.GetState(PlayerIndex.One));
        }

        public event EventHandler<ElementCreateEventArgs> CreateElement;
    }
}