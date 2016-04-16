using System;
using PlatHak.Common.Maths;

namespace PlatHak.Client.Common.Interfaces
{
    public interface IDragableSurface : ISurface
    {
        void OnDropItem(Vector2 posistion, DragItem item);
    }

    public interface IDragableSurfaceObjects : IDragableSurface
    {
        event DragableSurfaceObjectsDelegates.OnStartDrag OnStartDrag;
        event DragableSurfaceObjectsDelegates.OnDrop OnDrop;
        event DragableSurfaceObjectsDelegates.OnCancelDrag OnCancelDrag;
    }

    public static class DragableSurfaceObjectsDelegates
    {
        public delegate void OnStartDrag(object sender, EventArgs args);
        public delegate void OnDrop(object sender, DragItem item);
        public delegate void OnCancelDrag(object sender, EventArgs args);
    }
}