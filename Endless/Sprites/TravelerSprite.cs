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
using SharpDX.MediaFoundation;



namespace Endless.Sprites
{
    /// <summary>
    /// class for our mc
    /// </summary>
    public class TravelerSprite
    {
        private KeyboardState keyboardState;

        private GamePadState gamePadState;

        private Texture2D texture;

        private Vector2 minPos, maxPos;

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
        /// sets the bounds for the sprite to travle in
        /// </summary>
        /// <param name="mapSize">the given map size</param>
        /// <param name="tileSize">the given tile size</param>
        public void SetBounds(Point mapSize, Point tileSize)
        {
          
            minPos = new Vector2(0, 0);

            // max is bottom-right of map (scaled by tiles)
            maxPos = new Vector2(mapSize.X * tileSize.X * 2,  // times 2 because you scaled tiles in Map
                                 mapSize.Y * tileSize.Y * 2);
        }


        /// <summary>
        /// updates the sprite using gametime
        /// </summary>
        /// <param name="gameTime">the gametime</param>
        public void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();
            gamePadState = GamePad.GetState(0);

            Vector2 move = Vector2.Zero;

            if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
            {
                move.X -= 3;
                flipped = true;
            }

            if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
            {
                move.X += 3;
                flipped = false;
            }

            if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W))
                move.Y -= 3;

            if (keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S))
                move.Y += 3;

            move += gamePadState.ThumbSticks.Left * new Vector2(3, -3);
            if (gamePadState.ThumbSticks.Left.X < 0) flipped = true;
            if (gamePadState.ThumbSticks.Left.X > 0) flipped = false;

            // update position
            position += move;

            // clamps inside the map
            position.X = MathHelper.Clamp(position.X, minPos.X + 16, maxPos.X - 16);
            position.Y = MathHelper.Clamp(position.Y, minPos.Y + 16, maxPos.Y - 16);

            // update bounds
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
          
            SpriteEffects spriteEffect = flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            spriteBatch.Draw(texture, position,null,color,0, new Vector2(texture.Width / 2f, texture.Height / 2f), 2, spriteEffect,0);

            

        }
    }
}
