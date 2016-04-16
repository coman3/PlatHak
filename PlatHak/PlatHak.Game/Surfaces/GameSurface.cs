using System;
using System.Collections.Generic;
using System.Linq;
using PlatHak.Client.Common;
using PlatHak.Client.Common.Interfaces;
using PlatHak.Common.Maths;
using PlatHak.Common.Network;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;

namespace PlatHack.Game.Surfaces
{
    public abstract class GameSurface : Surface, IUpdatedSurface, IPacketReciverSurface, IInputSurface
    {
        public MyGame Game { get; set; }
        
        protected GameSurface(RectangleF viewPort, MyGame game) : base(viewPort)
        {
            Game = game;
            
        }

        public virtual void OnUpdate(GameTime time)
        {
            foreach (var result in Surfaces.OfType<IUpdatedSurface>())
            {
                result?.OnUpdate(time);
            }
        }
        public abstract void OnPacketRecived(Packet packet);
        public abstract void OnInput(InputEventArgs args);

       
    }
    
}