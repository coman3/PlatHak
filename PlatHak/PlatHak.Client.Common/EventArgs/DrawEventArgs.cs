using Microsoft.Xna.Framework.Graphics;

namespace PlatHak.Client.Common.EventArgs
{
    internal class DrawEventArgs : UpdateEventArgs
    {
        public SpriteBatch SpriteBatch { get; set; }
    }
}