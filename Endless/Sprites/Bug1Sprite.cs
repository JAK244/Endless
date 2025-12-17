using Endless.Collisions;
using Endless.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
    /// the bug sprite class
    /// </summary>
    public class Bug1Sprite
    {
        private Texture2D texture;
        private SoundEffect hitEffect;
        private double animationTimer;
        private short animationFrame;
        private double hitFlashTimer = 0;
        private const double HitFlashDuration = 0.1; // 100ms
        private BoundingCircle bounds;

        /// <summary>
        /// the speed of the bugs
        /// </summary>
        public float Speed = 60f;

        /// <summary>
        /// points of these bugs
        /// </summary>
        public int PointsWorth = 10;

        /// <summary>
        /// the position of the sprite
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// checks if the sprite is flipped
        /// </summary>
        public bool BugFlipped;

        /// <summary>
        /// checks if bug is Alive
        /// </summary>
        public bool IsAlive = true;

        /// <summary>
        /// the color of the sprite
        /// </summary>
        public Color color { get; set; } = Color.White;


        /// <summary>
        /// the bugs bounds
        /// </summary>
        public BoundingCircle Bounds
        {
            get
            {
                  return bounds;
            }
        }

        /// <summary>
        /// the bug sprite constructor
        /// </summary>
        /// <param name="position">the given position</param>
        public Bug1Sprite(Vector2 position)
        {
           Position = position;
           bounds = new BoundingCircle(position - new Vector2(-64, -110), -16); // moves the bounds

        }
        /// <summary>
        /// handles taking a hit
        /// </summary>
        public void TakeHit()
        {
            hitEffect.Play(AudioSettings.SfxVolume, 0f, 0f);
            color = Color.Red;
            hitFlashTimer = HitFlashDuration;
        }
 

        /// <summary>
        /// Loads the texture
        /// </summary>
        /// <param name="content">the contentManager to load with</param>
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("bug");
            hitEffect = content.Load<SoundEffect>("bug1Hit");
        }

        /// <summary>
        /// updates the sprite
        /// </summary>
        /// <param name="gameTime">the game time</param>
        public void Update(GameTime gameTime, Vector2 playerPosition)
        {

            if (hitFlashTimer > 0)
            {
                hitFlashTimer -= gameTime.ElapsedGameTime.TotalSeconds;
                if (hitFlashTimer <= 0)
                {
                    color = Color.White;
                }
            }


            if (!IsAlive)
            {
                bounds.Center = new Vector2(10000, 1000000); // removes the bounds in a goofy way
                return;
            }


            Vector2 bugCenter = Position + new Vector2(60,100);

          
            Vector2 toPlayer = playerPosition - bugCenter;

            if (toPlayer != Vector2.Zero)
                toPlayer.Normalize();

            
            Position += toPlayer * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            BugFlipped = toPlayer.X > 0;

           
            bounds.Center = bugCenter;
        }


        /// <summary>
        /// Draws the sprite
        /// </summary>
        /// <param name="gameTime">the game time</param>
        /// <param name="spriteBatch">the sprite batch to render with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            SpriteEffects spriteEffect = BugFlipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if (IsAlive)
            {
                if (animationTimer > 0.2)
                {
                    animationFrame++;
                    if (animationFrame > 3) animationFrame = 0;
                    animationTimer -= 0.2;
                }
            }
            else
            {
                if (animationFrame < 7) 
                {
                    if (animationTimer > 0.2)
                    {
                        animationFrame++;
                        animationTimer = 0; 
                    }
                }
                
            }

            var source = new Rectangle(animationFrame * 64, 0, 64, 64);
            spriteBatch.Draw(texture, Position, source, color, 0f, Vector2.Zero, 2f, spriteEffect, 0f);


        }
    }
}
