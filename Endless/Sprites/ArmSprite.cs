using Endless.Collisions;
using Endless.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Endless.Sprites
{
    /// <summary>
    /// the arm sprite class
    /// </summary>
    public class ArmSprite
    {
        private KeyboardState keyboardState;

        private GamePadState gamePadState;

        private MouseState mouseState;
        private MouseState currentMouse;
        private MouseState previousMouse;

        private Texture2D bulletTexture;

        public List<BulletSprite> Bullets = new List<BulletSprite>();

        private Texture2D texture;

        /// <summary>
        /// the arms position
        /// </summary>
        public Vector2 position;

        private float rotation;

        private bool flipped;

        /// <summary>
        /// loads the arm texture using content manager
        /// </summary>
        /// <param name="content">the given content manager</param>
        public void LoadContent(ContentManager content)
        {
            rotation = 0.0f;
            texture = content.Load<Texture2D>("TravelerArm");
            bulletTexture = content.Load<Texture2D>("Bullet");
        }

        /// <summary>
        /// updates the arms movements
        /// </summary>
        /// <param name="gameTime">the game time</param>
        public void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();
            gamePadState = GamePad.GetState(0);
            previousMouse = currentMouse;
            currentMouse = Mouse.GetState();

            if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
            {
                position += new Vector2(-3, 0);
            }

            if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
            {
                position += new Vector2(3, 0);
            }

            if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W))
            {
                position += new Vector2(0, -3);
                
            }

            if (keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S))
            {
                position += new Vector2(0, 3);
               
            }

            position += gamePadState.ThumbSticks.Left * new Vector2(3, -3);
           

            var mouseState = Mouse.GetState();
            Vector2 mouseWorld = Vector2.Transform(mouseState.Position.ToVector2(),Matrix.Invert(SceneManager.Instance.CurrentTranslation));

            // Look toward mouse
            Vector2 targetDirection = mouseWorld - position;

            // Right stick override
            Vector2 rightStick = gamePadState.ThumbSticks.Right;
            rightStick.Y *= -1;
            if (rightStick.Length() > 0.2f)
                targetDirection = rightStick;

            rotation = (float)Math.Atan2(targetDirection.Y, targetDirection.X);

            if (targetDirection.X < 0)
            {
                flipped = true;
                rotation += MathF.PI;
            }
            else
            {
                flipped = false;
            }

            if (currentMouse.LeftButton == ButtonState.Pressed && previousMouse.LeftButton == ButtonState.Released)
            {
                // Get mouse position
                Vector2 mousePos = new Vector2(currentMouse.X, currentMouse.Y);

                // Calculate direction vector from Traveler to mouse
                Vector2 direction = mousePos - position;
                if (direction != Vector2.Zero)
                    direction.Normalize();

                // Create bullet at Traveler’s center
                var bullet = new BulletSprite(position, direction);
                bullet.texture = bulletTexture;
                Bullets.Add(bullet);
            }

            foreach (var bullet in Bullets.ToList())
            {
                bullet.Update(gameTime);
                if (bullet.IsRemoved)
                    Bullets.Remove(bullet);
            }
        }

        /// <summary>
        /// draws the arm sprite
        /// </summary>
        /// <param name="gameTime">the game time</param>
        /// <param name="spriteBatch">the spriteBatch</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            SpriteEffects spriteEffect = flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            spriteBatch.Draw(texture, position, null, Color.White, rotation, new Vector2(texture.Width / 2f, texture.Height / 2f), 2, spriteEffect, 0);

            foreach (var bullet in Bullets)
                bullet.Draw(gameTime, spriteBatch);

        }
    }
}
