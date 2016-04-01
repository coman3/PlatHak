using PlatHak.Client.Common.Helpers;
using PlatHak.Common.Network;
using SharpDX.Direct2D1;

namespace PlatHack.Game
{
    public abstract class GameSurface : Surface, IUpdatedSurface, IDrawSurface, IPacketReciverSurface, IInitializedSurface, IInputSurface
    {
        protected GameSurface(MyGame game) : base(game)
        {

        }

        public abstract void OnUpdate(GameTime time);
        public abstract void Draw(RenderTarget target, GameTime time);
        public abstract void OnPacketRecived(Packet packet);
        public abstract void OnInput(InputEventArgs args);
        public abstract void OnInitialize(RenderTarget target);
    }
}