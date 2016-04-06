using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Box2DX.Collision;
using Box2DX.Common;
using Box2DX.Dynamics;
using PlatHak.Client.Common.Interfaces;
using PlatHak.Common.Maths;
using SharpDX.Direct2D1;
using Color = PlatHak.Common.Color;

namespace PlatHak.Client.Common.Objects
{
    public abstract class ObjectDef
    {
        //BodyDef
        public Vector2 Posistion { get; set; }
        public float Density { get; set; }
        public MassData? MassData { get; set; }
        public bool AllowSleep { get; set; }
        public float Angle { get; set; }
        public float AngularDamping { get; set; }
        public float LinearDamping { get; set; }
        public bool FixedRotation { get; set; }
        public bool IsBullet { get; set; }
        public bool IsSleeping { get; set; }
        public object UserData { get; set; }

        //ShapeDef
        public float Friction { get; set; }
        public FilterData FilterData { get; set; }
        public float Restitution { get; set; }
        
        //Other
        public Color ShapeDebugColor { get; set; }
        public bool FixedObject { get; set; }
        protected BodyDef CreateBodyDef()
        {
            var body = new BodyDef
            {
                AllowSleep = AllowSleep,
                Angle = Angle,
                Position = new Vec2(Posistion.X, Posistion.Y),
                AngularDamping = AngularDamping,
                FixedRotation = FixedRotation,
                IsBullet = IsBullet,
                IsSleeping = IsSleeping,
                LinearDamping = LinearDamping,
                UserData = UserData,
            };
            if (MassData.HasValue) body.MassData = MassData.Value;
            if(FixedObject) body.MassData = new MassData { Center = Vec2.Zero, I = 0, Mass = 0 };
            return body;
        }


        public abstract Body GetBody(World world);
        public abstract void SetShape(Body body);
    }
    public abstract class PhysicsObject<T> : GameObject, IPhysicsSurface 
        where T : ObjectDef
    {
        public Body Body { get; }
        public T ObjectDef { get; set; }
        public new Vector2 Posistion => Body.GetPosition().GetVector2();
        public new Rotation Rotation => Body.GetXForm().GetRotation();

        protected PhysicsObject(World world, T objectDef)
        {
            ObjectDef = objectDef;
            Body = objectDef.GetBody(world);
            objectDef.SetShape(Body);
            
        }
    }
}