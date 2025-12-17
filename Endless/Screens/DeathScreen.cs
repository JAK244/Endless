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
    /// the death Screen class
    /// </summary>
    public class DeathScreen : GameScenes
    {
        private Texture2D background;
        private Texture2D deadDuck;
        private SpriteFont Doto;
        private List<string> menuItems;
        private int selectedIndex;
        private KeyboardState oldState;
        private GamePadState oldPadState;
        private bool ignoreInput = true;
        private double animationTimer;
        private short animationFrame;


        /// <summary>
        /// Uses content manager to load components
        /// </summary>
        /// <param name="Content">the content manager</param>
        public override void LoadContent(ContentManager Content)
        {

            Doto = Content.Load<SpriteFont>("Doto-Black");
        
            background = Content.Load<Texture2D>("DEATHScreen");
            deadDuck = Content.Load<Texture2D>("DeadDuck");

            menuItems = new List<string> { "Retry", "MainMenu"};

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
                    SceneManager.Instance.AddScene(new TitleScene());
                }
                else if (selectedIndex == 2)
                {
                    Environment.Exit(0);
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


            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

            
            if (animationTimer > 0.2)
            {
                animationFrame++;
                if (animationFrame > 4) animationFrame = 0;
                animationTimer -= 0.2;
            }

            var source = new Rectangle(animationFrame * 64, 0, 64, 64);
            sb.Draw(deadDuck, new Vector2(450,150), source, Color.White, 0f, Vector2.Zero, 5f, SpriteEffects.None, 0f);


            sb.DrawString(Doto, $"LAMO", new Vector2(100, 0), Color.White);
            sb.DrawString(Doto, $"COOKED", new Vector2(800, 0), Color.White);
            

            Vector2 pos = new Vector2(150, 580);


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

                pos.X += 500f;
            }

            sb.End();
        }



    }
}
