using Endless.BaseClasses;
using Endless.Managers;
using Endless.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Endless.Screens
{
    /// <summary>
    /// the ShopScreen Class
    /// </summary>
    public class ShopScreen : GameScenes
    {
        private SpriteFont Doto;
        private List<ShopItems> allItems;
        private List<ShopItems> currentItems;
        private int selectIndex;
        private KeyboardState oldState;
        private GamePadState oldPadState;
        private Texture2D backGround;
        private Random random = new Random();

        private TravelerSprite player;

        /// <summary>
        /// the constructor fo the shopScreen
        /// </summary>
        /// <param name="player">the player</param>
        public ShopScreen(TravelerSprite player)
        {
            this.player = player;
        }


        /// <summary>
        /// Loads content using a contentManager
        /// </summary>
        /// <param name="content">the contentManager</param>
        public override void LoadContent(ContentManager content)
        {
            Doto = content.Load<SpriteFont>("Doto-Black");
            //backGround = content.Load<SpriteFont>( finish this later );

            allItems = new List<ShopItems>()
            {
                new ShopItems
                {
                    Name = "Hot sauce",
                    Icon = content.Load<Texture2D>("TelaItem"),
                    Description = "Fire up your chicken and incresse your speed",
                    ApplyEffect = (player) => player.SpeedMultiplier += 0.20f
                },

                new ShopItems
                {
                    Name = "Heart Container",
                    Icon = content.Load<Texture2D>("TelaItem"),
                    Description = "Gain an extra heart",
                    ApplyEffect = (player) => player.healthWentUp = true
                },

                new ShopItems
                {
                    Name = "Postition Device",
                    Icon = content.Load<Texture2D>("TelaItem"),
                    Description = "Save your location once then teleport back"

                },
            };

            GenerateRandomItems();


        }

        /// <summary>
        /// Gets 3 Random Items from the Shop
        /// </summary>
        private void GenerateRandomItems()
        {
            currentItems = new List<ShopItems>();

            var copy = new List<ShopItems>(allItems);
            for (int i = 0; i < 3; i++)
            {
                int index = random.Next(copy.Count);
                currentItems.Add(copy[index]);
                copy.RemoveAt(index);
            }
        }

        /// <summary>
        /// Updates the Items using gameTime
        /// </summary>
        /// <param name="gameTime">the GameTime</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Left) && oldState.IsKeyUp(Keys.Left))
                selectIndex = Math.Max(0, selectIndex - 1);
            if (state.IsKeyDown(Keys.Right) && oldState.IsKeyUp(Keys.Right))
                selectIndex = Math.Min(currentItems.Count - 1, selectIndex + 1);

            // Buy item
            if (state.IsKeyDown(Keys.Enter) && oldState.IsKeyUp(Keys.Enter))
            {
                var selected = currentItems[selectIndex];
                if (selected.ApplyEffect != null)
                {
                    selected.ApplyEffect(player);
                    System.Diagnostics.Debug.WriteLine(
                        $"Applied {selected.Name}! Speed={player.SpeedMultiplier}, Health={player.MaxHelth}");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"Item {selected.Name} has no ApplyEffect!");
                } // Apply effect permanently
                SceneManager.Instance.RemoveScene(); // or return to gameplay
            }

            oldState = state;

        }

        /// <summary>
        /// Draws the shopScreen
        /// </summary>
        /// <param name="gameTime">the GameTime</param>
        public override void Draw(GameTime gameTime)
        {
            var sb = SceneManager.Instance.SpriteBatch;
            sb.Begin();

            if (backGround != null)
                sb.Draw(backGround, Vector2.Zero, Color.White);

            sb.DrawString(Doto, "SHOP - Choose Your Upgrade",
                new Vector2(600, 40), Color.White, 0f,
                new Vector2(Doto.MeasureString("SHOP - Choose Your Upgrade").X / 2, 0), 0.6f,
                SpriteEffects.None, 0f);

            int screenWidth = 1200;
            int itemCount = currentItems.Count;
            int cardWidth = screenWidth / 3; 
            int cardCenterOffset = cardWidth / 2;

            for (int i = 0; i < currentItems.Count; i++)
            {
                var item = currentItems[i];
                // Center each card horizontally
                float centerX = i * cardWidth + cardCenterOffset;
                float startY = 200;

                Color color = (i == selectIndex) ? Color.Yellow : Color.White;

                // Draw Name centered
                string wrapped1 = WrapText(Doto, item.Name, cardWidth - 60, 0.3f);
                Vector2 nameSize = Doto.MeasureString(item.Name) * 0.5f;
                sb.DrawString(Doto, wrapped1,
                    new Vector2(centerX - nameSize.X / 2, startY),
                    color, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f);

                // Icon centered
                if (item.Icon != null)
                {
                    Vector2 iconPos = new Vector2(centerX - (item.Icon.Width / 2), startY + 50);
                    sb.Draw(item.Icon, iconPos, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                }

                // Description wrapped and centered
                string wrapped = WrapText(Doto, item.Description, cardWidth - 60, 0.3f);
                sb.DrawString(Doto, wrapped,new Vector2(centerX - (cardWidth / 2) + 30, startY + 150),
                    Color.LightGray, 0f, Vector2.Zero, 0.3f, SpriteEffects.None, 0f);
            }

            sb.DrawString(Doto, "Use LEFT/RIGHT to select, ENTER to buy",
                new Vector2(600, 670), Color.White, 0f,
                new Vector2(Doto.MeasureString("Use LEFT/RIGHT to select, ENTER to buy").X / 2, 0), 0.5f,
                SpriteEffects.None, 0f);

            sb.End();
        }

        /// <summary>
        /// Wraps the Text that goes off screen
        /// </summary>
        /// <param name="font">the font</param>
        /// <param name="text">the text</param>
        /// <param name="maxLineWidth"> the max width</param>
        /// <param name="scale">the scale</param>
        /// <returns>the wrapped string</returns>
        private string WrapText(SpriteFont font, string text, float maxLineWidth, float scale)
        {
            string[] words = text.Split(' ');
            StringBuilder wrapped = new StringBuilder();
            float lineWidth = 0f;
            float spaceWidth = font.MeasureString(" ").X * scale;

            foreach (string word in words)
            {
                Vector2 size = font.MeasureString(word) * scale;
                if (lineWidth + size.X < maxLineWidth)
                {
                    wrapped.Append(word + " ");
                    lineWidth += size.X + spaceWidth;
                }
                else
                {
                    wrapped.Append("\n" + word + " ");
                    lineWidth = size.X + spaceWidth;
                }
            }
            return wrapped.ToString();
        }

    }

}

