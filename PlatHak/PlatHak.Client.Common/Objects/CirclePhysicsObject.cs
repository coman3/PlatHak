using System.Runtime.InteropServices;
using Box2DX.Collision;
using Box2DX.Dynamics;
using PlatHak.Common.Maths;
using SharpDX;
using SharpDX.Direct2D1;

namespace PlatHak.Client.Common.Objects
{
    public class CirclePhysicsObjectDef : ObjectDef
    {
        public float Radius { get; set; }
        public override Body GetBody(World world)
        {
            return world.CreateBody(CreateBodyDef());
        }

        public override void SetShape(Body body)
        {
            body.CreateShape(new CircleDef
            {
                Radius = Radius,
                Density = Density,
                Friction = Friction,
                IsSensor = false,
                Restitution = Restitution,
            });
            if (!FixedObject) body.SetMassFromShapes();
        }
    }
    public class CirclePhysicsObject : PhysicsObject<CirclePhysicsObjectDef>
    {
        private Ellipse _ellipse;
        public SolidColorBrush ColorBrush { get; set; }
        public override void OnDraw(RenderTarget target, GameTime time)
        {
            _ellipse.Point = Posistion.RawVector2;
            target.Transform = Rotation.Matrix3X2;
            target.DrawEllipse(_ellipse, ColorBrush);
            target.Transform = Matrix3x2.Identity;
        }

        public override void OnInitialize(RenderTarget target, Factory factory, SharpDX.DirectWrite.Factory factoryDr)
        {
            _ellipse = new Ellipse(Posistion.RawVector2, ObjectDef.Radius, ObjectDef.Radius);
            ColorBrush = new SolidColorBrush(target, ObjectDef.ShapeDebugColor.ToRawColor4());
        }

        public CirclePhysicsObject(World world, CirclePhysicsObjectDef objectDef) : base(world, objectDef)
        {
        }
    }
}