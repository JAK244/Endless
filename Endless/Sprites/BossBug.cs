using Endless.Collisions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Endless.Sprites
{
    /// <summary>
    /// Boss Class
    /// </summary>
    public class BossBug
    {
        /// <summary>
        /// enum for phase 3
        /// </summary>
        private enum Phase3State
        {
            Disappeared,
            Targeting,
            Charging,
            Open
        }
        private Phase3State phase3State = Phase3State.Disappeared;
        private float phase3Timer = 0f;
        private int chargeCount = 0;
        private Vector2 chargeDirection;
        private float chargeSpeed = 900f;
        private bool visible = true;
        private bool directionLocked = false;

        /// <summary>
        /// enum for phase 2
        /// </summary>
        private enum Phase2State
        {
            Attacking,
            Open
        }
        private Phase2State phase2State = Phase2State.Attacking;
        private float phase2Timer = 0f;
        private float shootTimer = 0f;
        private float shootInterval = 0.25f; // how fast bullets spawn
        private float starRotation = 0f;     // rotates the pattern


        private Texture2D texture;
        private double dropTimer = 0.0;
        private double dropInterval;
        private readonly Vector2 hitboxOffsetRight = new Vector2(160, 130); // offset for bounds
        private readonly Vector2 hitboxOffsetLeft = new Vector2(96, 130); // offset for bounds
        private Vector2 screenCenter = new Vector2(785,725); // the center of the screen
        private double animationTimer;
        private short animationFrame;
        private double hitFlashTimer = 0;
        private const double HitFlashDuration = 0.1; // 100ms
        private bool reachedCenter = false;
        private BoundingCircle bounds;
        private BoundingCircle attackRange;
        private List<EnemyFire> enemyBullets;
        private ContentManager content;

        /// <summary>
        /// checks if attacking
        /// </summary>
        public bool IsAttacking = false;

        /// <summary>
        /// checks if alive to use ooze
        /// </summary>
        public bool CanDropOoze => IsAlive;

        /// <summary>
        /// handles droped ooze
        /// </summary>
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

        /// <summary>
        /// checks if bug is Alive
        /// </summary>
        public bool IsAlive = true;

        public int Bosshealth = 10; // phase 1: 5 hits, phase 2: 5 hits two open intervles, phase 3: 1 hit full helth 10

        /// <summary>
        /// checking if in phase 1
        /// </summary>
        public bool Phase1 = true;

        /// <summary>
        /// checking if in phase 2
        /// </summary>
        public bool Phase2 = false;

        /// <summary>
        /// checking if in phase 3
        /// </summary>
        public bool Phase3 = false;
      
        /// <summary>
        /// the color of the sprite
        /// </summary>
        public Color color { get; set; } = Color.White;

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

        /// <summary>
        /// handles taking a hit
        /// </summary>
        public void TakeHit()
        {
            color = Color.Red;
            hitFlashTimer = HitFlashDuration;
        }

        /// <summary>
        /// handles Droping ooze
        /// </summary>
        /// <param name="gameTime">the game time</param>
        /// <returns>the ooze</returns>
        public BossOoze TryDropOoze(GameTime gameTime)
        {
            if (!IsAlive || Phase1 != true) return null;
            dropTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if (dropTimer >= dropInterval)
            {
                dropTimer = 0;
                dropInterval = 3.0 + (new Random().NextDouble()); // reset interval 3-4 sec
                return new BossOoze(Position + new Vector2(450, 130)); // drop at current bug position
            }

            return null;
        }

        /// <summary>
        /// handles the start pattern when shooting 
        /// </summary>
        private void ShootStar()
        {
            Vector2 firePosition = Position + new Vector2(358, 125); // center of boss

            int points = 5; // 5-point star
            float angleStep = MathHelper.TwoPi / points;

            for (int i = 0; i < points; i++)
            {
                float angle = starRotation + angleStep * i;

                Vector2 direction = new Vector2(
                    (float)Math.Cos(angle),
                    (float)Math.Sin(angle)
                );

                EnemyFire bullet = new EnemyFire(firePosition, direction);
                bullet.LoadContent(content);
                enemyBullets.Add(bullet);
            }

            starRotation += 0.15f; //rotation
        }


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
            Debug.WriteLine(Bosshealth);
            if (!IsAlive)
            {
                bounds.Center = new Vector2(10000, 1000000); // removes the bounds in a goofy way
                return;
            }
            Vector2 hitboxOffset = BugFlipped ? hitboxOffsetRight : hitboxOffsetLeft;
            Vector2 bugCenter = Position + hitboxOffset;
              
            if(Phase1 == true)
            {
                Vector2 toPlayer = playerPosition - bugCenter;

                if (toPlayer != Vector2.Zero)
                    toPlayer.Normalize();

                Position += toPlayer * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                BugFlipped = toPlayer.X > 0;
                bounds.Center = bugCenter;
            }
            else if (Phase2 == true)
            {
                Vector2 bugCenter1 = Position + new Vector2(130, 120);
                Vector2 toCenter = screenCenter - bugCenter1;

                float distance = toCenter.Length();

                if (!reachedCenter)
                {
                    if (distance > 5f)
                    {
                        toCenter.Normalize();
                        Position += toCenter * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    }
                    else
                    {                      
                        reachedCenter = true;

                        // Reset attack timers once
                        phase2Timer = 0f;
                        shootTimer = 0f;
                        phase2State = Phase2State.Attacking;
                    }

                    bounds.Center = new Vector2(100000,100000);
                    return; 
                }

                // ATTACK LOGIC
                bounds.Center = bugCenter1;

                phase2Timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (phase2State == Phase2State.Attacking)
                {
                    IsAttacking = true;

                    shootTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (shootTimer >= shootInterval)
                    {
                        ShootStar();
                        shootTimer = 0f;
                    }

                    if (phase2Timer >= 5f)
                    {
                        phase2Timer = 0f;
                        phase2State = Phase2State.Open;
                    }
                }
                else
                {
                    IsAttacking = false;

                    if (phase2Timer >= 5f)
                    {
                        phase2Timer = 0f;
                        phase2State = Phase2State.Attacking;
                    }
                }
            }
            else if (Phase3)
            {
                phase3Timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                Vector2 bossCenter3 = Position + new Vector2(130, 120);

                switch (phase3State)
                {
                    case Phase3State.Disappeared:
                        visible = false;
                        bounds.Center = new Vector2(100000, 100000);

                        phase3Timer = 0f;
                        phase3State = Phase3State.Targeting;
                        directionLocked = false;
                        break;
                    case Phase3State.Targeting:
                        if (!directionLocked)
                        {
                            // Spawn ABOVE the screen, aligned with player X
                            Position = new Vector2(
                                playerPosition.X - 130, 
                                -300                   
                            );

                            chargeDirection = Vector2.UnitY;

                            directionLocked = true;
                        }

                        if (phase3Timer >= 1.5f) // brief warning time
                        {
                            phase3Timer = 0f;
                            phase3State = Phase3State.Charging;
                        }
                        break;
                    case Phase3State.Charging:
                        visible = true;

                        Position += chargeDirection * chargeSpeed *
                                    (float)gameTime.ElapsedGameTime.TotalSeconds;

                        bounds.Center = Position + new Vector2(130, 190);

                        // Stop when off the bottom of the screen
                        if (Position.Y > 1800)
                        {
                            phase3Timer = 0f;
                            chargeCount++;

                           
                            phase3State = Phase3State.Disappeared;
                            
                        }
                        break;
                }
            }
        }


        /// <summary>
        /// Draws the sprite
        /// </summary>
        /// <param name="gameTime">the game time</param>
        /// <param name="spriteBatch">the sprite batch to render with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!visible)
                return;

            SpriteEffects spriteEffect = BugFlipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if (Phase1 == true)
            {
                if (animationTimer > 0.2)
                {
                    animationFrame++;
                    if (animationFrame > 4) animationFrame = 0;
                    animationTimer -= 0.2;
                }
            }
            else if (Phase2 == true)
            {
                if (animationTimer > 0.2)
                {
                    animationFrame++;
                    if (animationFrame > 8) animationFrame = 5;
                    animationTimer -= 0.2;
                }
            }
            else if (Phase3 == true)
            {
                if (animationTimer > 0.2)
                {
                    animationFrame++;
                    if (animationFrame > 9) animationFrame = 9;
                    animationTimer -= 0.2;
                }
            }

            var source = new Rectangle(animationFrame * 128, 0, 128, 128);
            spriteBatch.Draw(texture, Position, source, color, 0f, Vector2.Zero, 2f, spriteEffect, 0f);
            

        }

    }
}
