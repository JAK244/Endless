using Endless.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Endless.Sprites
{
    /// <summary>
    /// the tilemap sprite class
    /// </summary>
    public class TileMapSprites
    {
        private Texture2D texture;

        /// <summary>
        /// the sprites position
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// the sprites origin
        /// </summary>
        public Vector2 Origin;

        /// <summary>
        /// TileMapSprites constructor
        /// </summary>
        /// <param name="texture">the texture</param>
        /// <param name="position">the position</param>
        public TileMapSprites(Texture2D texture,Vector2 position)
        {
            this.texture = texture;
            Position = position;
            Origin = new(texture.Width/2, texture.Height/2);
        }


        /// <summary>
        /// draws the tilemap sprites
        /// </summary>
        public void Draw()
        {
            var sb = SceneManager.Instance.SpriteBatch;
            sb.Draw(texture, Position, null, Color.White, 0f, Origin, 1f, SpriteEffects.None, 0f);
        }
    }
}
