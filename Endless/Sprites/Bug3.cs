using Endless.Collisions;
using Endless.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Endless.Sprites
{
    public class Bug3
    {
        private Texture2D texture;
        private double dropTimer = 0.0;
        private double dropInterval;
        private double animationTimer;
        private short animationFrame;
        private double hitFlashTimer = 0;
        private const double HitFlashDuration = 0.1; // 100ms
        private BoundingCircle bounds;
        private SoundEffect hitSound;

        /// <summary>
        /// checks if it can drop ooze
        /// </summary>
        public bool CanDropOoze => IsAlive;

        /// <summary>
        /// activates when ooze is droped
        /// </summary>
        public event Action<Vector2> OnDropOoze;


        /// <summary>
        /// the speed of the bugs
        /// </summary>
        public float Speed = 150f;

        /// <summary>
        /// points of these bugs
        /// </summary>
        public int PointsWorth = 10;

        /// <summary>
        /// the position of the sprite
        /// </summary>
        public Vector2 Position;

        public int bug3helth = 1; // two hits to kill

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
        public Bug3(Vector2 position)
        {
            Position = position;
            bounds = new BoundingCircle(position - new Vector2(-64, -110), -16); // moves the bounds
            dropInterval = 3.0 + (new Random().NextDouble());
        }

        /// <summary>
        /// handles taking a hit
        /// </summary>
        public void TakeHit()
        {
            color = Color.Red;
            hitFlashTimer = HitFlashDuration;
            hitSound.Play(AudioSettings.SfxVolume, 0f, 0f);
        }

        /// <summary>
        /// handles droping ooze
        /// </summary>
        /// <param name="gameTime">the game time</param>
        /// <returns></returns>
        public Ooze TryDropOoze(GameTime gameTime)
        {
            if (!IsAlive) return null;
            dropTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if (dropTimer >= dropInterval)
            {
                dropTimer = 0;
                dropInterval = 3.0 + (new Random().NextDouble()); // reset interval 3-4 sec
                return new Ooze(Position + new Vector2(350,20)); // drop at current bug position
            }

            return null;
        }

        /// <summary>
        /// Loads the texture
        /// </summary>
        /// <param name="content">the contentManager to load with</param>
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Bug3");
            hitSound = content.Load<SoundEffect>("bug3Hit");
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


            Vector2 bugCenter = Position + new Vector2(20, 40);


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
                    if (animationFrame > 4) animationFrame = 0;
                    animationTimer -= 0.2;
                }
            }
            else
            {
                if (animationFrame < 8)
                {
                    if (animationTimer > 0.2)
                    {
                        animationFrame++;
                        animationTimer = 0;
                    }
                }

            }

            var source = new Rectangle(animationFrame * 64, 0, 64, 64);
            spriteBatch.Draw(texture, Position, source, color, 0f, Vector2.Zero, 1f, spriteEffect, 0f);


        }
    }

}
