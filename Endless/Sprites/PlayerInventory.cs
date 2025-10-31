using Endless.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Endless.Sprites
{
    /// <summary>
    /// a class that handles players inventory
    /// </summary>
    public class PlayerInventory
    {
        /// <summary>
        /// the item from the base class
        /// </summary>
        public ItemBase Item { get; private set; }

        /// <summary>
        /// the texture
        /// </summary>
        public Texture2D texture { get; private set; }

        /// <summary>
        /// playerinventory constructor
        /// </summary>
        /// <param name="startingItem">the starting itme</param>
        public PlayerInventory(ItemBase startingItem)
        {
            Item = startingItem;
        }

        /// <summary>
        /// the class that uses the item
        /// </summary>
        /// <param name="player">the player</param>
        /// <param name="textManager">the textmanger</param>
        public void UseItem(TravelerSprite player, TextMessageManager textManager = null)
        {
            Item?.Use(player, textManager);
        }

        /// <summary>
        /// this Draws the player invintory
        /// </summary>
        /// <param name="gameTime">the game time</param>
        /// <param name="spriteBatch">the sprite batch</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Item?.Icon == null)
                return;

            Vector2 iconPosition = new Vector2(200, 10);

            Item.Draw(gameTime, spriteBatch);
        }
    }
}
