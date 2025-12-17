using Endless.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Endless.Screens
{
    public class SettingScreen : GameScenes
    {

        public enum SettingsReturnMode
        {
            PopScene,      // Pause menu
            GoToTitle      // Title screen
        }

        private SpriteFont Doto;
        private List<string> menuItems;
        private int selectedIndex;
        private KeyboardState oldState;
        private GamePadState oldPadState;
        private Texture2D settingsImage;

        private SettingsReturnMode returnMode;

        public SettingScreen(SettingsReturnMode mode)
        {
            returnMode = mode;
        }

        private void AdjustSetting(float amount)
        {
            if (selectedIndex == 0)
            {
                AudioSettings.SetMusicVolume(AudioSettings.MusicVolume + amount);
            }
            else if (selectedIndex == 1)
            {
                AudioSettings.SetSfxVolume(AudioSettings.SfxVolume + amount);
            }
        }


        /// <summary>
        /// Loads content using a contentManager
        /// </summary>
        /// <param name="content">the contentmanager</param>
        public override void LoadContent(ContentManager content)
        {
            Doto = content.Load<SpriteFont>("Doto-Black");
            menuItems = new List<string>
            {
                "Music Volume",
                "SFX Volume",
                "Return"
            };
            settingsImage = content.Load<Texture2D>("SettingsScreen");
        }


        /// <summary>
        /// updates the controll screen using gameTime
        /// </summary>
        /// <param name="game">the gameTime</param>
        public override void Update(GameTime game)
        {
            var keyboard = Keyboard.GetState();
            var gamepad = GamePad.GetState(0);

            if (IsKeyPressed(Keys.Up, keyboard) || IsKeyPressed(Keys.W, keyboard))
                selectedIndex = (selectedIndex - 1 + menuItems.Count) % menuItems.Count;

            if (IsKeyPressed(Keys.Down, keyboard) || IsKeyPressed(Keys.S, keyboard))
                selectedIndex = (selectedIndex + 1) % menuItems.Count;

            if (IsKeyPressed(Keys.Left, keyboard) || IsKeyPressed(Keys.A, keyboard))
            {
                AdjustSetting(-0.1f);
            }

            if (IsKeyPressed(Keys.Right, keyboard) || IsKeyPressed(Keys.D, keyboard))
            {
                AdjustSetting(0.1f);
            }

            if (IsKeyPressed(Keys.Space, keyboard) || gamepad.Buttons.B == ButtonState.Pressed && oldPadState.Buttons.B == ButtonState.Released)
            {
                if (selectedIndex == 2) 
                {

                    if (returnMode == SettingsReturnMode.PopScene)
                    {
                        SceneManager.Instance.RemoveScene();
                    }
                    else
                    {
                        SceneManager.Instance.ChangeScene(new TitleScene());
                    }

                }
            }

            oldState = keyboard;
            oldPadState = gamepad;
        }

        /// <summary>
        /// Draws the Controlls Screen using GameTime
        /// </summary>
        /// <param name="game">the GameTime</param>
        public override void Draw(GameTime game)
        {
            var sb = SceneManager.Instance.SpriteBatch;
            sb.Begin();

            sb.Draw(settingsImage, Vector2.Zero, Color.White);

            Vector2 startPos = new Vector2(50, 100);
            float verticalSpacing = 80f;

            for (int i = 0; i < menuItems.Count; i++)
            {
                Vector2 pos = startPos + new Vector2(0, i * verticalSpacing);

                string text = menuItems[i];

                if (i == 0)
                    text += $" : {(int)(AudioSettings.MusicVolume * 100)}%";
                else if (i == 1)
                    text += $" : {(int)(AudioSettings.SfxVolume * 100)}%";

                Color color = (i == selectedIndex) ? Color.Gold : Color.White;

                sb.DrawString(
                    Doto,
                    (i == selectedIndex ? "> " : "") + text,
                    pos,
                    color
                );
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
