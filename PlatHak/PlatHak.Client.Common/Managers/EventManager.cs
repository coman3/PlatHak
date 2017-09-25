using System;
using System.Collections.Generic;
using System.Linq;
using PlatHak.Client.Common.EventArgs;
using PlatHak.Client.Common.Interfaces;

namespace PlatHak.Client.Common.Managers
{
    public class EventManager
    {
        public SortedSet<IDraw> Drawables { get; set; }
        public SortedSet<IUpdate> Updateables { get; set; }
        public List<ILoad> Loadables { get; set; }
        public List<IElementContainer> ElementContainers { get; set; }

        private readonly Game _game;
        private bool _isLoaded = false;

        public EventManager(Game game)
        {
            Drawables = new SortedSet<IDraw>(new ByDrawOrder());
            Updateables = new SortedSet<IUpdate>(new ByUpdateOrder());
            Loadables = new List<ILoad>();
            ElementContainers = new List<IElementContainer>();
            _game = game;
            _game.OnUpdated += OnUpdated;
            _game.OnLoad += OnLoad;
            _game.OnDraw += OnDraw;
        }

        private void OnDraw(object sender, DrawEventArgs e)
        {
            foreach (var draw in Drawables)
            {
                draw.Draw(e.GameTime, e.SpriteBatch);
            }
        }
        private void OnLoad(object sender, LoadEventArgs e)
        {
            _isLoaded = true;
            var currentLoadables = Loadables.Where(x=> !x.Loaded).ToArray();
            foreach (var load in currentLoadables)
            {
                load.LoadContent(e.GraphicsDevice, e.ContentManager);
                load.Loaded = true;
            }
            if(Loadables.Any(x=> !x.Loaded))
                OnLoad(this, e);
        }
        private void OnUpdated(object sender, UpdateEventArgs e)
        {
            foreach (var update in Updateables)
            {
                update.Update(e.GameTime);
            }
        }


        public void AddElement(object element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            if (element is IDraw draw)
            {
                Drawables.Add(draw);
            }

            if (element is IUpdate update)
            {
                Updateables.Add(update);
            }

            if (element is ILoad load)
            {
                Loadables.Add(load);
                if (_isLoaded)
                    load.LoadContent(_game.GraphicsDevice, _game.Content);
            }
            if (element is IElementContainer container)
            {
                ElementContainers.Add(container);
                container.CreateElement += ContainerOnCreateElement;
            }
        }

        private void ContainerOnCreateElement(object sender, ElementCreateEventArgs elementCreateEventArgs)
        {
            AddElement(elementCreateEventArgs.Element);
        }

        #region IComparers

        private class ByDrawOrder : IComparer<IDraw>
        {
            public int Compare(IDraw x, IDraw y)
            {
                return x.DrawOrder.CompareTo(y.DrawOrder);
            }
        }
        private class ByUpdateOrder : IComparer<IUpdate>
        {
            public int Compare(IUpdate x, IUpdate y)
            {
                return x.UpdateOrder.CompareTo(y.UpdateOrder);
            }
        }

        #endregion
    }
}