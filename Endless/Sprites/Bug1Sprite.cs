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


namespace Endless.Sprites
{
   

    /// <summary>
    /// the bug sprite class
    /// </summary>
    public class Bug1Sprite
    {
        private Texture2D texture;

        /// <summary>
        /// the direction timer
        /// </summary>
        private double directionTimer;

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

        private double animationTimer;

        private short animationFrame;

        /// <summary>
        /// checks if bug is Alive
        /// </summary>
        public bool IsAlive = true;

        /// <summary>
        /// the color of the sprite
        /// </summary>
        public Color color { get; set; } = Color.White;

        private BoundingCircle bounds;

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
        /// Loads the texture
        /// </summary>
        /// <param name="content">the contentManager to load with</param>
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("bug");
        }

        /// <summary>
        /// updates the sprite
        /// </summary>
        /// <param name="gameTime">the game time</param>
        public void Update(GameTime gameTime, Vector2 playerPosition)
        {
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
            spriteBatch.Draw(texture, Position, source, Color.White, 0f, Vector2.Zero, 2f, spriteEffect, 0f);


        }
    }
}
