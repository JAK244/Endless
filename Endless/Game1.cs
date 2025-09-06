using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct3D9;

namespace Endless
{
    /// <summary>
    /// the main game class
    /// </summary>
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private TravelerSprite traveler;
        private PortalSprite[] portals;
        private Bug1Sprite[] bugs;
        private SpriteFont Doto;
        private PowerBallSprite powerBall;
        private StarSprite[] stars;

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
            // TODO: Add your initialization logic here
            traveler = new TravelerSprite();
            portals = new PortalSprite[]
            {
                new PortalSprite(){Position = new Vector2(700,355)},
                new PortalSprite(){Position = new Vector2(-25 ,355), PortalFlipped = true},
            };

            bugs = new Bug1Sprite[]
            {
                new Bug1Sprite(){Position = new Vector2(630,353)},
                new Bug1Sprite(){Position = new Vector2(550,353)},
                new Bug1Sprite(){Position = new Vector2(30,353), BugFlipped = true},
                new Bug1Sprite(){Position = new Vector2(150,353), BugFlipped = true},
            };
            powerBall = new PowerBallSprite();

            stars = new StarSprite[]
            {
                new StarSprite(){Position = new Vector2(50,50)},
                new StarSprite(){Position = new Vector2(600,200) },
            };

            base.Initialize();
        }

        /// <summary>
        /// Loads the conten of the game
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            traveler.LoadContent(Content);
            foreach (var portal in portals) portal.LoadContent(Content);
            foreach (var bug in bugs) bug.LoadContent(Content);
            foreach (var star in stars) star.LoadContent(Content);
            Doto = Content.Load<SpriteFont>("Doto-Black");
            powerBall.LoadContent(Content);
            // but nothing for bugs!


            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// updates the games movement/input
        /// </summary>
        /// <param name="gameTime">the game time</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            traveler.Update(gameTime);

            foreach(var bug in  bugs) bug.Update(gameTime);
           
            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the sprites using sprite batch
        /// </summary>
        /// <param name="gameTime">the game time</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            traveler.Draw(gameTime, spriteBatch);
            foreach (var portal in portals)
            {
                portal.Draw(gameTime, spriteBatch);
            }
            foreach (var bug in bugs)
            {
                bug.Draw(gameTime, spriteBatch);
            }
            foreach(var star in stars) star.Draw(gameTime, spriteBatch);
            spriteBatch.DrawString(Doto, $"Void", new Vector2(300, 100), Color.Black);
            spriteBatch.DrawString(Doto, $"Traveler", new Vector2(200, 180), Color.Black);
            spriteBatch.DrawString(Doto, $"Press ESC to EXIT", new Vector2(520, 0), Color.Gold,0f,new Vector2(0,0),0.3f,SpriteEffects.None,0);
            powerBall.Draw(gameTime, spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
