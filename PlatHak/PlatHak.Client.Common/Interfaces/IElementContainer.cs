using System;

namespace PlatHak.Client.Common.Interfaces
{
    public interface IElementContainer
    {
        event EventHandler<ElementCreateEventArgs> CreateElement;
    }

    public class ElementCreateEventArgs : System.EventArgs
    {
        public object Element { get; set; }

        public ElementCreateEventArgs(object element)
        {
            Element = element;
        }
    }
}