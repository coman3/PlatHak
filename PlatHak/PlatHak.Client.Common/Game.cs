using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpDX.Windows;


namespace PlatHak.Client.Common
{
    public abstract class Game
    {
        private readonly GameTime _clock = new GameTime();
        private FormWindowState _currentFormWindowState;
        private bool _disposed;
        private bool _closing;
        protected RenderForm Form { get; private set; }
        private float _frameAccumulator;
        private int _frameCount;
        private GameConfiguration _gameConfiguration;
        public static event OnGameCloseHandler OnGameClosed;
        public static event OnGameCloseHandler OnGameClosing;
        public delegate void OnGameCloseHandler();

        /// <summary>
        ///   Performs object finalization.
        /// </summary>
        ~Game()
        {
            if (!_disposed)
            {
                Dispose(false);
                _disposed = true;
            }
        }

        /// <summary>
        ///   Disposes of object resources.
        /// </summary>
        public void Dispose()
        {
            if (!_disposed)
            {
                Dispose(true);
                _disposed = true;
            }
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///   Disposes of object resources.
        /// </summary>
        /// <param name = "disposeManagedResources">If true, managed resources should be
        ///   disposed of in addition to unmanaged resources.</param>
        protected virtual void Dispose(bool disposeManagedResources)
        {
            if (disposeManagedResources)
            {
                Form?.Dispose();
            }
        }

        public void Close()
        {
            _closing = true;

        }
        /// <summary>
        /// Return the Handle to display to.
        /// </summary>
        protected IntPtr DisplayHandle
        {
            get { return Form.Handle; }
        }

        /// <summary>
        /// Gets the config.
        /// </summary>
        /// <value>The config.</value>
        public GameConfiguration Config
        {
            get { return _gameConfiguration; }
        }

        /// <summary>
        ///   Gets the number of seconds passed since the last frame.
        /// </summary>
        public float FrameDelta { get; private set; }

        /// <summary>
        ///   Gets the number of seconds passed since the last frame.
        /// </summary>
        public float FramePerSecond { get; private set; }

        /// <summary>
        /// Create Form for this game.
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        protected virtual RenderForm CreateForm(GameConfiguration config)
        {
            return new RenderForm(config.Title)
            {
                ClientSize = new System.Drawing.Size(config.Width, config.Height),
                WindowState = config.Maximized ? FormWindowState.Maximized : FormWindowState.Normal,
                FormBorderStyle = config.Resizeable ? FormBorderStyle.Sizable : FormBorderStyle.Fixed3D,
                MaximizeBox = !config.DisableResizeButtons,
                MinimizeBox = !config.DisableResizeButtons
            };
        }

        /// <summary>
        /// Runs the game with default presentation
        /// </summary>
        public void Run()
        {
            Run(new GameConfiguration());
        }

        /// <summary>
        /// Runs the game.
        /// </summary>
        public void Run(GameConfiguration gameConfiguration)
        {
            _gameConfiguration = gameConfiguration ?? new GameConfiguration();
            Form = CreateForm(_gameConfiguration);
            Form.Closed += (_,args) => OnGameClosed?.Invoke();
            Form.Closing += (_, args) => OnGameClosing?.Invoke();
            Initialize(_gameConfiguration);

            bool isFormClosed = false;
            bool formIsResizing = false;

            Form.MouseClick += HandleMouseClick;
            Form.KeyDown += HandleKeyDown;
            Form.KeyUp += HandleKeyUp;
            Form.Resize += (o, args) =>
            {

                if (Form.WindowState != _currentFormWindowState)
                {
                    HandleResize(o, args);
                }

                _currentFormWindowState = Form.WindowState;
            };

            Form.ResizeBegin += (o, args) => { formIsResizing = true; };
            Form.ResizeEnd += (o, args) =>
            {
                formIsResizing = false;
                HandleResize(o, args);
            };

            Form.Closed += (o, args) => { isFormClosed = true; };

            LoadContent();

            _clock.Start();
            BeginRun();
            RenderLoop.Run(Form, () =>
            {
                if (_closing) Dispose();
                if (isFormClosed)
                {
                    return;
                }

                OnUpdate();
                if (!formIsResizing)
                    Render();
            });

            UnloadContent();
            EndRun();

            // Dispose explicitly
            Dispose();
        }

        /// <summary>
        ///   In a derived class, implements logic to initialize the game.
        /// </summary>
        protected abstract void Initialize(GameConfiguration gameConfiguration);

        protected virtual void LoadContent()
        {
        }

        protected virtual void UnloadContent()
        {
        }

        /// <summary>
        ///   In a derived class, implements logic to update any relevant game state.
        /// </summary>
        protected virtual void Update(GameTime time)
        {
        }

        /// <summary>
        ///   In a derived class, implements logic to render the game.
        /// </summary>
        protected virtual void Draw(GameTime time)
        {
        }

        protected virtual void BeginRun()
        {
        }

        protected virtual void EndRun()
        {
        }

        /// <summary>
        ///   In a derived class, implements logic that should occur before all
        ///   other rendering.
        /// </summary>
        protected virtual void BeginDraw()
        {
        }

        /// <summary>
        ///   In a derived class, implements logic that should occur after all
        ///   other rendering.
        /// </summary>
        protected virtual void EndDraw()
        {
        }

        /// <summary>
        ///   Quits the game.
        /// </summary>
        public void Exit()
        {
            Form.Close();
        }

        /// <summary>
        ///   Updates game state.
        /// </summary>
        private void OnUpdate()
        {
            FrameDelta = (float) _clock.Update();
            Update(_clock);
        }

        protected System.Drawing.Size RenderingSize
        {
            get { return Form.ClientSize; }
        }

        /// <summary>
        ///   Renders the game.
        /// </summary>
        private void Render()
        {
            _frameAccumulator += FrameDelta;
            ++_frameCount;
            if (_frameAccumulator >= 1.0f)
            {
                FramePerSecond = _frameCount/_frameAccumulator;

                Form.Text = _gameConfiguration.Title + " - FPS: " + FramePerSecond;
                _frameAccumulator = 0.0f;
                _frameCount = 0;
            }
            BeginDraw();
            Draw(_clock);
            EndDraw();
        }

        protected virtual void MouseClick(MouseEventArgs e)
        {
        }

        protected virtual void KeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Exit();
        }

        protected virtual void KeyUp(KeyEventArgs e)
        {
        }

        /// <summary>
        ///   Handles a mouse click event.
        /// </summary>
        /// <param name = "sender">The sender.</param>
        /// <param name = "e">The <see cref = "System.Windows.Forms.MouseEventArgs" /> instance containing the event data.</param>
        private void HandleMouseClick(object sender, MouseEventArgs e)
        {
            MouseClick(e);
        }

        /// <summary>
        ///   Handles a key down event.
        /// </summary>
        /// <param name = "sender">The sender.</param>
        /// <param name = "e">The <see cref = "System.Windows.Forms.KeyEventArgs" /> instance containing the event data.</param>
        private void HandleKeyDown(object sender, KeyEventArgs e)
        {
            KeyDown(e);
        }

        /// <summary>
        ///   Handles a key up event.
        /// </summary>
        /// <param name = "sender">The sender.</param>
        /// <param name = "e">The <see cref = "System.Windows.Forms.KeyEventArgs" /> instance containing the event data.</param>
        private void HandleKeyUp(object sender, KeyEventArgs e)
        {
            KeyUp(e);
        }

        private void HandleResize(object sender, EventArgs e)
        {
            if (Form.WindowState == FormWindowState.Minimized)
            {
                return;
            }
        }
    }
}

