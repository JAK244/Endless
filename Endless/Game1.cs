using Endless.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Windows.Forms.VisualStyles;


namespace Endless
{
    /// <summary>
    /// the main game class
    /// </summary>
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        public ContentManager content;
        

        /// <summary>
        /// the game window
        /// </summary>
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            
        }

        /// <summary>
        /// Initializes the game components and sets up the initial state of the game.
        /// </summary>
        protected override void Initialize()
        {
            SceneManager.Instance.Initialize();
            SceneManager.Instance.Dimensions = new Vector2(1200,720); //800,480 original screen size/1200,720 preferred screen size
            graphics.PreferredBackBufferWidth = (int)SceneManager.Instance.Dimensions.X;
            graphics.PreferredBackBufferHeight = (int) SceneManager.Instance.Dimensions.Y;
            graphics.ApplyChanges();

            // TODO: Add your initialization logic here
           

            base.Initialize();
        }

        /// <summary>
        /// Loads the conten of the game
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            SceneManager.Instance.LoadContent(Content, GraphicsDevice);
            SceneManager.Instance.SpriteBatch = spriteBatch;

            
            // but nothing for bugs!


            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// updates the games movement/input
        /// </summary>
        /// <param name="gameTime">the game time</param>
        protected override void Update(GameTime gameTime)
        {
            SceneManager.Instance.Update(gameTime);


            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            
            base.Update(gameTime);
           
        }

        /// <summary>
        /// Draws the sprites using sprite batch
        /// </summary>
        /// <param name="gameTime">the game time</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);
            

            SceneManager.Instance.Draw(gameTime); // this calls TitleScene.Draw()

            base.Draw(gameTime);
        }
    }
}
