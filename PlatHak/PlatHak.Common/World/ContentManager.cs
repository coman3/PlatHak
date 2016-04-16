using System;
using System.Collections.Generic;

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
    [Serializable]
    public abstract class Content
    {
        public Guid Id { get; set; }
    }
    [Serializable]
    public abstract class Content<T>
    {
        
        public T Data { get; set; }
    }

    public abstract class ImageContent : Content<Color[,]>
    {
        
    }
}