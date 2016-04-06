using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlatHak.Client.Common.Interfaces;
using PlatHak.Client.Common.Objects;
using PlatHak.Common;

namespace PlatHack.Game.Surfaces.Dragables
{
    class CircleDragItem : DragItem
    {
        public CirclePhysicsObjectDef ObjectDef { get; set; }

        public CircleDragItem(CirclePhysicsObjectDef objectDef)
        {
            ObjectDef = objectDef;
        }
    }
}
