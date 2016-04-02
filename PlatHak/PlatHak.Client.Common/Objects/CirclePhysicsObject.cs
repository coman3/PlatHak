using Box2DX.Collision;
using Box2DX.Dynamics;
using PlatHak.Client.Common.Helpers;
using SharpDX.Direct2D1;

namespace PlatHak.Client.Common.Objects
{
    public class CirclePhysicsObject : PhysicsObject
    {
        public CirclePhysicsObject(World world, BodyDef bodyDef, CircleDef shapeDef, bool setMass) : base(world, bodyDef, shapeDef, setMass)
        {

        }

        public override void OnDraw(RenderTarget target, GameTime time)
        {
            
        }

        public override void OnInitialize(RenderTarget target, Factory factory)
        {
            
        }
    }
}