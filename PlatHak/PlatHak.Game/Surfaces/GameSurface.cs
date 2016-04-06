using System;
using System.Collections.Generic;
using System.Linq;
using Box2DX.Collision;
using Box2DX.Common;
using Box2DX.Dynamics;
using PlatHak.Client.Common;
using PlatHak.Client.Common.Interfaces;
using PlatHak.Common.Maths;
using PlatHak.Common.Network;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;

namespace PlatHack.Game.Surfaces
{
    public abstract class GameSurface : Surface, IUpdatedSurface, IPacketReciverSurface, IInitializedSurface, IInputSurface
    {
        public World World { get; set; }
        public MyGame Game { get; set; }
        public SharpDxDebugDrawer DebugDrawer { get; set; }
        public List<IPhysicsSurface> PhysicsSurfaces { get; set; }
        
        protected GameSurface(RectangleF viewPort, MyGame game) : base(viewPort)
        {
            PhysicsSurfaces = new List<IPhysicsSurface>();
            Game = game;
            World = new World(new AABB
            {
               LowerBound = new Vec2(viewPort.X, viewPort.Y),
               UpperBound = new Vec2(viewPort.Right, viewPort.Bottom),
            }, new Vec2(0, 10f), true);
        }

        public virtual void OnUpdate(GameTime time)
        {
            

        }

        public override void Draw(RenderTarget target, GameTime time)
        {
            base.Draw(target, time);
            World.Step(1 / 15f, 8, 50);
            World.Validate();
            target.DrawRectangle(ViewPort.RawRectangleF, new SolidColorBrush(target, new RawColor4(255, 255, 255, 255)), 4);

        }

        public void AddIPhysicsSurface(IPhysicsSurface surface)
        {
            var item = surface as IDrawSurface;
            if (item != null) DrawSurfaces.Add(item);

            var init = surface as IInitializedSurface;
            init?.OnInitialize(Game.RenderTarget2D, Game.Factory2D, Game.FactoryDWrite);

            PhysicsSurfaces.Add(surface);
        }
        public abstract void OnPacketRecived(Packet packet);
        public abstract void OnInput(InputEventArgs args);

        public override void OnInitialize(RenderTarget target, Factory factory, SharpDX.DirectWrite.Factory factoryDr)
        {
            DebugDrawer = new SharpDxDebugDrawer(target, factory);
            World.SetDebugDraw(DebugDrawer);
            World.SetWarmStarting(true);
            World.SetContinuousPhysics(true);
            DebugDrawer.Flags = DebugDraw.DrawFlags.Shape | DebugDraw.DrawFlags.CoreShape;
            foreach (var physicsSurface in PhysicsSurfaces)
            {
                if (physicsSurface is IInitializedSurface)
                {
                    var init = physicsSurface as IInitializedSurface;
                    init?.OnInitialize(target, factory, factoryDr);
                }
            }

        }
    }
    public class SharpDxDebugDrawer : DebugDraw
    {
        public RenderTarget Target { get; set; }
        public Factory Factory { get; set; }
        public SolidColorBrush Brush { get; set; }
        public SharpDxDebugDrawer(RenderTarget target, Factory factory)
        {
            Target = target;
            Factory = factory;
            Brush = new SolidColorBrush(target, new RawColor4(255, 255, 255, 255));
        }
        public override void DrawPolygon(Vec2[] vertices, int vertexCount, Color color)
        {
            //var points = vertices.Where(x => x != Vec2.Zero).Select(x => x.GetVector2().RawVector2).ToArray();
            //for (int i = 0; i < points.Length - 1; i++)
            //{
            //    Target.DrawLine(points[i], points[i + 1], Brush);
            //}
        }

        public override void DrawSolidPolygon(Vec2[] vertices, int vertexCount, Color color)
        {
            var brush = new SolidColorBrush(Target, new RawColor4(color.R, color.G, color.B, 255));
            var points = vertices.Where(x => x != Vec2.Zero).Select(x => x.GetVector2().RawVector2).ToArray();
            
            for (int i = 0; i < points.Length - 1; i++)
            {
                Target.DrawLine(points[i], points[i + 1], brush);
            }
            Target.DrawLine(points[0], points[points.Length - 1], brush);
        }

        public override void DrawCircle(Vec2 center, float radius, Color color)
        {
            
        }

        public override void DrawSolidCircle(Vec2 center, float radius, Vec2 axis, Color color)
        {
            var brush = new SolidColorBrush(Target, new RawColor4(color.R, color.G, color.B, 255));
            Target.DrawEllipse(new Ellipse(center.GetVector2().RawVector2, radius, radius), brush);
        }

        public override void DrawSegment(Vec2 p1, Vec2 p2, Color color)
        {
            var brush = new SolidColorBrush(Target, new RawColor4(color.R, color.G, color.B, 255));
            Target.DrawLine(new RawVector2(p1.X, p1.Y), new RawVector2(p2.X, p2.Y), brush);
        }

        public override void DrawXForm(XForm xf)
        {
            
        }
    }
}