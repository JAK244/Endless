using Endless.Managers;
using Endless.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SharpDX.Direct2D1;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;

namespace Endless.Screens
{
    /// <summary>
    /// win screen class
    /// </summary>
    public class WinScreen : GameScenes
    {
        private Texture2D background;
        private SpriteFont Doto;
        private List<string> menuItems;
        private int selectedIndex;
        private KeyboardState oldState;
        private GamePadState oldPadState;
        private bool ignoreInput = true;


        /// <summary>
        /// Uses content manager to load components
        /// </summary>
        /// <param name="Content">the content manager</param>
        public override void LoadContent(ContentManager Content)
        {

            Doto = Content.Load<SpriteFont>("Doto-Black");

            background = Content.Load<Texture2D>("WinScreen");

            //backGroundMusic = Content.Load<Song>("Synthwave Loop");
            //MediaPlayer.IsRepeating = true;
            //MediaPlayer.Play(backGroundMusic);              music                                                       
            menuItems = new List<string> { "Play again", "EXIT" };

            base.LoadContent(Content);
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
                return; 
            }

            if (IsKeyPressed(Keys.Left, keyboard) || IsKeyPressed(Keys.A, keyboard) ||
                (gamepad.DPad.Up == ButtonState.Pressed && oldPadState.DPad.Up == ButtonState.Released) ||
                (gamepad.ThumbSticks.Left.Y > 0.5f && oldPadState.ThumbSticks.Left.Y <= 0.5f))
            {
                selectedIndex = (selectedIndex - 1 + menuItems.Count) % menuItems.Count;
            }

            if (IsKeyPressed(Keys.Right, keyboard) || IsKeyPressed(Keys.D, keyboard) ||
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
                    SceneManager.Instance.ChangeScene(new MainGameScene());

                }
                else if (selectedIndex == 1)
                {
                    System.Environment.Exit(0);
                }
            }

            oldState = keyboard;
            oldPadState = gamepad;
        }

        /// <summary>
        /// returns true if the given key was just pressed this frame (edge detection)
        /// </summary>
        private bool IsKeyPressed(Keys key, KeyboardState current)
        {
            return current.IsKeyDown(key) && oldState.IsKeyUp(key);
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


            sb.DrawString(Doto, $"YOU", new Vector2(100, 0), Color.Black);
            sb.DrawString(Doto, $"WIN!!!", new Vector2(900, 0), Color.Black);


            Vector2 pos = new Vector2(10, 320);


            for (int i = 0; i < menuItems.Count; i++)
            {
                var text = menuItems[i];
                var color = (i == selectedIndex) ? Color.Blue : Color.Black;

                if (i == selectedIndex)
                {
                    sb.DrawString(Doto, "> " + text, pos, color);
                }
                else
                {
                    sb.DrawString(Doto, text, pos, color);
                }

                pos.X += 900f; // offset
            }

            sb.End();
        }

    }
}
