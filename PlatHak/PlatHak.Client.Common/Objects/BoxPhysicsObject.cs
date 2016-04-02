using System.Linq;
using Box2DX.Collision;
using Box2DX.Dynamics;
using PlatHak.Client.Common.Helpers;
using PlatHak.Common.Maths;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;
using RectangleF = SharpDX.RectangleF;

namespace PlatHak.Client.Common.Objects
{
    public class BoxPhysicsObject : PhysicsObject
    {
        private SolidColorBrush brush;
        public BoxPhysicsObject(World world, BodyDef bodyDef, ShapeDef shapeDef, bool setMass = true) : base(world, bodyDef, shapeDef, setMass)
        {
        }

        public override void OnDraw(RenderTarget target, GameTime time)
        {
            //target.Transform = Rotation.Matrix3X2;
            //target.DrawRectangle(new RectangleF(Posistion.X, Posistion.Y, 100, 100), brush);
            //target.Transform = Matrix3x2.Identity;
        }

        public override void OnInitialize(RenderTarget target, Factory factory)
        {
            //brush = new SolidColorBrush(target, new RawColor4(255, 0, 0, 255)); 
        }
    }
}