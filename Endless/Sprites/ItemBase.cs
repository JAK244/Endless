using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Endless.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Endless.Sprites
{
    /// <summary>
    /// the base class for all items
    /// </summary>
    public abstract class ItemBase
    {
        /// <summary>
        /// the name of the item
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// the image of the item
        /// </summary>
        public Texture2D Icon { get; protected set; }

        /// <summary>
        /// class for using an itme
        /// </summary>
        /// <param name="sprite">the players sprite</param>
        /// <param name="textManager">the text manager</param>
        public abstract void Use(TravelerSprite sprite, TextMessageManager textManager = null);

        /// <summary>
        /// draws the item
        /// </summary>
        /// <param name="gameTime">the game time</param>
        /// <param name="spriteBatch">the sprite batch</param>
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}
