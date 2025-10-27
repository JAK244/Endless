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

        private double animationTimer;
        private short animationFrame;


        public TeleportingControlItem(Texture2D icon)
        {
            Name = "Teleport Controller"; 
            Icon = icon;
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
                savedPosition = null; // reset
            }
        }

        

        public override void Draw(GameTime gameTime, SpriteBatch sb)
        {
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if (animationTimer > 0.2)
            {
                animationFrame++;
                if (animationFrame > 6) animationFrame = 0;
                animationTimer -= 0.2;
            }


            var source = new Rectangle(animationFrame * 32, 0, 32, 32);
            sb.Draw(Icon, new Vector2(210, 10), source, Color.White,0f,Vector2.Zero,2f,SpriteEffects.None,0f);
            
        }

        

        
       
    }
}
