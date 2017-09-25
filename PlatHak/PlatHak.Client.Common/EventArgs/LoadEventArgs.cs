using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PlatHak.Client.Common.EventArgs
{
    internal class LoadEventArgs : System.EventArgs
    {
        public GraphicsDevice GraphicsDevice { get; set; }
        public ContentManager ContentManager { get; set; }
    }
}