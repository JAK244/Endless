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
        private float rotation;
        private ArrowSpriteTest arrow;
        //private Camera camera;

        public override void Initialize()
        {
            base.Initialize();
            Traveler = new TravelerSprite();
            arrow = new ArrowSpriteTest();
            //camera = new(Vector2.Zero);
            
        }

        public override void LoadContent(ContentManager Content)
        {
            base.LoadContent(Content);
            Traveler.LoadContent(Content);
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
            Traveler.Draw(gameTime, sb);
            arrow.Draw(gameTime, sb);
            sb.DrawString(Doto, "MAIN GAME AHHHHHHHHHH", new Vector2(0, 0), Color.Black);
            sb.End();
            base.Draw(gameTime);
        }


    }
}
