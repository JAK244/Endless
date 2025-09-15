using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;


namespace Endless
{
    /// <summary>
    /// enum of possible directions
    /// </summary>
    public enum Direction
    {
        Left, Right,
    }

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
        /// the direction the sprite moves
        /// </summary>
        public Direction direction;

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
        public void Update(GameTime gameTime)
        {
            
            directionTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if(BugFlipped == true)
            {
                switch (direction)
                {
                    case Direction.Left:
                        direction = Direction.Right; 
                        break;
                    //case Direction.Right:
                        //direction = Direction.Left;
                        //break;
                }
                directionTimer -= 2.0;
            }

            switch(direction)
            {
                case Direction.Left:
                    Position += new Vector2(-1, 0) * 20 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    break;
                case Direction.Right:
                    Position += new Vector2(1, 0) * 20 * (float)gameTime.ElapsedGameTime.TotalSeconds;

                    //stops it from going out of frame
                    if (Position.Y >= 353)
                    {
                        Position = new Vector2(Position.X, 353);
                        direction = Direction.Left; 
                    }
                    break;
            }
            
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

            if (animationTimer > 0.3)
            {
                animationFrame++;
                if (animationFrame > 3) animationFrame = 0;
                animationTimer -= 0.3;
            }

            var source = new Rectangle(animationFrame * 64, 16, 64, 48);
            spriteBatch.Draw(texture, Position, source, Color.White, 0f, new Vector2(0, 0), 2f, spriteEffect, 0f);

        }
    }
}
