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
    public abstract class ItemBase
    {
        public string Name { get; protected set; }
        public Texture2D Icon { get; protected set; }


        public abstract void Use(TravelerSprite sprite, TextMessageManager textManager = null);

        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}
