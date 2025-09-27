using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Endless.Managers;
using Endless.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Endless.Screens
{
    /// <summary>
    /// the main game screen class
    /// </summary>
    public class MainGameScene : GameScenes
    {
        
        private SpriteFont Doto;
        private TravelerSprite Traveler;
        private HelthSprite[] healths;
        private PortalSprite[] portals;
        private PowerBallSprite powerBall;
        private StarSprite[] stars;
        private Bug1Sprite[] bugs;
        private ArmSprite arm;
        private int healthLeft;

        private double damageCooldown = 0;


        private Texture2D ball;
        private Texture2D backGroundImage;


        private float rotation;
        
        //private Camera camera;

        /// <summary>
        /// initializes the content for the scene
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            

            Traveler = new TravelerSprite() { position = new Vector2(500, 420) };
            
            arm = new ArmSprite() { position = new Vector2(500, 420) };

            portals = new PortalSprite[]
            {
                //new PortalSprite(){Position = new Vector2(700,350)},
                //new PortalSprite(){Position = new Vector2(-25 ,350), PortalFlipped = true},
            };

            powerBall = new PowerBallSprite() { Position = new Vector2(330, 200) };

            stars = new StarSprite[]
            {
                new StarSprite(){Position = new Vector2(50,50)},
                new StarSprite(){Position = new Vector2(600,200) },
            };

            bugs = new Bug1Sprite[]
            {
                
                //new Bug1Sprite(new Vector2(630,352)){CanMove = true},
                    //new Bug1Sprite(){Position = new Vector2(550,352), CanMove = false},
                    //new Bug1Sprite(){Position = new Vector2(30,352), BugFlipped = true , CanMove = false},
                //new Bug1Sprite(new Vector2(150,352)){BugFlipped = true , CanMove = true},
            };

            healths = new HelthSprite[]
            {
                new HelthSprite(){position = new Vector2(120,0)},
                new HelthSprite(){position = new Vector2(60,0)},
                new HelthSprite(){position = new Vector2(0,0)},
            };
            healthLeft = healths.Length;
            //camera = new(Vector2.Zero);

        }

        /// <summary>
        /// loads the content for the scene using a content manager
        /// </summary>
        /// <param name="Content">the content manager</param>
        public override void LoadContent(ContentManager Content)
        {
            base.LoadContent(Content);
            backGroundImage = Content.Load<Texture2D>("ForestBG");
            ball = Content.Load<Texture2D>("Ball");
            Traveler.LoadContent(Content);
            arm.LoadContent(Content);
            foreach (var portal in portals) portal.LoadContent(Content);
            foreach (var bug in bugs) bug.LoadContent(Content);
            foreach (var star in stars) star.LoadContent(Content);
            foreach (var helth in healths) helth.LoadContent(Content);
            powerBall.LoadContent(Content);
            
            rotation = 0.0f;
            Doto = Content.Load<SpriteFont>("Doto-Black");
        }

        /// <summary>
        /// unloads the content from the scene
        /// </summary>
        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        /// <summary>
        /// updates the content using game time
        /// </summary>
        /// <param name="gameTime">the game time</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (Keyboard.GetState().IsKeyDown(Keys.U))
            {
                SceneManager.Instance.AddScene(new TitleScene());
            }

            Traveler.color = Color.White;



            Traveler.Update(gameTime);
            
            arm.Update(gameTime);

            damageCooldown -= gameTime.ElapsedGameTime.TotalSeconds;

            foreach (var bug in bugs)
            {
                bug.Update(gameTime);

                if (damageCooldown <= 0 && bug.Bounds.CollidesWith(Traveler.Bounds))
                {
                    for (int i = 0; i < healths.Length; i++)
                    {
                        if (!healths[i].Damaged)
                        {
                            Traveler.color = Color.Red;
                            healths[i].Damaged = true;
                            healthLeft--;
                            damageCooldown = 1.0; 
                            break;
                        }
                    }
                }
            }

            //camera.Follow(Traveler.Bounds, new Vector2(800, 400));

        }


        /// <summary>
        /// draws the content on screen
        /// </summary>
        /// <param name="gameTime">the game time</param>
        public override void Draw(GameTime gameTime)
        {
            var sb = SceneManager.Instance.SpriteBatch;
            sb.Begin(samplerState: SamplerState.PointClamp);

            sb.Draw(backGroundImage,new Rectangle(0,0, sb.GraphicsDevice.Viewport.Width, sb.GraphicsDevice.Viewport.Height),Color.White);

            powerBall.Draw(gameTime, sb);

            foreach (var portal in portals)
                portal.Draw(gameTime, sb);
            foreach (var star in stars)
                star.Draw(gameTime, sb);
            foreach (var helth in healths) 
                helth.Draw(gameTime, sb);
            foreach (var bug in bugs)
            {
                bug.Draw(gameTime, sb);
                //var rec = new Rectangle((int)(bug.Bounds.Center.X - bug.Bounds.Radius), (int)(bug.Bounds.Center.Y - bug.Bounds.Radius), (int)bug.Bounds.Radius * 2, (int)bug.Bounds.Radius * 2);

                //sb.Draw(ball, rec, Color.White);
            }

            //var rec2 = new Rectangle((int)Traveler.Bounds.X,(int)Traveler.Bounds.Y,(int)Traveler.Bounds.Width,(int)Traveler.Bounds.Height);

            //sb.Draw(ball, rec2, Color.White);

            arm.Draw(gameTime, sb);
            Traveler.Draw(gameTime, sb);
            sb.End();
            base.Draw(gameTime);

        }
        
    }
}
