using Endless.Managers;
using Endless.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using static Endless.Screens.SettingScreen;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;

namespace Endless.Screens
{
    /// <summary>
    /// the title screen class
    /// </summary>
    public class TitleScene : GameScenes
    {
        private Texture2D background;
        private TravelerSprite traveler;
        private PortalSprite[] portals;
        private PowerBallSprite powerBall;
        private StarSprite[] stars;
        private Song backGroundMusic;
        private SpriteFont Doto;
        private List<string> menuItems;
        private int selectedIndex;
        private KeyboardState oldState;
        private GamePadState oldPadState;
        private bool ignoreInput = true;




        /// <summary>
        /// sets the inital state of the game
        /// </summary>
        public override void Initialize()
        {
            traveler = new TravelerSprite() { position = new Vector2(500, 624) };
            portals = new PortalSprite[]
            {
                new PortalSprite(){Position = new Vector2(1100,525)},// x, y
                new PortalSprite(){Position = new Vector2(-25 ,525), PortalFlipped = true},
            };

           
            powerBall = new PowerBallSprite() { Position = new Vector2(525, 561) };

            stars = new StarSprite[]
            {
                new StarSprite(){Position = new Vector2(260,80)},
                new StarSprite(){Position = new Vector2(800,80) },
            };
        }

        /// <summary>
        /// Uses content manager to load components
        /// </summary>
        /// <param name="Content">the content manager</param>
        public override void LoadContent(ContentManager Content)
        {
           
            Doto = Content.Load<SpriteFont>("Doto-Black");
            traveler.LoadContent(Content);
            background = Content.Load<Texture2D>("TitleScreen");
            foreach (var portal in portals) portal.LoadContent(Content);
            foreach (var star in stars) star.LoadContent(Content);
            powerBall.LoadContent(Content);
            backGroundMusic = Content.Load<Song>("Synthwave Loop");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(backGroundMusic);              //music                                                       
            menuItems = new List<string> { "Start Game", "Controls", "Settings", "Exit" };

            base.LoadContent(Content);
        }

        /// <summary>
        /// unloads loaded components
        /// </summary>
        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        /// <summary>
        /// updates the compontes using gametime
        /// </summary>
        /// <param name="gameTime">the game time</param>
        public override void Update(GameTime gameTime)
        {
            var keyboard = Keyboard.GetState();
            var gamepad = GamePad.GetState(0);

            if (ignoreInput)
            {
                oldState = keyboard;
                oldPadState = gamepad;
                ignoreInput = false;
                return; // skip update for 1 frame
            }

            if (IsKeyPressed(Keys.Up, keyboard) || IsKeyPressed(Keys.W, keyboard) || 
                (gamepad.DPad.Up == ButtonState.Pressed && oldPadState.DPad.Up == ButtonState.Released) ||
                (gamepad.ThumbSticks.Left.Y > 0.5f && oldPadState.ThumbSticks.Left.Y <= 0.5f))
            {
                selectedIndex = (selectedIndex - 1 + menuItems.Count) % menuItems.Count;
            }

            if (IsKeyPressed(Keys.Down, keyboard) || IsKeyPressed(Keys.S, keyboard) ||
                (gamepad.DPad.Down == ButtonState.Pressed && oldPadState.DPad.Down == ButtonState.Released) ||
                (gamepad.ThumbSticks.Left.Y < -0.5f && oldPadState.ThumbSticks.Left.Y >= -0.5f))
            {
                selectedIndex = (selectedIndex + 1) % menuItems.Count;
            }

            if (IsKeyPressed(Keys.Enter, keyboard) || IsKeyPressed(Keys.Space, keyboard) ||
                (gamepad.Buttons.A == ButtonState.Pressed && oldPadState.Buttons.A == ButtonState.Released))
            {
                if (selectedIndex == 0)
                {
                    SceneManager.Instance.AddScene(new MainGameScene()); // starts game
                }
                else if (selectedIndex == 1)
                {
                    SceneManager.Instance.AddScene(new ControllsScene()); // opens controlls screen
                }
                else if (selectedIndex == 2)
                {
                    SceneManager.Instance.ChangeScene(new SettingScreen(SettingsReturnMode.GoToTitle)); // opens Settings
                }
                else if (selectedIndex == 3)
                {
                    Environment.Exit(0); // exit
                }
            }

            oldState = keyboard;
            oldPadState = gamepad;
           
        }

        /// <summary>
        /// Draws the componets
        /// </summary>
        /// <param name="gameTime">the game time</param>
        public override void Draw(GameTime gameTime)
        {
            var sb = SceneManager.Instance.SpriteBatch;

            sb.Begin();
            if (background != null)
                sb.Draw(background, Vector2.Zero, Color.White);

            traveler.Draw(gameTime, sb);
            foreach (var portal in portals)
                portal.Draw(gameTime, sb);
            foreach (var star in stars)
                star.Draw(gameTime, sb);
            powerBall.Draw(gameTime, sb);


            sb.DrawString(Doto, $"Void", new Vector2(500, 0), Color.Black);
            sb.DrawString(Doto, $"Traveler", new Vector2(400, 70), Color.Black);

            Vector2 pos = new Vector2(400, 180);
            

            for (int i = 0; i < menuItems.Count; i++)
            {
                var text = menuItems[i];
                var color = (i == selectedIndex) ? Color.Gold : Color.White;

                if (i == selectedIndex)
                {
                    sb.DrawString(Doto, "> " + text, pos, color);
                }
                else
                {
                    sb.DrawString(Doto, text, pos, color);
                }

                pos.Y += 100f;
            }

            sb.End();
        }

        /// <summary>
        /// returns true if the given key was just pressed this frame (edge detection)
        /// </summary>
        private bool IsKeyPressed(Keys key, KeyboardState current)
        {
            return current.IsKeyDown(key) && oldState.IsKeyUp(key);
        }
    }
}
