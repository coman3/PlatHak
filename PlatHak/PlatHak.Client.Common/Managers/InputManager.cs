using System;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PlatHak.Client.Common.Managers
{
    public abstract class InputManager
    {
        protected Func<GamePadState> GetGamePadStateAction;

        public GamePadState GetGamePadState()
        {
            return GetGamePadStateAction();
        }
    }
}