using Endless.Collisions;
using Endless.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Audio;


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

        private Texture2D ball;

        private SoundEffect gunShotSound;

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
            ball = content.Load<Texture2D>("Ball");
            gunShotSound = content.Load<SoundEffect>("GunShotSound");
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

            flipped = targetDirection.X < 0;

            if (currentMouse.LeftButton == ButtonState.Pressed && previousMouse.LeftButton == ButtonState.Released)
            {
                Vector2 direction = mouseWorld - position;
                if (direction != Vector2.Zero)
                    direction.Normalize();

                float barrelLength = (texture.Width * 0.5f * 2f) + 4f;
                
                Vector2 barrelOffset = new Vector2((float)Math.Cos(rotation),(float)Math.Sin(rotation)) * barrelLength;
                Vector2 bulletSpawnPos = (position + barrelOffset) + new Vector2(87, 14);

                var bullet = new BulletSprite(bulletSpawnPos, direction);
                bullet.texture = bulletTexture;
                Bullets.Add(bullet);
                gunShotSound.Play();

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
            SpriteEffects spriteEffect = flipped ? SpriteEffects.FlipVertically : SpriteEffects.None;
            spriteBatch.Draw(texture, position, null, Color.White, rotation, new Vector2(texture.Width / 2f, texture.Height / 2f), 2, spriteEffect, 0);

            foreach (var bullet in Bullets)
            {
                //var rec = new Rectangle((int)(bullet.Bounds.Center.X - bullet.Bounds.Radius), (int)(bullet.Bounds.Center.Y - bullet.Bounds.Radius), (int)bullet.Bounds.Radius * 2, (int)bullet.Bounds.Radius * 2);
                //spriteBatch.Draw(ball, rec, Color.White);

                bullet.Draw(gameTime, spriteBatch);
            }

        }
    }
}
