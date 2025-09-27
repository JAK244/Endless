using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;


namespace Endless.Sprites
{
    /// <summary>
    /// the Star sprite class
    /// </summary>
    public class StarSprite
    {
        private Texture2D texture;

        private double animationTimer;

        private short animationFrame;

        /// <summary>
        /// the positon of the sprite
        /// </summary>
        public Vector2 Position;


        /// <summary>
        /// Loads the Texture
        /// </summary>
        /// <param name="content">the contentmanaager to load to</param>
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Star");
        }

        /// <summary>
        /// updates the sprite
        /// </summary>
        /// <param name="gameTime">the game time</param>
        public void Update(GameTime gameTime)
        {

        }

        /// <summary>
        /// Draws/animates the game sprite
        /// </summary>
        /// <param name="gameTime">the game time</param>
        /// <param name="spriteBatch">the spriteBatch to render with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if(animationTimer > 0.3)
            {
                animationFrame++;
                if (animationFrame > 5) animationFrame = 0;
                animationTimer -= 0.3;
            }

            
            var source = new Rectangle(animationFrame * 64,16,64,64);
            spriteBatch.Draw(texture,Position,source,Color.White,0f,new Vector2(0,0),2,SpriteEffects.None,0f);

        }
    }
}
