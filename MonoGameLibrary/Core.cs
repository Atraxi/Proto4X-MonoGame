using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoGameLibrary
{
    public class Core : Game
    {
        internal static Core s_instance;

        /// <summary>
        /// Gets a reference to the Core instance.
        /// </summary>
        public static Core Instance => s_instance;

        /// <summary>
        /// Gets the graphics device manager to control the presentation of graphics.
        /// </summary>
        protected static GraphicsDeviceManager Graphics { get; private set; }

        /// <summary>
        /// Gets the sprite batch used for all 2D rendering.
        /// </summary>
        protected SpriteBatch SpriteBatch { get; private set; }

        /// <summary>
        /// Creates a new Core instance.
        /// </summary>
        /// <param name="title">The title to display in the title bar of the game window.</param>
        /// <param name="width">The initial width, in pixels, of the game window.</param>
        /// <param name="height">The initial height, in pixels, of the game window.</param>
        /// <param name="fullScreen">Indicates if the game should start in fullscreen mode.</param>
        public Core(string title, int width, int height, bool fullScreen)
        {
            // Ensure that multiple cores are not created.
            if (s_instance != null)
            {
                throw new InvalidOperationException($"Only a single Core instance can be created");
            }

            // Store reference to engine for global member access.
            s_instance = this;

            // Create a new graphics device manager.
            Graphics = new GraphicsDeviceManager(this)
            {
                // Set the graphics defaults.
                PreferredBackBufferWidth = width,
                PreferredBackBufferHeight = height,
                IsFullScreen = fullScreen,
            };

            // Apply the graphic presentation changes.
            Graphics.ApplyChanges();

            // Set the window title.
            Window.Title = title;
            
            // Set the root directory for content.
            Content.RootDirectory = "Content";

            // Mouse is visible by default.
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // Create the sprite batch instance.
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            base.Initialize();
        }
    }
}
