using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Endless.Sprites
{
    public class ShopItems
    {
        public string Name { get; set; }
        public Texture2D Icon { get; set; }
        public string Description { get; set; }
        public Action<TravelerSprite> ApplyEffect { get; set; }  // Applies upgrade to the player
    }
}

