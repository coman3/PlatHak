using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PlatHak.Client.Common.Interfaces
{
    public interface IDraw
    {
        event EventHandler<System.EventArgs> DrawOrderChanged;
        event EventHandler<System.EventArgs> VisibleChanged;
        int DrawOrder { get; }
        bool Visible { get; set; }

        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}