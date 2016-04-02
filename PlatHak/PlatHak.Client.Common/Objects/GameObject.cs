using PlatHak.Client.Common.Helpers;
using PlatHak.Client.Common.Interfaces;
using PlatHak.Common.Maths;
using SharpDX;
using SharpDX.Direct2D1;
using RectangleF = PlatHak.Common.Maths.RectangleF;
using Vector2 = PlatHak.Common.Maths.Vector2;

namespace PlatHak.Client.Common.Objects
{
    public abstract class GameObject : IDrawSurface, IInitializedSurface
    {
        public Rotation Rotation { get; set; }
        public Vector2 Posistion { get; set; }
        public Vector2 Velocity { get; set; }
        public Vector2 Acceleration { get; set; }

        public bool IsFixedInSpace { get; set; }

        protected GameObject(Rotation rotation, Vector2 posistion, Vector2 velocity, Vector2 acceleration)
        {
            Rotation = rotation;
            Posistion = posistion;
            Velocity = velocity;
            Acceleration = acceleration;
            IsFixedInSpace = false;
        }

        protected GameObject()
        {
            
        }

        protected GameObject(Vector2 posistion)
        {
            Rotation = Rotation.Zero;
            Posistion = posistion;
            Velocity = Vector2.Zero;
            Acceleration = Vector2.Zero;
            IsFixedInSpace = true;
        }

        public virtual void Draw(RenderTarget target, GameTime time)
        {
            target.Transform = Rotation.Matrix3X2;
            OnDraw(target, time);
            target.Transform = Matrix3x2.Identity;
        }

        public abstract void OnDraw(RenderTarget target, GameTime time);
        public abstract void OnInitialize(RenderTarget target, Factory factory);
    }
}