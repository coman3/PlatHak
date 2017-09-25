using System;
using Microsoft.Xna.Framework;

namespace PlatHak.Client.Common.Interfaces
{
    public interface IUpdate
    {
        void Update(GameTime gameTime);

        event EventHandler<System.EventArgs> EnabledChanged;

        event EventHandler<System.EventArgs> UpdateOrderChanged;

        bool Enabled { get; set; }

        int UpdateOrder { get; }
    }
}