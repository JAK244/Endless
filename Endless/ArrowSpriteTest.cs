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
    public class ArrowSpriteTest
    {
        private KeyboardState keyboardState;

        private Texture2D texture;

        private Vector2 position = new Vector2(200, 200);

        private bool flipped;

        private BoundingRectangle bounds = new BoundingRectangle(new Vector2(200 - 16, 200 - 16), 32, 32);

        private float rotation;

        public BoundingRectangle Bounds
        {
            get { return bounds; }
        }

        /// <summary>
        /// Loads the sprite from the content manager
        /// </summary>
        /// <param name="content">the contnet manager to load with</param>
        public void LoadContent(ContentManager content)
        {
            rotation = 0.0f;
            texture = content.Load<Texture2D>("ArrowSprite");
        }

        /// <summary>
        /// Updates the sprite;s postion on users input
        /// </summary>
        /// <param name="gameTime">the game time</param>
        public void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();

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

            bounds.X = position.X - bounds.Width / 2f;
            bounds.Y = position.Y - bounds.Height / 2f;

            Vector2 mousePosition = Mouse.GetState().Position.ToVector2();

            Vector2 distance = mousePosition - position;

            rotation = (float)Math.Atan2(distance.Y, distance.X);
        }

        /// <summary>
        /// Draws the sprite
        /// </summary>
        /// <param name="gameTime">the game time</param>
        /// <param name="spriteBatch">the sprite batch to render with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            SpriteEffects spriteEffect = (flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
            spriteBatch.Draw(texture, position, null, Color.White, rotation, new Vector2(texture.Width / 2f, texture.Height / 2f), 2, spriteEffect, 0);

        }
    }
}

