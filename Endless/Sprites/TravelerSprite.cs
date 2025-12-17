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
        private double animationTimer;
        private short animationFrame;
        private bool IsMoving = false;
        private bool flipped;
        private BoundingRectangle bounds = new BoundingRectangle(new Vector2(400 -16,350 -16), 32,64);

        /// <summary>
        /// Checks if there health incressed
        /// </summary>
        public bool healthWentUp = false;

        /// <summary>
        /// the multiplier to speed
        /// </summary>
        public float SpeedMultiplier { get; set; } = 1f;

        /// <summary>
        /// the multiplier to damage
        /// </summary>
        public float DamageMultiplier = 1f;

        /// <summary>
        /// the check of health
        /// </summary>
        public int MaxHelth { get; set; } = 3;

        /// <summary>
        /// the sprites position
        /// </summary>
        public Vector2 position;


       
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
            texture = content.Load<Texture2D>("DuckTravler");
            
        }


        /// <summary>
        /// sets the bounds for the sprite to travle in
        /// </summary>
        /// <param name="mapSize">the given map size</param>
        /// <param name="tileSize">the given tile size</param>
        public void SetBounds(Point mapSize, Point tileSize)
        {
          
            minPos = new Vector2(0, 0);

            maxPos = new Vector2(mapSize.X * tileSize.X * 2,  
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

            IsMoving = false;

            // Movement input
            if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
            {
                move.X -= 3 * SpeedMultiplier;
                flipped = true;
                IsMoving = true;
            }

            if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
            {
                move.X += 3 * SpeedMultiplier;
                flipped = false;
                IsMoving = true;
            }

            if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W))
            {
                move.Y -= 3 * SpeedMultiplier;
                IsMoving = true;
            }

            if (keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S))
            {
                move.Y += 3 * SpeedMultiplier;
                IsMoving = true;
            }

            // Gamepad movement
            move += gamePadState.ThumbSticks.Left * new Vector2(3 * SpeedMultiplier, -3 * SpeedMultiplier);
            if (gamePadState.ThumbSticks.Left.Length() > 0.2f)
            {
                IsMoving = true;
                if (gamePadState.ThumbSticks.Left.X < 0) flipped = true;
                if (gamePadState.ThumbSticks.Left.X > 0) flipped = false;
            }
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

            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if (!IsMoving)
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
                if (animationTimer > 0.2)
                {
                    animationFrame++;
                    if (animationFrame > 7) animationFrame = 4;
                    animationTimer -= 0.2;
                }
            }

                var source = new Rectangle(animationFrame * 64, 0, 64, 64);
            spriteBatch.Draw(texture, position,source,color,0, new Vector2(32, 32), 2, spriteEffect,0);

            

        }
    }
}
