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
    public class HelthSprite
    {
        private Texture2D texture;

        public Vector2 position;

        /// <summary>
        /// Loads the sprite from the content manager
        /// </summary>
        /// <param name="content">the contnet manager to load with</param>
        public void LoadContent(ContentManager content)
        {

            texture = content.Load<Texture2D>("HealthHeart");
        }

        /// <summary>
        /// Updates the sprite;s postion on users input
        /// </summary>
        /// <param name="gameTime">the game time</param>
        public void Update(GameTime gameTime)
        {
           
        }

        /// <summary>
        /// Draws the sprite
        /// </summary>
        /// <param name="gameTime">the game time</param>
        /// <param name="spriteBatch">the sprite batch to render with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            
            spriteBatch.Draw(texture, position, null, Color.White, 0, new Vector2(0,0), 1f, SpriteEffects.None, 0);

        }
    }
}
