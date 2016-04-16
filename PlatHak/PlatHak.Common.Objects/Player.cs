using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlatHak.Common.Maths;

namespace PlatHak.Common.Objects
{
    public partial class Player : GameObject
    {
        public Player(Guid id, string name, Vector2 posistion) : base(id, name, posistion)
        {
        }


    }
}
