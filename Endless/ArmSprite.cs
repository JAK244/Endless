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

        private bool flipped;

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

            // Move with left stick
            position += gamePadState.ThumbSticks.Left * new Vector2(3, 0);

            // Default: look at mouse
            Vector2 targetDirection = Mouse.GetState().Position.ToVector2() - position;

            // If right stick is being used, override with its direction
            Vector2 rightStick = gamePadState.ThumbSticks.Right;
            rightStick.Y *= -1; 

            if (rightStick.Length() > 0.2f) // deadzone check
            {
                targetDirection = rightStick;
            }

            // Rotation from direction
            rotation = (float)Math.Atan2(targetDirection.Y, targetDirection.X);

            // Handle flipping
            if (targetDirection.X < 0)
            {
                flipped = true;
                rotation += MathF.PI;
            }
            else
            {
                flipped = false;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            SpriteEffects spriteEffect = (flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
            spriteBatch.Draw(texture, position, null, Color.White, rotation, new Vector2(texture.Width / 2f, texture.Height / 2f), 2, spriteEffect, 0);

        }
    }
}
