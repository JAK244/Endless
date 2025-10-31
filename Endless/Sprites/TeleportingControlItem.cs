using Endless.Managers;
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
        private Texture2D teleportMarker;
        private bool visibleMarker = false;
        private Vector2 markerPosition;

        public TeleportingControlItem(Texture2D icon, Texture2D marker)
        {
            Name = "Teleport Controller";
            Icon = icon;
            teleportMarker = marker;
        }

    
        public override void Use(TravelerSprite player, TextMessageManager textManager = null)
        {
            if (savedPosition == null)
            {
                savedPosition = player.position;
                markerPosition = savedPosition.Value;
                visibleMarker = true;

                Debug.WriteLine("Position Saved!");
                textManager?.Add("Saved", player.position - new Vector2(0, 40), Color.Yellow);
            }
            else
            {
                player.position = savedPosition.Value;
                visibleMarker = false;
                savedPosition = null;

                Debug.WriteLine("Position reversed!");
                textManager?.Add("Loaded", player.position - new Vector2(0, 40), Color.Cyan);
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
            sb.Draw(Icon, new Vector2(247, 11), source, Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);

        }

        public void DrawMarker(GameTime gameTime, SpriteBatch sb)
        {
            if (!visibleMarker) return;

            if (animationTimer > 0.2)
            {
                animationFrame++;
                if (animationFrame > 6) animationFrame = 0;
                animationTimer -= 0.2;
            }
            var source2 = new Rectangle(animationFrame * 64, 0, 64, 64);
            //float pulse = (float)(Math.Sin(gameTime.TotalGameTime.TotalSeconds * 4) * 0.25f + 1f);
            sb.Draw(
                teleportMarker,
                markerPosition,
                source2,
                Color.White * 0.8f,
                0f,
                Vector2.Zero, 
                1f, 
                SpriteEffects.None,
                0f
            );
        }
    }
}
