using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Endless.Sprites
{
    public enum PowerUpRarity
    {
        Common,
        Rare,
        Epic,
        Legendary
    }

    public abstract class PowerUpBase
    {
        public string Name { get; protected set; }
        public string Description { get; protected set; }
        public PowerUpRarity Rarity { get; protected set; }
        public Texture2D Icon { get; protected set; }

        //public abstract void ApplyEffect(TravelerSprite player);
        public virtual void Update(GameTime gameTime, TravelerSprite player)
        {
            //time limit buffs
        }

        public virtual void RemoveEffect(TravelerSprite player) 
        {
            // removing buffs
        }
    }
}
