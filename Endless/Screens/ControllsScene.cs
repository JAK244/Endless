using Endless.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Endless.Screens
{
    public class ControllsScene : GameScenes
    {
        private SpriteFont Doto;
        private List<string> menuItems;
        private int selectedIndex;
        private KeyboardState oldState;
        private GamePadState oldPadState;
        private Texture2D ControlImage;


        public override void LoadContent(ContentManager content)
        {
            Doto = content.Load<SpriteFont>("Doto-Black");
            menuItems = new List<string> { "Return"};
            ControlImage = content.Load<Texture2D>("ControllsPage");
        }

        public override void Update(GameTime game)
        {
            var keyboard = Keyboard.GetState();
            var gamepad = GamePad.GetState(0);

            if (IsKeyPressed(Keys.Up, keyboard))
                selectedIndex = (selectedIndex - 1 + menuItems.Count) % menuItems.Count;

            if (IsKeyPressed(Keys.Down, keyboard))
                selectedIndex = (selectedIndex + 1) % menuItems.Count;

            if (IsKeyPressed(Keys.Back, keyboard) || gamepad.Buttons.B == ButtonState.Pressed && oldPadState.Buttons.B == ButtonState.Released)
            {
                if (selectedIndex == 0) // start game
                {

                    SceneManager.Instance.ChangeScene(new TitleScene());
                }
            }

            oldState = keyboard;
            oldPadState = gamepad;
        }

        public override void Draw(GameTime game)
        {
            var sb = SceneManager.Instance.SpriteBatch;
            sb.Begin();

            sb.Draw(ControlImage,Vector2.Zero, Color.White);

            Vector2 pos = new Vector2(450, 600);

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
