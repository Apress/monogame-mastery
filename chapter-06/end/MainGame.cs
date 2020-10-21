using chapter_06.Enum;
using chapter_06.States;
using chapter_06.States.Base;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace chapter_06
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class MainGame : Game
    {
        private BaseGameState _currentGameState;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private RenderTarget2D _renderTarget;
        private Rectangle _renderScaleRectangle;

        private const int DESIGNED_RESOLUTION_WIDTH = 1280;
        private const int DESIGNED_RESOLUTION_HEIGHT = 720;

        private const float DESIGNED_RESOLUTION_ASPECT_RATIO = DESIGNED_RESOLUTION_WIDTH / (float)DESIGNED_RESOLUTION_HEIGHT;

        public MainGame()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = DESIGNED_RESOLUTION_WIDTH,
                PreferredBackBufferHeight = DESIGNED_RESOLUTION_HEIGHT,
                IsFullScreen = false
            };

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            _renderTarget = new RenderTarget2D(graphics.GraphicsDevice, DESIGNED_RESOLUTION_WIDTH, DESIGNED_RESOLUTION_HEIGHT, false,
                SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.DiscardContents);

            _renderScaleRectangle = GetScaleRectangle();

            base.Initialize();
        }

        /// <summary>
        /// Uses the current window size compared to the design resolution
        /// </summary>
        /// <returns>Scaled Rectangle</returns>
        private Rectangle GetScaleRectangle()
        {
            var variance = 0.5;
            var actualAspectRatio = Window.ClientBounds.Width / (float)Window.ClientBounds.Height;

            Rectangle scaleRectangle;

            if (actualAspectRatio <= DESIGNED_RESOLUTION_ASPECT_RATIO)
            {
                var presentHeight = (int)(Window.ClientBounds.Width / DESIGNED_RESOLUTION_ASPECT_RATIO + variance);
                var barHeight = (Window.ClientBounds.Height - presentHeight) / 2;

                scaleRectangle = new Rectangle(0, barHeight, Window.ClientBounds.Width, presentHeight);
            }
            else
            {
                var presentWidth = (int)(Window.ClientBounds.Height * DESIGNED_RESOLUTION_ASPECT_RATIO + variance);
                var barWidth = (Window.ClientBounds.Width - presentWidth) / 2;

                scaleRectangle = new Rectangle(barWidth, 0, presentWidth, Window.ClientBounds.Height);
            }

            return scaleRectangle;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            SwitchGameState(new SplashState());
        }

        private void CurrentGameState_OnStateSwitched(object sender, BaseGameState e)
        {
            SwitchGameState(e);
        }

        private void SwitchGameState(BaseGameState gameState)
        {
            if (_currentGameState != null)
            {
                _currentGameState.OnStateSwitched -= CurrentGameState_OnStateSwitched;
                _currentGameState.OnEventNotification -= _currentGameState_OnEventNotification;
                _currentGameState.UnloadContent();
            }

            _currentGameState = gameState;

            _currentGameState.Initialize(Content, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);

            _currentGameState.LoadContent();

            _currentGameState.OnStateSwitched += CurrentGameState_OnStateSwitched;
            _currentGameState.OnEventNotification += _currentGameState_OnEventNotification;
        }

        private void _currentGameState_OnEventNotification(object sender, Enum.Events e)
        {
            switch (e)
            {
                case Events.GAME_QUIT:
                    Exit();
                    break;
            }
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            _currentGameState?.UnloadContent();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            _currentGameState.HandleInput(gameTime);
            _currentGameState.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // Render to the Render Target
            GraphicsDevice.SetRenderTarget(_renderTarget);

            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            _currentGameState.Render(spriteBatch);

            spriteBatch.End();

            // Now render the scaled content
            graphics.GraphicsDevice.SetRenderTarget(null);

            graphics.GraphicsDevice.Clear(ClearOptions.Target, Color.Black, 1.0f, 0);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);

            spriteBatch.Draw(_renderTarget, _renderScaleRectangle, Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}