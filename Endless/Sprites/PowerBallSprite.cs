using Endless.Collisions;
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
    /// the power ball sprite class
    /// </summary>
    public class PowerBallSprite
    {
        private Texture2D texture;

        private double animationTimer;

        private short animationFrame;

        /// <summary>
        /// the positon of the sprite 
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// checks if the the sprite needs to be flipped
        /// </summary>
        public bool BallFlipped;

        private BoundingRectangle bounds = new BoundingRectangle(new Vector2(770, 760), 32, 64);

        /// <summary>
        /// the sprites bounds
        /// </summary>
        public BoundingRectangle Bounds
        {
            get { return bounds; }
        }

        /// <summary>
        /// Loads the texture
        /// </summary>
        /// <param name="content">the contentManager to load with</param>
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Power_ball");
        }

        /// <summary>
        /// updates the sprite
        /// </summary>
        /// <param name="gameTime">the game time</param>
        public void Update(GameTime gameTime)
        {
            bounds.X = Position.X - 16;
            bounds.Y = Position.Y - 16;
        }

        /// <summary>
        /// Draws the sprite
        /// </summary>
        /// <param name="gameTime">the gmae time</param>
        /// <param name="spriteBatch">the sprite batch to render with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            SpriteEffects spriteEffect = BallFlipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if (animationTimer > 0.2)
            {
                animationFrame++;
                if (animationFrame > 7) animationFrame = 0;
                animationTimer -= 0.2;
            }


            var source = new Rectangle(animationFrame * 64, 16, 64, 48);
            spriteBatch.Draw(texture, Position, source, Color.White, 0f, new Vector2(0, 0), 2f, spriteEffect, 0f);

        }
    }
}
