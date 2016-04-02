using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Box2DX.Collision;
using Box2DX.Dynamics;
using PlatHak.Client.Common.Helpers;
using PlatHak.Common.Maths;
using SharpDX.Direct2D1;

namespace PlatHak.Client.Common.Objects
{
    public abstract class PhysicsObject : GameObject
    {
        public PolygonDef PolygonDef { get; set; }
        public Body Body { get;  }
        public Shape Shape { get; }
        public new Vector2 Posistion => Body.GetPosition().GetVector2();
        public new Rotation Rotation => Body.GetXForm().GetRotation();

        protected PhysicsObject(World world, BodyDef bodyDef, ShapeDef shapeDef, bool setMass)
        {
            Body = world.CreateBody(bodyDef);
            Shape = Body.CreateShape(shapeDef);
            PolygonDef = shapeDef as PolygonDef;
            if(setMass) Body.SetMassFromShapes();
        }
    }
}