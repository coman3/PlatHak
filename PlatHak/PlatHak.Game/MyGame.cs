using PlatHak.Client.Common;
using PlatHak.Client.Common.Config;
using PlatHak.Client.Common.Helpers;
using SharpDX.Mathematics.Interop;

namespace PlatHak.Game
{
    public class MyGame : Game2D
    {
        public RawColor4 SceneColor { get; set; }
        protected override void Draw(GameTime time)
        {
            RenderTarget2D.Clear(SceneColor);

        }
    }
}