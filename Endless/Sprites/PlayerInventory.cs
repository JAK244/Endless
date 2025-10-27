using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Endless.Sprites
{
    public class PlayerInventory
    {
        public ItemBase Item { get; private set; }

        public PlayerInventory(ItemBase startingItem)
        {
            Item = startingItem;
        }

        public void UseItem(TravelerSprite player)
        {
            Item?.Use(player);
        }
    }
}
