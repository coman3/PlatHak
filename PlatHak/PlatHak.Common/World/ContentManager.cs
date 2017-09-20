using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using PlatHak.Common.Drawing;

namespace PlatHak.Common.World
{
    public abstract class ContentManager
    {
        public List<Content> Content { get; set; }

        protected ContentManager()
        {
            Content = new List<Content>();
        }

        public abstract void LoadContent();
    }
    
    public abstract class Content
    {
        public Guid Id { get; set; }
    }
    
    public abstract class Content<T>
    {
        
        public T Data { get; set; }
    }

    public abstract class ImageContent : Content<Color[,]>
    {
        
    }
}