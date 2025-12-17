using System;
using System.Collections.Generic;
using Endless.Managers;
using Endless.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Endless.Screens
{
    /// <summary>
    /// the PauseScreen Class
    /// </summary>
    public class PauseScene : GameScenes
    {
        private SpriteFont Doto;
        private List<string> menuItems;
        private int selectedIndex;
        private KeyboardState oldState;
        private GamePadState oldPadState;

        /// <summary>
        /// Loads the content using a contentManager
        /// </summary>
        /// <param name="content">the contentManager</param>
        public override void LoadContent(ContentManager content)
        {
            Doto = content.Load<SpriteFont>("Doto-Black");
            menuItems = new List<string> { "Resume", "Exit Game" };
        }

        /// <summary>
        /// Updates the screen using gameTime
        /// </summary>
        /// <param name="game">the gameTime</param>
        public override void Update(GameTime game)
        {
            var keyboard = Keyboard.GetState();
            var gamepad = GamePad.GetState(0);

            if (IsKeyPressed(Keys.Up, keyboard) ||
                (gamepad.DPad.Up == ButtonState.Pressed && oldPadState.DPad.Up == ButtonState.Released) ||
                (gamepad.ThumbSticks.Left.Y > 0.5f && oldPadState.ThumbSticks.Left.Y <= 0.5f))
            {
                selectedIndex = (selectedIndex - 1 + menuItems.Count) % menuItems.Count;
            }

            if (IsKeyPressed(Keys.Down, keyboard) ||
                (gamepad.DPad.Down == ButtonState.Pressed && oldPadState.DPad.Down == ButtonState.Released) ||
                (gamepad.ThumbSticks.Left.Y < -0.5f && oldPadState.ThumbSticks.Left.Y >= -0.5f))
            {
                selectedIndex = (selectedIndex + 1) % menuItems.Count;
            }

            if (IsKeyPressed(Keys.Enter, keyboard) ||
                (gamepad.Buttons.A == ButtonState.Pressed && oldPadState.Buttons.A == ButtonState.Released))
            {
                if (selectedIndex == 0) // resumes game
                {
                    
                    SceneManager.Instance.RemoveScene();
                }
                else if (selectedIndex == 1) // exit
                {

                    System.Environment.Exit(0);
                }
            }

            oldState = keyboard;
            oldPadState = gamepad;
        }

        /// <summary>
        /// Draws the pause Screen
        /// </summary>
        /// <param name="game">the gameTime</param>
        public override void Draw(GameTime game)
        {
            var sb = SceneManager.Instance.SpriteBatch;
            sb.Begin();

                Vector2 pos = new Vector2(0, 0);
                
                for (int i = 0; i < menuItems.Count; i++)
                {
                    var text = menuItems[i];
                    var color = (i == selectedIndex) ? Color.Gold : Color.White;

                    // draw selection mark for clarity
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
