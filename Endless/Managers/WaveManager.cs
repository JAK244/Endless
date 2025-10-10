using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private double spawnTimer;
        private int enemiesToSpawn;

        /// <summary>
        /// checks if there is an active wave
        /// </summary>
        public bool WaveActive { get; private set; }

        /// <summary>
        /// gets or sets the current wave
        /// </summary>
        public int CurrentWave {  get; private set; }

        private List<PortalSprite> portals;
        private List<Bug1Sprite> bugs;

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
        public WaveManager(List<PortalSprite> portals, List<Bug1Sprite> bugs, ContentManager content)
        {
            this.portals = portals;
            this.bugs = bugs;
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
            enemiesToSpawn = 10 + (waveNumber - 1) * 5; // increments each round by 5
            spawnTimer = 0;

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

            if (spawnTimer >= 1.0 && enemiesToSpawn > 0)
            {
                SpawnBug();
                enemiesToSpawn--;
                spawnTimer = 0;
            }

            if (enemiesToSpawn == 0 && bugs.All(b => !b.IsAlive))
            {
                WaveActive = false;
                OnWaveEnd?.Invoke(CurrentWave);
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

            var newBug = new Bug1Sprite(spawnPos)
            {
                IsAlive = true,
                BugFlipped = ran.Next(2) == 0
            };
            newBug.LoadContent(content);
            bugs.Add(newBug);
        }
    }

}
