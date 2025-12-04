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
namespace Endless.Sprites
{
    public class Bug2
    {
        private Texture2D texture;

        /// <summary>
        /// the direction timer
        /// </summary>
        private double directionTimer;

        /// <summary>
        /// the speed of the bugs
        /// </summary>
        public float Speed = 60f;

        /// <summary>
        /// points of these bugs
        /// </summary>
        public int PointsWorth = 100;

        /// <summary>
        /// the position of the sprite
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// checks if the sprite is flipped
        /// </summary>
        public bool BugFlipped;

        private double animationTimer;

        private short animationFrame;

        /// <summary>
        /// checks if bug is Alive
        /// </summary>
        public bool IsAlive = true;

        /// <summary>
        /// the color of the sprite
        /// </summary>
        public Color color { get; set; } = Color.White;

        private BoundingRectangle bounds = new BoundingRectangle(new Vector2(400 - 16, 350 - 16), 32, 64);
        private BoundingCircle attackRange;

        private float attackCooldown = 0.5f; // time between bursts
        private float attackTimer = 0f;

        public bool IsAttacking = false;

        // we need a reference to a bullet list so Bug2 can spawn bullets
        private List<EnemyFire> enemyBullets;
        private ContentManager content;

        /// <summary>
        /// the bugs bounds
        /// </summary>
        public BoundingCircle AttackRange
        {
            get
            {
                return attackRange;
            }
        }


        /// <summary>
        /// the sprites bounds
        /// </summary>
        public BoundingRectangle Bounds
        {
            get { return bounds; }
        }

        private void Shoot(Vector2 playerPosition)
        {
            // direction from bug → player
            Vector2 firePosition = Position + new Vector2(290,80);

            Vector2 dir = playerPosition - firePosition;
            dir.Normalize();

            //fire 3 bullets spread slightly
            Vector2 left = new Vector2(dir.X, dir.Y - 0.2f);
            Vector2 center = dir;
            Vector2 right = new Vector2(dir.X, dir.Y + 0.2f);

            // normalize each
            left.Normalize();
            center.Normalize();
            right.Normalize();



            EnemyFire b1 = new EnemyFire(firePosition, left);
            EnemyFire b2 = new EnemyFire(firePosition, center);
            EnemyFire b3 = new EnemyFire(firePosition, right);

            b1.LoadContent(content);
            b2.LoadContent(content);
            b3.LoadContent(content);

            enemyBullets.Add(b1);
            enemyBullets.Add(b2);
            enemyBullets.Add(b3);
        }

        /// <summary>
        /// the bug sprite constructor
        /// </summary>
        /// <param name="position">the given position</param>
        public Bug2(Vector2 position, List<EnemyFire> bullets, ContentManager content)
        {
            Position = position;
            this.enemyBullets = bullets;
            this.content = content;

            attackRange = new BoundingCircle(position, 350);
        }

        /// <summary>
        /// Loads the texture
        /// </summary>
        /// <param name="content">the contentManager to load with</param>
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Bug2");
        }

        /// <summary>
        /// updates the sprite
        /// </summary>
        /// <param name="gameTime">the game time</param>
        public void Update(GameTime gameTime, Vector2 playerPosition)
        {
            if (!IsAlive) return;

            attackTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            // update attack range position
            attackRange.Center = Position + new Vector2(64, 64);

            // check if player is inside attack range
            bool playerInRange = attackRange.CollidesWith(new BoundingCircle(playerPosition, 10));

            if (playerInRange)
            {
                IsAttacking = true;

                // stop movement
                Vector2 zero = Vector2.Zero;

                // shoot if cooldown finished
                if (attackTimer >= attackCooldown)
                {
                    Shoot(playerPosition);
                    attackTimer = 0f;
                }

                return; // don't run move logic
            }
            else
            {
                IsAttacking = false;
            }


            // ------- movement (only when NOT attacking) -------

            Vector2 bugCenter = Position + new Vector2(60, 100);
            Vector2 toPlayer = playerPosition - bugCenter;

            if (toPlayer != Vector2.Zero)
                toPlayer.Normalize();

            Position += toPlayer * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            BugFlipped = toPlayer.X > 0;

            bounds.X = bugCenter.X - 16;
            bounds.Y = bugCenter.Y - 50;
        }


        /// <summary>
        /// Draws the sprite
        /// </summary>
        /// <param name="gameTime">the game time</param>
        /// <param name="spriteBatch">the sprite batch to render with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            SpriteEffects spriteEffect = BugFlipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if (IsAlive)
            {
                if (animationTimer > 0.2)
                {
                    animationFrame++;
                    if (animationFrame > 5) animationFrame = 0;
                    animationTimer -= 0.2;
                }
            }
            else
            {
                if (animationFrame < 9)
                {
                    if (animationTimer > 0.2)
                    {
                        animationFrame++;
                        animationTimer = 0;
                    }
                }

            }

            var source = new Rectangle(animationFrame * 64, 0, 64, 64);
            spriteBatch.Draw(texture, Position, source, Color.White, 0f, Vector2.Zero, 2f, spriteEffect, 0f);


        }
    
    }
}
