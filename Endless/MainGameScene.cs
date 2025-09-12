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

        public override void LoadContent(ContentManager Content)
        {
            base.LoadContent(Content);
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
        }


        public override void Draw(GameTime gameTime)
        {
            var sb = SceneManager.Instance.SpriteBatch;
            sb.Begin();
            sb.DrawString(Doto, "MAIN GAME AHHHHHHHHHH", new Vector2(0, 0), Color.Black);
            sb.End();
            base.Draw(gameTime);
        }


    }
}
