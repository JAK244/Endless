using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Endless.Managers;
using Endless.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Endless.Screens
{
    /// <summary>
    /// the main game screen class
    /// </summary>
    public class MainGameScene : GameScenes
    {
        
        private SpriteFont Doto;
        private TravelerSprite Traveler;
        private HelthSprite[] healths;
        private PortalSprite[] portals;
        private PowerBallSprite powerBall;
        private List<Bug1Sprite> bugs;
        private ArmSprite arm;
        private int healthLeft;
        private List<BulletSprite> bullets = new List<BulletSprite>();
        private Song backGroundMusic_nonC;
        private Song backGroundMusic_InC;
        private double damageCooldown = 0;
        private bool showInteractPrompt = false;
        public WaveManager waveManager;
        private Texture2D ball;
        private Map map;
        private float rotation;
        private float shakeDuration = 0f;
        private float shakeMagnitude = 5f;
        private Random random = new Random();
        private Vector2 shakeOffset = Vector2.Zero;

        private PlayerInventory playerInventory;
        private KeyboardState previousKeyboardState;



        private Video video;
        private VideoPlayer vPlayer;
        private Texture2D videoTexture;
        private bool isPlaying = false;
        private Texture2D VideoBorder;
      
       


      

        /// <summary>
        /// the translation matrix
        /// </summary>
        public Matrix _translation;

        public void TriggerShake(float duration, float magnitude = 5f)
        {
            shakeDuration = duration;
            shakeMagnitude = magnitude;
        }

        private void HandleWaveStart(int wave)
        {
            waveManager.ShowWaveMessage($"Wave {wave} Start!");
            MediaPlayer.Stop();
            MediaPlayer.Play(backGroundMusic_InC);
        }

        private void HandleWaveEnd(int wave)
        {
            waveManager.ShowWaveMessage($"Wave {wave} Complete!");
            MediaPlayer.Stop();
            MediaPlayer.Play(backGroundMusic_nonC);
        }


        /// <summary>
        /// initializes the content for the scene
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            

            Traveler = new TravelerSprite() { position = new Vector2(500, 780) };

            arm = new ArmSprite(Traveler);

            map = new Map();
            

            portals = new PortalSprite[]
            {
                new PortalSprite(){Position = new Vector2(0,725), PortalFlipped = true}, // top
                new PortalSprite(){Position = new Vector2(1450, 725)}, // bottom
                new PortalSprite(){Position = new Vector2(725,0), PortialTB = true}, // right
                new PortalSprite(){Position = new Vector2(725,1450), PortialTB = true}, // left
            };

            powerBall = new PowerBallSprite() { Position = new Vector2(725, 725) }; //1450, 1450 max screen size


            bugs = new List<Bug1Sprite>{};

            waveManager = new WaveManager(portals.ToList(), bugs, SceneManager.Instance.Content);
            waveManager.LoadContent(SceneManager.Instance.Content);
            waveManager.OnWaveStart += HandleWaveStart;
            waveManager.OnWaveEnd += HandleWaveEnd;

            healths = new HelthSprite[]
            {
                new HelthSprite(),
                new HelthSprite(),
                new HelthSprite(),
            };
            healthLeft = healths.Length;

            playerInventory = new PlayerInventory(new TeleportingControlItem());


        }

        private void CalculateTranslation()
        {
            var halfScreenW = SceneManager.Instance.Dimensions.X / 2;
            var halfScreenH = SceneManager.Instance.Dimensions.Y / 2;

            var dx = halfScreenW - Traveler.position.X;
            var dy = halfScreenH - Traveler.position.Y;

            // clamp against the pixel map size
            dx = MathHelper.Clamp(dx, -(map.PixelWidth - SceneManager.Instance.Dimensions.X), 0);
            dy = MathHelper.Clamp(dy, -(map.PixelHeight - SceneManager.Instance.Dimensions.Y), 0);

            _translation = Matrix.CreateTranslation(dx, dy, 0f);
            SceneManager.Instance.CurrentTranslation = _translation;
        }

        /// <summary>
        /// loads the content for the scene using a content manager
        /// </summary>
        /// <param name="Content">the content manager</param>
        public override void LoadContent(ContentManager Content)
        {
            base.LoadContent(Content);
            ball = Content.Load<Texture2D>("Ball");
            Traveler.LoadContent(Content);
            arm.LoadContent(Content);
            map.LoadContent(Content);
            Traveler.SetBounds(new Point(50, 49), new Point(16, 16));
            arm.SetBounds(new Point(50, 49), new Point(16, 16));
            foreach (var portal in portals) portal.LoadContent(Content);
            foreach (var bug in bugs) bug.LoadContent(Content);   
            foreach (var helth in healths) helth.LoadContent(Content);
            powerBall.LoadContent(Content);
            VideoBorder = Content.Load<Texture2D>("VideoBorderFinal");

            video = Content.Load<Video>("MEMEThoughts2");
            vPlayer = new VideoPlayer();
           
            //vPlayer.Play(video);
            isPlaying = true;
            
            




                rotation = 0.0f;
            Doto = Content.Load<SpriteFont>("Doto-Black");

            backGroundMusic_nonC = Content.Load<Song>("if Anyone Dies (instrumental)");
            backGroundMusic_InC = content.Load<Song>("Arena Theme");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(backGroundMusic_nonC);

            
        }

        /// <summary>
        /// unloads the content from the scene
        /// </summary>
        public override void UnloadContent()
        {
            base.UnloadContent();

            try
            {
                if (vPlayer != null)
                {
                    if (vPlayer.State != MediaState.Stopped)
                        vPlayer.Stop();

                    vPlayer.Dispose();
                    
                }

                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Video cleanup failed: {ex.Message}");
            }
        }

        /// <summary>
        /// updates the content using game time
        /// </summary>
        /// <param name="gameTime">the game time</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            KeyboardState currentKeyboardState = Keyboard.GetState();

            if (Keyboard.GetState().IsKeyDown(Keys.U))
            {
                SceneManager.Instance.AddScene(new PauseScene());
               
            }

            Traveler.color = Color.White;

            CalculateTranslation();

            Traveler.Update(gameTime);
            
            arm.Update(gameTime);

            damageCooldown -= gameTime.ElapsedGameTime.TotalSeconds;

            //bugs interacting with player
            foreach (var bug in bugs)
            {
                bug.Update(gameTime, Traveler.position);

                if (damageCooldown <= 0 && bug.Bounds.CollidesWith(Traveler.Bounds))
                {
                    for (int i = 0; i < healths.Length; i++)
                    {
                        if (!healths[i].Damaged)
                        {
                            Traveler.color = Color.Red;
                            healths[i].Damaged = true;
                            healthLeft--;
                            damageCooldown = 1.0;
                            TriggerShake(0.3f, 8f);
                            break;
                        }
                    }
                }
            }

            // bullets interacting with bugs
            foreach (var bullet in arm.Bullets.ToList())
            {
                foreach (var bug in bugs.ToList())
                {
                    if (bullet.Bounds.CollidesWith(bug.Bounds))
                    {
                        bullet.IsRemoved = true;
                        bug.IsAlive = false; 
                    }
                }
            }

            // traveler interacting with powerball
            waveManager.Update(gameTime);

            if (Traveler.Bounds.CollidesWith(powerBall.Bounds))
            {
                showInteractPrompt = true;

                if (Keyboard.GetState().IsKeyDown(Keys.F) && !waveManager.WaveActive)
                {
                    waveManager.StartWave(waveManager.CurrentWave + 1);
                }
            }
            else
            {
                showInteractPrompt = false;
            }

            // my screen shake
            if (shakeDuration > 0)
            {
                shakeDuration -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                // Random offset within magnitude range
                shakeOffset = new Vector2(
                    (float)(random.NextDouble() * 2 - 1) * shakeMagnitude,
                    (float)(random.NextDouble() * 2 - 1) * shakeMagnitude
                );

                if (shakeDuration <= 0)
                    shakeOffset = Vector2.Zero;
            }


            if (isPlaying && vPlayer.State == MediaState.Stopped)
            {
                //vPlayer.Play(video); // restart when finished
            }

            if (currentKeyboardState.IsKeyDown(Keys.E) && !previousKeyboardState.IsKeyDown(Keys.E))
            {
                playerInventory.UseItem(Traveler);
                
            }

            // Save state for next frame
            previousKeyboardState = currentKeyboardState;



        }


        /// <summary>
        /// draws the content on screen
        /// </summary>
        /// <param name="gameTime">the game time</param>
        public override void Draw(GameTime gameTime)
        {
            var sb = SceneManager.Instance.SpriteBatch;

            

            Matrix shakeTransform = _translation * Matrix.CreateTranslation(shakeOffset.X, shakeOffset.Y, 0);
            sb.Begin(transformMatrix: shakeTransform, samplerState: SamplerState.PointClamp);

            map.Draw(sb);

            powerBall.Draw(gameTime, sb);
            //var rec3 = new Rectangle((int)powerBall.Bounds.X,(int)powerBall.Bounds.Y,(int)powerBall.Bounds.Width,(int)powerBall.Bounds.Height);
            //sb.Draw(ball, rec3, Color.White);

            if (showInteractPrompt)
            {
                Vector2 textPos = powerBall.Position + new Vector2(40, -80);
                sb.DrawString(Doto, "F", textPos, Color.Black);
            }

            foreach (var portal in portals)
                portal.Draw(gameTime, sb);
            
            
            foreach (var bug in bugs)
            {
                bug.Draw(gameTime, sb);
                //var rec = new Rectangle((int)(bug.Bounds.Center.X - bug.Bounds.Radius), (int)(bug.Bounds.Center.Y - bug.Bounds.Radius), (int)bug.Bounds.Radius * 2, (int)bug.Bounds.Radius * 2);
                //sb.Draw(ball, rec, Color.White);
            }

            foreach (var bullet in bullets)
                bullet.Draw(gameTime, sb);


            //var rec2 = new Rectangle((int)Traveler.Bounds.X,(int)Traveler.Bounds.Y,(int)Traveler.Bounds.Width,(int)Traveler.Bounds.Height);
            //sb.Draw(ball, rec2, Color.White);

            arm.Draw(gameTime, sb);
            Traveler.Draw(gameTime, sb);


            sb.End();


            // handles the UI
            sb.Begin(samplerState: SamplerState.PointClamp);

            int spacing = 70; // pixels between hearts
            int startX = 10; 
            int totalHearts = healths.Length;

            for (int i = 0; i < totalHearts; i++)
            {
                int heartIndex = totalHearts - 1 - i;

                if (!healths[heartIndex].Damaged)
                {
                    Vector2 heartPos = new Vector2(startX + (i * spacing), 10);

                    healths[heartIndex].position = heartPos;
                    healths[heartIndex].Draw(gameTime, sb);
                }
            }

            waveManager.Draw(gameTime, sb);

            sb.End();


            if (isPlaying && vPlayer.State == MediaState.Playing)
            {
                videoTexture = vPlayer.GetTexture();
                if (videoTexture != null)
                {
                    sb.Begin();
                    sb.Draw(videoTexture, new Rectangle(1030, 20, 150, 150), Color.White); // the video 
                    sb.Draw(VideoBorder, new Rectangle(1030, 20, 150, 150), Color.White); // the video border
                    sb.End();
                }
                

            }

            base.Draw(gameTime);
        }
    }
}
