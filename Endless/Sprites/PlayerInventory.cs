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
        public Texture2D teture { get; private set; }

        public PlayerInventory(ItemBase startingItem)
        {
            Item = startingItem;
        }

        // Accept optional manager and forward it to the item's Use
        public void UseItem(TravelerSprite player, TextMessageManager textManager = null)
        {
            Item?.Use(player, textManager);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Item?.Icon == null)
                return;

            // Ui Position
            Vector2 iconPosition = new Vector2(200, 10);

            // Draw using the Item's Draw method (which uses gameTime)
            Item.Draw(gameTime, spriteBatch);
        }
    }
}
