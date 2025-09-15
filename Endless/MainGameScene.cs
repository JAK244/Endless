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
    public class MainGameScene : GameScenes
    {
        
        private SpriteFont Doto;
        private TravelerSprite Traveler;
        private PortalSprite[] portals;
        private PowerBallSprite powerBall;
        private StarSprite[] stars;


        private float rotation;
        private ArrowSpriteTest arrow;
        //private Camera camera;

        public override void Initialize()
        {
            base.Initialize();
            Traveler = new TravelerSprite() { position = new Vector2(500, 420) };
            arrow = new ArrowSpriteTest();

            portals = new PortalSprite[]
            {
                new PortalSprite(){Position = new Vector2(700,350)},
                new PortalSprite(){Position = new Vector2(-25 ,350), PortalFlipped = true},
            };

            powerBall = new PowerBallSprite();

            stars = new StarSprite[]
            {
                new StarSprite(){Position = new Vector2(50,50)},
                new StarSprite(){Position = new Vector2(600,200) },
            };
            //camera = new(Vector2.Zero);

        }

        public override void LoadContent(ContentManager Content)
        {
            base.LoadContent(Content);
            Traveler.LoadContent(Content);
            foreach (var portal in portals) portal.LoadContent(Content);
            foreach (var star in stars) star.LoadContent(Content);
            powerBall.LoadContent(Content);
            arrow.LoadContent(Content);
            rotation = 0.0f;
            Doto = Content.Load<SpriteFont>("Doto-Black");
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (Keyboard.GetState().IsKeyDown(Keys.U))
            {
                SceneManager.Instance.AddScene(new TitleScene());
            }
            Traveler.Update(gameTime);
            arrow.Update(gameTime);
            

            //camera.Follow(Traveler.Bounds, new Vector2(800, 400));


        }


        public override void Draw(GameTime gameTime)
        {
            var sb = SceneManager.Instance.SpriteBatch;
            sb.Begin(samplerState: SamplerState.PointClamp);
            arrow.Draw(gameTime, sb);
            powerBall.Draw(gameTime, sb);
            foreach (var portal in portals)
                portal.Draw(gameTime, sb);
            foreach (var star in stars)
                star.Draw(gameTime, sb);
            sb.DrawString(Doto, "MAIN GAME AHHHHHHHHHH", new Vector2(0, 0), Color.Black);
            Traveler.Draw(gameTime, sb);
            sb.End();
            base.Draw(gameTime);
        }


    }
}
