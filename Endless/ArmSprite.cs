using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Endless.Collisions;


namespace Endless
{
    public class ArmSprite
    {
        private KeyboardState keyboardState;

        private GamePadState gamePadState;

        private Texture2D texture;

        public Vector2 position;

        private float rotation;

        public void LoadContent(ContentManager content)
        {
            rotation = 0.0f;
            texture = content.Load<Texture2D>("TravelerArm");
        }

        public void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();
            gamePadState = GamePad.GetState(0);

            if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
            {
                position += new Vector2(-3, 0);
            }

            if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
            {
                position += new Vector2(3, 0);
          
            }

            position += gamePadState.ThumbSticks.Left * new Vector2(3, 0);

            Vector2 mousePosition = Mouse.GetState().Position.ToVector2();

            Vector2 distance = mousePosition - position;

            rotation = (float)Math.Atan2(distance.Y, distance.X);

            

        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(texture, position, null, Color.White, rotation, new Vector2(texture.Width / 2f, texture.Height / 2f), 2, SpriteEffects.None, 0);

        }
    }
}
