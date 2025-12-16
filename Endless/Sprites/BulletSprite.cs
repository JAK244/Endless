using Endless.Collisions;
using Endless.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Endless.Sprites
{
    /// <summary>
    /// the bullet sprite class
    /// </summary>
    public class BulletSprite
    {
        private float timer;
        private float lifeSpan = .8f;
        private float speed = 300f;

        /// <summary>
        /// checks if bullet is removed
        /// </summary>
        public bool IsRemoved = false;

        /// <summary>
        /// the bullet texture
        /// </summary>
        public Texture2D texture;

        /// <summary>
        /// the sprite position
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// the sprite Direction
        /// </summary>
        public Vector2 Direction;

        private double animationTimer;
        private short animationFrame;
        private BoundingCircle bounds;

        

        /// <summary>
        /// the bullet bounds
        /// </summary>
        public BoundingCircle Bounds
        {
            get
            {
                return bounds;
            }
        }

        /// <summary>
        /// the bullet constructor
        /// </summary>
        /// <param name="position">the positon</param>
        /// <param name="direction">the direction</param>
        public BulletSprite( Vector2 position, Vector2 direction)
        {
            Position = position;
            Direction = direction;
            Direction.Normalize(); 
            bounds = new BoundingCircle(position - new Vector2(-64, -110), -16);
        }

        /// <summary>
        /// Loads the texture
        /// </summary>
        /// <param name="content">the contentManager to load with</param>
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Bullet");
        }

        /// <summary>
        /// updates the bullet using game time
        /// </summary>
        /// <param name="gameTime">the game time</param>
        public void Update(GameTime gameTime)
        {
            Vector2 bulletCenter = Position + new Vector2(-90, 0);
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timer >= lifeSpan)
            {
                 IsRemoved = true;
            }

            Position += Direction * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            bounds.Center = bulletCenter;

            
        }

        /// <summary>
        /// draws and animates the sprite using a spriteBatch and gametime
        /// </summary>
        /// <param name="gameTime">the gametime</param>
        /// <param name="spriteBatch">the spritebatch</param>
        public void Draw(GameTime gameTime,SpriteBatch spriteBatch)
        {

            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if (animationTimer > 0.3)
            {
                animationFrame++;
                if (animationFrame > 3) animationFrame = 0;
                animationTimer -= 0.3;
            }


            var source = new Rectangle(animationFrame * 64, 0, 64, 64);
            spriteBatch.Draw(texture, Position, source, Color.White, 0f, new Vector2(texture.Width / 2f, texture.Height / 2f), 1f, SpriteEffects.None, 0f);

            

        }
    }
}
