using Endless.Collisions;
using Endless.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct2D1.Effects;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Endless.Sprites
{
    public class BulletSprite
    {
        private float timer;

        private float lifeSpan = 2f;

        private float speed = 300f;

        public bool IsRemoved = false;

        public Texture2D texture;

        public Vector2 Position;

        public Vector2 Direction;

        private double animationTimer;

        private short animationFrame;

        private BoundingCircle bounds;

        

        /// <summary>
        /// the bugs bounds
        /// </summary>
        public BoundingCircle Bounds
        {
            get
            {
                return bounds;
            }
        }

        public BulletSprite( Vector2 position, Vector2 direction)
        {
            Position = position;
            Direction = direction;
            Direction.Normalize(); // make sure it's unit length
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
