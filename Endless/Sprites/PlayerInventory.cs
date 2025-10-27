using Endless.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Endless.Sprites
{
    public class PlayerInventory
    {
        public ItemBase Item { get; private set; }
        public Texture2D teture {  get; private set; }

        public PlayerInventory(ItemBase startingItem)
        {
            Item = startingItem;
        }

        public void UseItem(TravelerSprite player)
        {
            Item?.Use(player);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Item.Draw(gameTime, spriteBatch);

            // Optional: draw name or key hint
            //var font = SceneManager.Instance.Content.Load<SpriteFont>("Doto-Black");
            //spriteBatch.DrawString(font, Item.Name, iconPosition + new Vector2(60, 10), Color.White);
        }
    }
}
