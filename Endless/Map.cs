using Endless.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Endless
{
    /// <summary>
    /// the map class
    /// </summary>
    public class Map
    {
        private Texture2D tileSheet;

        /// <summary>
        /// the width of the tile
        /// </summary>
        public int TileWidth = 16;

        /// <summary>
        /// the height of the tile
        /// </summary>
        public int TileHeight = 16;

        private const int TileCount = 5;
        private int[,] tiles;

        /// <summary>
        /// the map Width in tiles
        /// </summary>
        public int mapWidth = 100;
        
        /// <summary>
        /// the map height in tiles
        /// </summary>
        public int mapHeight = 100;

        /// <summary>
        /// the scale used when drawing
        /// </summary>
        public int Scale = 5; 

        /// <summary>
        /// the pixle width
        /// </summary>
        public int PixelWidth => mapWidth * TileWidth * Scale;

        /// <summary>
        /// the pixle height
        /// </summary>
        public int PixelHeight => mapHeight * TileHeight * Scale;


        /// <summary>
        /// loads content using content manager
        /// </summary>
        /// <param name="content">the content manager</param>
        public void LoadContent(ContentManager content)
        {
            tileSheet = content.Load<Texture2D>("RockTileMap-Sheet");

            tiles = new int[mapHeight, mapWidth];
            Random rand = new Random();

            
            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    tiles[y, x] = rand.Next(TileCount);
                }
            }
        }

        /// <summary>
        /// Draws the map with a sprite batch
        /// </summary>
        /// <param name="spriteBatch">the spriteBatch</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    int tileIndex = tiles[y, x];
                    Rectangle source = new Rectangle(0, tileIndex * TileHeight, TileWidth, TileHeight);

                    spriteBatch.Draw(tileSheet, new Vector2(x * TileWidth * 2, y * TileHeight * 2), source, Color.White, 0f, Vector2.Zero, 5f, SpriteEffects.None, 0f);
                }
            }
        }
    }
}
