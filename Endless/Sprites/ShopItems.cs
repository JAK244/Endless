using System;
using System.Windows.Forms.Design;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Endless.Sprites
{
    /// <summary>
    /// the shopItmes base class
    /// </summary>
    public class ShopItems
    {
        /// <summary>
        /// the name of the items
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// the icon of the itmes
        /// </summary>
        public Texture2D Icon { get; set; }

        /// <summary>
        /// the description of the items
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// the Affct of the items
        /// </summary>
        public Action<TravelerSprite> ApplyEffect { get; set; }  // Applies upgrade to the player
    }
}

