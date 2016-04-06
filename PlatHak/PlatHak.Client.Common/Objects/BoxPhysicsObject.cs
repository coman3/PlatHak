using System.Linq;
using Box2DX.Collision;
using Box2DX.Common;
using Box2DX.Dynamics;
using PlatHak.Common.Maths;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;
using Color = PlatHak.Common.Color;
using RectangleF = SharpDX.RectangleF;
using Vector2 = PlatHak.Common.Maths.Vector2;

namespace PlatHak.Client.Common.Objects
{

    public class BoxPhysicsObjectDef : ObjectDef
    {
        public float Width { get; set; }
        public float Height { get; set; }

        public override Body GetBody(World world)
        {
            return world.CreateBody(CreateBodyDef());
        }
        
        public override void SetShape(Body body)
        {
            var polygonDef = new PolygonDef
            {
                Density = Density,
                Friction = Friction,
                IsSensor = false,
                Restitution = Restitution,
                UserData = UserData,
            };
            polygonDef.SetAsBox(Width / 2f, Height / 2f);
            body.CreateShape(polygonDef);
            if(!FixedObject) body.SetMassFromShapes();
        }
    }
    public class BoxPhysicsObject : PhysicsObject<BoxPhysicsObjectDef>
    {
        public SolidColorBrush ColorBrush { get; set; }
        public BoxPhysicsObject(World world, BoxPhysicsObjectDef objectDef) : base(world, objectDef)
        {
        }

        public override void OnDraw(RenderTarget target, GameTime time)
        {
            target.Transform = Rotation.Matrix3X2;
            target.DrawRectangle(new RectangleF(Posistion.X - ObjectDef.Width / 2f, Posistion.Y - ObjectDef.Height /2f, ObjectDef.Width, ObjectDef.Height), ColorBrush);
            target.Transform = Matrix3x2.Identity;
        }

        public override void OnInitialize(RenderTarget target, Factory factory, SharpDX.DirectWrite.Factory factoryDr)
        {
            ColorBrush = new SolidColorBrush(target, ObjectDef.ShapeDebugColor.ToRawColor4()); 
        }
    }
}