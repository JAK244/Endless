using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Endless
{
    public class TitleScene : GameScenes
    {
        private GraphicsDeviceManager graphics;
        private TravelerSprite traveler;
        private PortalSprite[] portals;
        private Bug1Sprite[] bugs;
        private SpriteFont Doto;
        private PowerBallSprite powerBall;
        private StarSprite[] stars;


        public override void Initialize()
        {
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
        }

        public override void LoadContent(ContentManager Content)
        {
           
            Doto = Content.Load<SpriteFont>("Doto-Black");
            traveler.LoadContent(Content);
            foreach (var portal in portals) portal.LoadContent(Content);
            foreach (var bug in bugs) bug.LoadContent(Content);
            foreach (var star in stars) star.LoadContent(Content);
            powerBall.LoadContent(Content);
            
            base.LoadContent(Content);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            
            
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                SceneManager.Instance.AddScene(new MainGameScene());
            }
            

            // TODO: Add your update logic here
            
            

            foreach (var bug in bugs) bug.Update(gameTime);
            

        }

        public override void Draw(GameTime gameTime)
        {
            var sb = SceneManager.Instance.SpriteBatch;

            sb.Begin();
            traveler.Draw(gameTime, sb);
            foreach (var portal in portals)
                portal.Draw(gameTime, sb);
            foreach (var bug in bugs)
                bug.Draw(gameTime, sb);
            foreach (var star in stars)
                star.Draw(gameTime, sb);
            sb.DrawString(Doto, $"Void", new Vector2(300, 100), Color.Black);
            sb.DrawString(Doto, $"Traveler", new Vector2(200, 180), Color.Black);
            sb.DrawString(Doto, $"Press Enter to start", new Vector2(250, 300), Color.Gold,0f, Vector2.Zero, 0.3f, SpriteEffects.None, 0);
            sb.DrawString(Doto, $"Press ESC to EXIT", new Vector2(520, 0), Color.Gold, 0f, Vector2.Zero, 0.3f, SpriteEffects.None, 0);
            powerBall.Draw(gameTime, sb);
            sb.End();
        }
    }
}
