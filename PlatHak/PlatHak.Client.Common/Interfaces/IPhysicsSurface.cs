using Box2DX.Collision;
using Box2DX.Dynamics;
using PlatHak.Common.Maths;

namespace PlatHak.Client.Common.Interfaces
{
    public interface IPhysicsSurface
    {
        Body Body { get; }
    }
}