using System.Collections.Generic;
using Box2DX.Collision;
using Box2DX.Common;
using PlatHack.Game.Surfaces.Dragables;
using PlatHak.Client.Common.Interfaces;
using PlatHak.Client.Common.Objects;
using PlatHak.Common.Maths;
using PlatHak.Common.Network;
using Color = PlatHak.Common.Color;
using Math = System.Math;

namespace PlatHack.Game.Surfaces.Games
{
    public class Game : GameSurface, IDragableSurface
    {
        public Game(RectangleF viewPort, MyGame game) : base(viewPort, game)
        {
            AddIPhysicsSurface(new CirclePhysicsObject(World, new CirclePhysicsObjectDef
            {
                Density = 100,
                Radius = 30,
                Posistion = new Vector2(500, 20),
                ShapeDebugColor = new Color(0, 0, 255, 255),
                AllowSleep = true,
                Friction = 1,
            }));
            AddIPhysicsSurface(new BoxPhysicsObject(World, new BoxPhysicsObjectDef
            {
                Width = ViewPort.Width - 20,
                Height = 30,
                Posistion = new Vector2(viewPort.Center.X, viewPort.Bottom - 40),
                Density = 0,
                MassData = new MassData { Center = Vec2.Zero, I = 0, Mass = 0},
                FixedRotation = true,
                ShapeDebugColor = new Color(255, 255, 255, 255)

            }));
            AddIPhysicsSurface(new BoxPhysicsObject(World, new BoxPhysicsObjectDef
            {
                Width = 30,
                Height = ViewPort.Height,
                Posistion = new Vector2(ViewPort.Left + 40, ViewPort.Center.Y),
                Density = 0,
                MassData = new MassData { Center = Vec2.Zero, I = 0, Mass = 0 },
                FixedRotation = true,
                ShapeDebugColor = new Color(255, 255, 255, 255)

            }));
            AddIPhysicsSurface(new PolygonPhysicsObject(World, new PolygonPhysicsObjectDef
            {
                Vertices = new List<Vector2>
                {
                    new Vector2(-20, 0),
                    new Vector2(20, 0),
                    new Vector2(0, 15f)
                },
                Density = 1,
                Posistion = new Vector2(ViewPort.Left + 100, ViewPort.Top + 10),
                AllowSleep = true,
                Restitution = 0.3f,
                Friction = 100,
            }));
            AddIPhysicsSurface(new BoxPhysicsObject(World, new BoxPhysicsObjectDef
            {
                Width = 30,
                Height = 500,
                Posistion = new Vector2(ViewPort.Right - 100, ViewPort.Bottom - 100),
                Density = 0,
                Angle = 45,
                MassData = new MassData { Center = Vec2.Zero, I = 0, Mass = 0 },
                FixedRotation = true,
                ShapeDebugColor = new Color(255, 255, 255, 255)

            }));

            MouseWheelValue = 20;
        }

        public override void OnPacketRecived(Packet packet)
        {
            
        }

        public Vector2 LastMousePos { get; set; }
        public float MouseWheelValue { get; set; }
        public bool Static { get; set; }
        public override void OnInput(InputEventArgs args)
        {

            if (args.InputType == InputType.Mouse)
            {
                if (args.ValueType == InputValue.ScrollWheelValue)
                {
                    MouseWheelValue += args.ValueX/10f;
                    MouseWheelValue = Math.Max(5, Math.Min(MouseWheelValue, 200));
                    return;
                }
                if (args.ValueType == InputValue.MouseMove && args.ValueY.HasValue)
                {
                    var point = new Vector2(args.ValueX, args.ValueY.Value);
                    if (ViewPort.Contains(point))
                        LastMousePos = point;
                    return;
                }
                if (ViewPort.Contains(LastMousePos))
                {
                    if (args.ValueType == InputValue.ScrollWheel) Static = args.ValueX > 0;
                    if (args.ValueType == InputValue.LeftMouse && args.ValueX > 0)
                        AddIPhysicsSurface(new BoxPhysicsObject(World, new BoxPhysicsObjectDef
                        {
                            Width = MouseWheelValue,
                            Height = MouseWheelValue,
                            Density = 1,
                            FixedObject = Static,
                            Restitution = 0.3f,
                            ShapeDebugColor = new Color(0, 255, 255, 255),
                            AllowSleep = true,
                            Posistion = LastMousePos
                        }));
                    if (args.ValueType == InputValue.RightMouse && args.ValueX > 0)
                        AddIPhysicsSurface(new CirclePhysicsObject(World, new CirclePhysicsObjectDef
                        {
                            Density = 1,
                            FixedObject = Static,
                            Radius = MouseWheelValue,
                            Posistion = LastMousePos,
                            ShapeDebugColor = new Color(0, 0, 255, 255),
                            AllowSleep = true,
                            Friction = 0.1f,
                            LinearDamping = 0,
                            Restitution = 0.3f
                        }));
                }
            }
        }

        public void OnDropItem(Vector2 posistion, DragItem item)
        {
            var circle = item as CircleDragItem;
            if (circle != null)
            {
                AddIPhysicsSurface(new CirclePhysicsObject(World, circle.ObjectDef));
            }
        }
    }
}