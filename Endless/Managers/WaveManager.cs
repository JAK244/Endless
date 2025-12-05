using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Endless.Sprites;

namespace Endless.Managers
{
    /// <summary>
    /// the waveManager Class
    /// </summary>
    public class WaveManager
    {
        private Random ran = new Random();
        private SpriteFont Doto;
        private string waveMessage = "";
        private float messageAlpha = 0f;
        private double messageTimer = 0.0;
        private bool showingMessage = false;
        private int totalEnemiesRemaining;

        private double spawnTimer;
        private int enemiesToSpawn;
        private List<EnemyFire> enemyFires;
        private List<Ooze> oozes = new List<Ooze>();

        private int bug1ToSpawn;
        private int bug2ToSpawn;
        private int bug3ToSpawn;




        /// <summary>
        /// checks if there is an active wave
        /// </summary>
        public bool WaveActive { get; private set; }

        /// <summary>
        /// gets or sets the current wave
        /// </summary>
        public int CurrentWave {  get; private set; }

        private List<PortalSprite> portals;
        private List<Bug1Sprite> bug1;
        private List<Bug2> bug2;
        private List<Bug3> bug3;

        private ContentManager content;

        /// <summary>
        /// starts the wave
        /// </summary>
        public event Action<int> OnWaveStart;

        /// <summary>
        /// ends the wave
        /// </summary>
        public event Action<int> OnWaveEnd;

        /// <summary>
        /// the waveManager constructor
        /// </summary>
        /// <param name="portals">the portals</param>
        /// <param name="bugs">the bugs</param>
        /// <param name="content">the contnet manager</param>
        public WaveManager(List<PortalSprite> portals, List<EnemyFire> enemyFires, List<Bug1Sprite> bug1, List<Bug2> bug2, List<Bug3> bug3, ContentManager content)
        {
            this.portals = portals;
            this.enemyFires = enemyFires;
            this.bug1 = bug1;
            this.bug2 = bug2;
            this.bug3 = bug3;
            this.content = content;
        }

        /// <summary>
        /// Loads content using a contentManager
        /// </summary>
        /// <param name="Content">the content manager</param>
        public void LoadContent(ContentManager Content)
        {
            Doto = Content.Load<SpriteFont>("Doto-Black");
        }

        /// <summary>
        /// initalizes and shows the wave message
        /// </summary>
        /// <param name="message">the given message</param>
        public void ShowWaveMessage(string message)
        {
            waveMessage = message;
            messageAlpha = 0f;
            messageTimer = 0.0;
            showingMessage = true;
        }

        /// <summary>
        /// initalizes and starts the wave
        /// </summary>
        /// <param name="waveNumber">the waveNumber</param>
        public void StartWave(int waveNumber)
        {
            CurrentWave = waveNumber;
            WaveActive = true;
            spawnTimer = 0;

            // Reset counters
            bug1ToSpawn = 0;
            bug2ToSpawn = 0;
            bug3ToSpawn = 0;

            // Set enemies based on wave
            switch (waveNumber)
            {
                case 1:
                    bug1ToSpawn = 10;
                    break;
                case 2:
                    bug1ToSpawn = 15;
                    break;
                case 3:
                    bug1ToSpawn = 15;
                    bug3ToSpawn = 2;
                    break;
                case 4:
                    bug1ToSpawn = 20;
                    bug3ToSpawn = 4;
                    break;
                case 5:
                    bug1ToSpawn = 20;
                    bug3ToSpawn = 2;
                    bug2ToSpawn = 1;
                    break;
                case 6:
                    bug1ToSpawn = 25;
                    bug3ToSpawn = 3;
                    bug2ToSpawn = 1;
                    break;
                case 7:
                    bug1ToSpawn = 25;
                    bug3ToSpawn = 6;
                    bug2ToSpawn = 2;
                    break;
                case 8:
                    bug1ToSpawn = 30;
                    bug3ToSpawn = 6;
                    bug2ToSpawn = 3;
                    break;
                case 9:
                    bug1ToSpawn = 40;
                    bug3ToSpawn = 8;
                    bug2ToSpawn = 5;
                    break;
            }

            // Total enemies for speed scaling
            totalEnemiesRemaining = bug1ToSpawn + bug2ToSpawn + bug3ToSpawn;

            OnWaveStart?.Invoke(CurrentWave);
        }

        /// <summary>
        /// updates the waveManager using gameTime
        /// </summary>
        /// <param name="gameTime">the gameTime</param>
        public void Update(GameTime gameTime)
        {
            if (showingMessage)
            {
                messageTimer += gameTime.ElapsedGameTime.TotalSeconds;

                if (messageTimer < 1)
                    messageAlpha = (float)(messageTimer / 1.0);
                else if (messageTimer < 2)
                    messageAlpha = 1f; // hold
                else if (messageTimer < 3)
                    messageAlpha = 1f - (float)((messageTimer - 2) / 1.0);
                else
                    showingMessage = false;
            }

            if (!WaveActive)
                return;

            spawnTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if (spawnTimer >= 1.0 && (bug1ToSpawn > 0 || bug2ToSpawn > 0 || bug3ToSpawn > 0))
            {
                SpawnBug();
                spawnTimer = 0;
            }

            if (bug1ToSpawn == 0 && bug2ToSpawn == 0 && bug3ToSpawn == 0 && bug1.All(b => !b.IsAlive) && bug2.All(b => !b.IsAlive) && bug3.All(b => !b.IsAlive))
            {
                WaveActive = false;
                OnWaveEnd?.Invoke(CurrentWave);
            }

            if (WaveActive)
            {
                int aliveCount = bug1.Count(b => b.IsAlive) + bug2.Count(b => b.IsAlive) + bug3.Count(b => b.IsAlive);
                if (aliveCount > 0)
                {
                    float ratioAlive = (float)aliveCount / totalEnemiesRemaining;
                    float speedMultiplier = 1.0f;

                    if (ratioAlive <= 0.75f) speedMultiplier = 1.5f;
                    if (ratioAlive <= 0.50f) speedMultiplier = 2f;
                    if (ratioAlive <= 0.25f) speedMultiplier = 3f;
                    if (ratioAlive <= 0.10f) speedMultiplier = 3.5f;

                    foreach (var bug in bug1.Where(b => b.IsAlive))
                        bug.Speed = 60f * speedMultiplier;
                    foreach (var bug in bug2.Where(b => b.IsAlive))
                        bug.Speed = 60f * speedMultiplier;
                    foreach (var bug in bug3.Where(b => b.IsAlive))
                        bug.Speed = 60f * speedMultiplier;
                }
            }


            

        }

        /// <summary>
        /// Draws the message using a spriteBatch and GameTime
        /// </summary>
        /// <param name="gameTime">the game time</param>
        /// <param name="sb">the sprite batch</param>
        public void Draw(GameTime gameTime, SpriteBatch sb)
        {
            if (showingMessage && !string.IsNullOrEmpty(waveMessage))
            {
                var size = Doto.MeasureString(waveMessage);
                var pos = new Vector2((SceneManager.Instance.Dimensions.X - size.X) / 2, 100);

                sb.DrawString(Doto, waveMessage, pos, Color.White * messageAlpha);
            }
        }

        /// <summary>
        /// spwans the bugs from the portals position at random
        /// </summary>
        private void SpawnBug()
        {
            var portal = portals[ran.Next(portals.Count)];
            Vector2 spawnPos = portal.Position + new Vector2(ran.Next(-20, 20), ran.Next(-20, 20));

            if (bug1ToSpawn > 0)
            {
                var newBug = new Bug1Sprite(spawnPos)
                {
                    IsAlive = true,
                    BugFlipped = ran.Next(2) == 0
                };
                newBug.LoadContent(content);
                bug1.Add(newBug);
                bug1ToSpawn--;
            }
            else if (bug2ToSpawn > 0)
            {
                var newBug = new Bug2(spawnPos, enemyFires, content)
                {
                    IsAlive = true,
                    BugFlipped = ran.Next(2) == 0
                };
                newBug.LoadContent(content);
                bug2.Add(newBug);
                bug2ToSpawn--;
            }
            else if (bug3ToSpawn > 0)
            {
                var newBug = new Bug3(spawnPos)
                {
                    IsAlive = true,
                    BugFlipped = ran.Next(2) == 0
                };
                newBug.LoadContent(content);
                bug3.Add(newBug);
                bug3ToSpawn--;
            }
        }
    }

}
