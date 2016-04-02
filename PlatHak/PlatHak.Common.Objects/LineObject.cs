using System;
using PlatHak.Common.Maths;

namespace PlatHak.Common.Objects
{
    public class LineObject : GameObject
    {
        public Vector2 End { get; set; }
        public LineObject( string name, Vector2 start, Vector2 end, int width) : base(Guid.NewGuid(), name, start)
        {
            
        }
    }
}