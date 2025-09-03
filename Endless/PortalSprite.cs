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
    /// the Potal sprite class
    /// </summary>
    public class PortalSprite
    {
        private Texture2D texture;
        
        /// <summary>
        /// the position of the sprite
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// Checks is the sprite is flipped
        /// </summary>
        public bool PortalFlipped;

        /// <summary>
        /// Loads the texture
        /// </summary>
        /// <param name="content">the content manager to load with</param>
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Void-portal");
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
        /// <param name="gameTime">the game time</param>
        /// <param name="spriteBatch">the sprite batch to render with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            SpriteEffects spriteEffect = PortalFlipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            spriteBatch.Draw(texture,Position,null,Color.White,0f,new Vector2(0,0), 2f,spriteEffect,0f);
            
        }
    }
}
