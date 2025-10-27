using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Endless.Sprites
{
    public class TeleportingControlItem : ItemBase
    {
        private Vector2? savedPosition = null;

        public TeleportingControlItem()
        {
            Name = "Teleport Controller"; 
        }

        public override void Use(TravelerSprite player)
        {
            if(savedPosition == null)
            {
                savedPosition = player.position;
                Debug.WriteLine("Position Saved!");
            }
            else
            {
                player.position = savedPosition.Value;
                Debug.WriteLine("Position reversed!");
                savedPosition = null; // reset, can be reused if desired
            }
        }

        

        
       
    }
}
