using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Hnefatafln
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameEngine : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Screen CurrentScreen { get;  set; }
        MainMenu MainMenu { get; set; }
        GameScreen GameScreen { get; set; }
        NetGame OnlineScreen { get; set; }
        CreditsScreen CreditsScreen { get; set; }
        GameState GameState { get; set; }

        Texture2D background;
        Rectangle mainFrame = new Rectangle(0, 0, Consts.ScreenWidth, Consts.ScreenHeight);

        public GameEngine()
        {
            graphics = new GraphicsDeviceManager(this);
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
            graphics.PreferredBackBufferHeight = Consts.ScreenHeight;
            graphics.PreferredBackBufferWidth = Consts.ScreenWidth;
            this.IsMouseVisible = true;
            graphics.ApplyChanges();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            MainMenu = new MainMenu(spriteBatch);
            CurrentScreen = MainMenu;
            GameScreen = new GameScreen(spriteBatch);
            CreditsScreen = new CreditsScreen(spriteBatch);
            OnlineScreen = new NetGame(spriteBatch);
            MainMenu.LoadContent(Content);
            GameScreen.LoadContent(Content);
            CreditsScreen.LoadContent(Content);
            OnlineScreen.LoadContent(Content);
            background = Content.Load<Texture2D>("Board");

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            CurrentScreen.Update(gameTime);

            if (CurrentScreen.SwitchState)
            {
                SwitchState();
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin();
            spriteBatch.Draw(background, mainFrame, Color.White);
            CurrentScreen.Draw(gameTime);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Switches current game state using information from CurrentState and MainMenu
        /// </summary>
        private void SwitchState()
        {
            if(GameState != GameState.MainMenu)
            {
                GameState = GameState.MainMenu;
                CurrentScreen = MainMenu;
            }
            else
            {
                GameState = MainMenu.NextState;
                switch(MainMenu.NextState)
                {
                    case GameState.Gameplay:
                        CurrentScreen = GameScreen;
                        GameScreen.StartGame();
                        break;
                    case GameState.Online:
                        CurrentScreen = OnlineScreen;
                        OnlineScreen.StartGame();
                        break;
                    case GameState.Credits:
                        CurrentScreen = CreditsScreen;
                        break;
                    case GameState.Exit:
                        Exit();
                        break;
                }
            }
        }
    }
}
