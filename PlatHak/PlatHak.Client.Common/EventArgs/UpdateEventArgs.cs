using Microsoft.Xna.Framework;

namespace PlatHak.Client.Common.EventArgs
{
    internal class UpdateEventArgs : System.EventArgs
    {
        public GameTime GameTime { get; set; }
    }
}