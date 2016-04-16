using System;
using PlatHak.Common.Maths;

namespace PlatHak.Common.World
{
    [Serializable]
    public class Player
    {
        public string Username { get; set; }
        public VectorInt2 Posistion { get; set; }
    }
}