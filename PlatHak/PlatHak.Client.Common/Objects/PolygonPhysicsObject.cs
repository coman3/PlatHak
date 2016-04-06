using System.Collections.Generic;
using System.Linq;
using Box2DX.Collision;
using Box2DX.Common;
using Box2DX.Dynamics;
using PlatHak.Common.Maths;
using SharpDX.Direct2D1;

namespace PlatHak.Client.Common.Objects
{
    public class PolygonPhysicsObjectDef : ObjectDef
    {
        public List<Vector2> Vertices { get; set; }
        public override Body GetBody(World world)
        {
            return world.CreateBody(CreateBodyDef());
        }

        public override void SetShape(Body body)
        {
            body.CreateShape(new PolygonDef
            {
                Vertices = Vertices.Select(x => new Vec2(x.X, x.Y)).ToArray(),
                VertexCount = Vertices.Count,
                Density = Density,
                Friction = Friction,
                UserData = UserData,
                Restitution = Restitution,
            });
            if (!FixedObject) body.SetMassFromShapes();
        }
    }
    public class PolygonPhysicsObject : PhysicsObject<PolygonPhysicsObjectDef>
    {
        public PolygonPhysicsObject(World world, PolygonPhysicsObjectDef objectDef) : base(world, objectDef)
        {
        }

        public override void OnDraw(RenderTarget target, GameTime time)
        {
            
        }

        public override void OnInitialize(RenderTarget target, Factory factory, SharpDX.DirectWrite.Factory factoryDr)
        {
            
        }
    }
}