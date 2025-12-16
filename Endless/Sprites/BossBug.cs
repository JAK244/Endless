using Endless.Collisions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Endless.Sprites
{
    public class BossBug
    {
            private Texture2D texture;

            private double dropTimer = 0.0;
            private double dropInterval;
            public bool CanDropOoze => IsAlive;
            public event Action<Vector2> OnDropOoze;

            /// <summary>
            /// the speed of the bugs
            /// </summary>
            public float Speed = 150f;

            /// <summary>
            /// points of these bugs
            /// </summary>
            public int PointsWorth = 10000;

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

            private double hitFlashTimer = 0;
            private const double HitFlashDuration = 0.1; // 100ms

            /// <summary>
            /// checks if bug is Alive
            /// </summary>
            public bool IsAlive = true;

            public int Bug2health = 10; // phase 1: 5 hits, phase 2: 5 hits two open intervles, phase 3: 1 hit 

            /// <summary>
            /// the color of the sprite
            /// </summary>
            public Color color { get; set; } = Color.White;

            private BoundingCircle bounds;
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
            /// the bugs bounds
            /// </summary>
            public BoundingCircle Bounds
            {
                get
                {
                    return bounds;
                }
            }

            public void TakeHit()
            {
                color = Color.Red;
                hitFlashTimer = HitFlashDuration;
            }

            public BossOoze TryDropOoze(GameTime gameTime)
            {
                if (!IsAlive) return null;
                dropTimer += gameTime.ElapsedGameTime.TotalSeconds;

                if (dropTimer >= dropInterval)
                {
                    dropTimer = 0;
                    dropInterval = 3.0 + (new Random().NextDouble()); // reset interval 3-4 sec
                    return new BossOoze(Position + new Vector2(450, 130)); // drop at current bug position
                }

                return null;
            }

            /*
            private void Shoot(Vector2 playerPosition)
            {
                // center spawn — still using your 290,80 hack for now
                Vector2 firePosition = Position + new Vector2(290, 80);

                int bulletCount = 12; // number of bullets in the circle
                float angleStep = MathHelper.TwoPi / bulletCount;

                for (int i = 0; i < bulletCount; i++)
                {
                    float angle = angleStep * i;

                    Vector2 direction = new Vector2(
                        (float)Math.Cos(angle),
                        (float)Math.Sin(angle)
                    );

                    EnemyFire bullet = new EnemyFire(firePosition, direction);
                    bullet.LoadContent(content);
                    enemyBullets.Add(bullet);
                }
            }
            */

            /// <summary>
            /// the bug sprite constructor
            /// </summary>
            /// <param name="position">the given position</param>
            public BossBug(Vector2 position, List<EnemyFire> bullets, ContentManager content)
            {
                Position = position;
                this.enemyBullets = bullets;
                this.content = content;
                bounds = new BoundingCircle(position - new Vector2(-64, -110), -16);
                attackRange = new BoundingCircle(position, 350);
            }

            /// <summary>
            /// Loads the texture
            /// </summary>
            /// <param name="content">the contentManager to load with</param>
            public void LoadContent(ContentManager content)
            {
                texture = content.Load<Texture2D>("BOSSBUG");
            }

            /// <summary>
            /// updates the sprite
            /// </summary>
            /// <param name="gameTime">the game time</param>
            public void Update(GameTime gameTime, Vector2 playerPosition)
            {
                if (hitFlashTimer > 0)
                {
                    hitFlashTimer -= gameTime.ElapsedGameTime.TotalSeconds;
                    if (hitFlashTimer <= 0)
                    {
                        color = Color.White;
                    }
                }

                if (!IsAlive)
                {
                    bounds.Center = new Vector2(10000, 1000000); // removes the bounds in a goofy way
                    return;
                }

                attackTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                // update attack range position
                attackRange.Center = Position + new Vector2(64, 64);
              

                Vector2 bugCenter = Position + new Vector2(160, 130);
                Vector2 toPlayer = playerPosition - bugCenter;

                if (toPlayer != Vector2.Zero)
                    toPlayer.Normalize();

                Position += toPlayer * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                BugFlipped = toPlayer.X > 0;

                bounds.Center = bugCenter;
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
                        if (animationFrame > 4) animationFrame = 0;
                        animationTimer -= 0.2;
                    }
                }
                

                var source = new Rectangle(animationFrame * 128, 0, 128, 128);
                spriteBatch.Draw(texture, Position, source, color, 0f, Vector2.Zero, 2f, spriteEffect, 0f);


            }

    }
}
