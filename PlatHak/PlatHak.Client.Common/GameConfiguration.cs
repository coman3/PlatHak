namespace PlatHak.Client.Common
{
    public class GameConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameConfiguration"/> class.
        /// </summary>
        public GameConfiguration() : this("PlatHak") { }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameConfiguration"/> class.
        /// </summary>
        public GameConfiguration(string title) : this(title, 800, 600, false) { }

        public GameConfiguration(string title, int width, int height) : this(title, width, height, false) { }

        public GameConfiguration(string title, int width, int height, bool maximized)
        {
            Title = title;
            Width = width;
            Height = height;
            Maximized = maximized;
            WaitVerticalBlanking = false;
            Resizeable = false;
            DisableResizeButtons = true;
        }

        /// <summary>
        /// Gets or sets the window title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the width of the window.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the window.
        /// </summary>
        public int Height { get; set; }

        public bool Maximized { get; set; }
        public bool Resizeable { get; set; }
        public bool DisableResizeButtons { get; set; }
        public bool IsFullscreen { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [wait vertical blanking].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [wait vertical blanking]; otherwise, <c>false</c>.
        /// </value>
        public bool WaitVerticalBlanking { get; set; }
    }
}