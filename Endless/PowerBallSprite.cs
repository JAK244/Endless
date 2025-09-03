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
    /// the power ball sprite class
    /// </summary>
    public class PowerBallSprite
    {
        private Texture2D texture;

        /// <summary>
        /// the positon of the sprite 
        /// </summary>
        public Vector2 Position = new Vector2(320, 352);

        /// <summary>
        /// checks if the the sprite needs to be flipped
        /// </summary>
        public bool BallFlipped;

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

        }

        /// <summary>
        /// Draws the sprite
        /// </summary>
        /// <param name="gameTime">the gmae time</param>
        /// <param name="spriteBatch">the sprite batch to render with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            SpriteEffects spriteEffect = BallFlipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            spriteBatch.Draw(texture, Position, null, Color.White, 0f, new Vector2(0, 0), 2f, spriteEffect, 0f);

        }
    }
}
