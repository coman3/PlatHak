using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PlatHak.Client.Common.Interfaces
{
    public interface ILoad
    {
        bool Loaded { get; set; }
        void LoadContent(GraphicsDevice graphicsDevice, ContentManager contentManager);
    }
}