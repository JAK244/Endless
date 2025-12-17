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
    /// the travelrs arm sprite class
    /// </summary>
    public class ArmSprite
    {
        private KeyboardState keyboardState;
        private GamePadState gamePadState;
        private MouseState mouseState;
        private MouseState currentMouse;
        private MouseState previousMouse;
        private float previousRightTrigger;
        private Texture2D bulletTexture;
        private Texture2D texture;
        private Texture2D ball;
        private SoundEffect gunShotSound;
        private TravelerSprite player;
        private float rotation;
        private bool flipped;
        private Vector2 minPos, maxPos;
        private double fireCooldown = 2.0; // how often to fire
        private double fireTimer = 0;

        /// <summary>
        /// the list of bullets
        /// </summary>
        public List<BulletSprite> Bullets = new List<BulletSprite>();

        /// <summary>
        /// the arms position
        /// </summary>
        public Vector2 position;


        /// <summary>
        /// checks if realoading
        /// </summary>
        public bool isReloading = false;

        /// <summary>
        /// the durration of the reload
        /// </summary>
        public double reloadDuration = 2.0;

        /// <summary>
        /// checks fire rate
        /// </summary>
        public bool fireRateLower = false;

        /// <summary>
        /// the arm sprite constructor
        /// </summary>
        /// <param name="player">the given player sprite</param>
        public ArmSprite(TravelerSprite player)
        {
            this.player = player;
            position = player.position;
        }

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
        /// the bounds of how far the arm can go 
        /// </summary>
        /// <param name="mapSize">the mapSize</param>
        /// <param name="tileSize">the TitleSize</param>
        public void SetBounds(Point mapSize, Point tileSize)
        {
            
            minPos = new Vector2(0, 0);

            // max is bottom-right of map 
            maxPos = new Vector2(mapSize.X * tileSize.X * 2, 
                                 mapSize.Y * tileSize.Y * 2);
        }

        /// <summary>
        /// calculates position to shoot
        /// </summary>
        private void TryShoot()
        {
            
            // direction to shoot
            Vector2 direction = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
            if (direction != Vector2.Zero)
                direction.Normalize();

            // barrel offset
            float barrelLength = (texture.Width * 0.5f * 2f) + 4f;
            Vector2 barrelOffset = direction * barrelLength;

            Vector2 bulletSpawnPos = (position + barrelOffset) + new Vector2(87, 14);

            var bullet = new BulletSprite(bulletSpawnPos, direction);
            bullet.texture = bulletTexture;
            Bullets.Add(bullet);

            gunShotSound.Play(AudioSettings.SfxVolume, 0f, 0f);             
        
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

            position = player.position;
            
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

            // handles firing gun
            fireTimer -= gameTime.ElapsedGameTime.TotalSeconds;

            if (fireTimer <= 0)
            {
                TryShoot();
                if(fireRateLower == false)
                {
                    fireTimer = fireCooldown; // reset timer
                }
                else
                {
                    fireCooldown = fireCooldown * 0.5f;
                    fireTimer = fireCooldown;
                    fireRateLower = false;
                }
            }

            // track mouse for rotating
            rotation = (float)Math.Atan2(targetDirection.Y, targetDirection.X);
            flipped = targetDirection.X < 0;

            // updates bullet list
            foreach (var bullet in Bullets.ToList())
            {
                bullet.Update(gameTime);
                if (bullet.IsRemoved)
                    Bullets.Remove(bullet);
            }

            // clamp inside map
            position.X = MathHelper.Clamp(position.X, minPos.X + 16, maxPos.X - 16);
            position.Y = MathHelper.Clamp(position.Y, minPos.Y + 16, maxPos.Y - 16);

            
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
