using SharpDX.Text;

namespace PlatHack.Game
{
    public abstract class Surface
    {
        public MyGame Game { get; set; }

        protected Surface(MyGame game)
        {
            Game = game;
        }

        public bool IsDrawSurface => (this as IDrawSurface) != null;
        public bool IsUpdatedSurface => (this as IUpdatedSurface) != null;
        public bool IsInitializedSurface => (this as IInitializedSurface) != null;
        public bool IsInputSurface => (this as IInputSurface) != null;
        public bool IsPacketReciverSurface => (this as IPacketReciverSurface) != null;

    }
}