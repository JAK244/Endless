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
    /// <summary>
    /// class for our mc
    /// </summary>
    public class TravelerSprite
    {
        private KeyboardState keyboardState;

        private GamePadState gamePadState;

        private Texture2D texture;

        /// <summary>
        /// the sprites position
        /// </summary>
        public Vector2 position;

        private bool flipped;

        private BoundingRectangle bounds = new BoundingRectangle(new Vector2(400 -16,350 -16), 32,64);

       
        /// <summary>
        /// the sprites bounds
        /// </summary>
        public BoundingRectangle Bounds
        {
            get { return bounds; }
        }
        
        /// <summary>
        /// the sprites color
        /// </summary>
        public Color color { get; set; } = Color.White;

        /// <summary>
        /// Loads the sprite from the content manager
        /// </summary>
        /// <param name="content">the contnet manager to load with</param>
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Traveler");
        }

        /// <summary>
        /// Updates the sprites postion based on users input and handles bounds movement
        /// </summary>
        /// <param name="gameTime">the game time</param>
        public void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();
            gamePadState = GamePad.GetState(0);

            if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
            {
                position += new Vector2(-3, 0);
                flipped = true;
            }
            
            if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
            {
                position += new Vector2(3, 0);
                flipped = false;
            }

            /*
            if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W))
            {
                position += new Vector2(0, -3);
                flipped = true;
            }

            if (keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S))
            {
                position += new Vector2(0, 3);
                flipped = false;
            }
            */
           

            position += gamePadState.ThumbSticks.Left * new Vector2(3, 0);
            if (gamePadState.ThumbSticks.Left.X < 0) flipped = true;
            if (gamePadState.ThumbSticks.Left.X > 0) flipped = false;

            bounds.X = position.X - 16;
            bounds.Y = position.Y - 16;

            

            
        }

        /// <summary>
        /// Draws the sprite
        /// </summary>
        /// <param name="gameTime">the game time</param>
        /// <param name="spriteBatch">the sprite batch to render with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
          
            SpriteEffects spriteEffect = (flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
            spriteBatch.Draw(texture, position,null,color,0, new Vector2(texture.Width / 2f, texture.Height / 2f), 2, spriteEffect,0);
         
        }
    }
}
