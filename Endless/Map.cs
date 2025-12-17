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
        private const int TileCount = 5;
        private int[,] tiles;

        /// <summary>
        /// the width of the tile
        /// </summary>
        public int TileWidth = 16;

        /// <summary>
        /// the height of the tile
        /// </summary>
        public int TileHeight = 16;


        /// <summary>
        /// the map width in tiles
        /// </summary>
        public int mapWidth = 50; 

        /// <summary>
        /// the map width in tiles
        /// </summary>
        public int mapHeight = 50; 

        /// <summary>
        /// the scale of the drawing
        /// </summary>
        public int Scale = 2; 
        


        /// <summary>
        /// the pixle width
        /// </summary>
        public int PixelWidth => mapWidth * TileWidth * Scale;

        /// <summary>
        /// the pixle height
        /// </summary>
        public int PixelHeight => mapHeight * TileHeight * Scale;

        /// <summary>
        /// helps place tiles depending on weights 
        /// </summary>
        /// <param name="weights">the tile weight</param>
        /// <param name="rand">random</param>
        /// <returns></returns>
        private int GetWeightedRandom(int[] weights, Random rand)
        {
            int totalWeight = 0;
            foreach (int w in weights) totalWeight += w;

            int r = rand.Next(totalWeight);
            int sum = 0;
            for (int i = 0; i < weights.Length; i++)
            {
                sum += weights[i];
                if (r < sum)
                    return i;
            }
            return weights.Length - 1;
        }

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
                    int[] tileWeights = { 80, 5,5, 5, 5 }; 
                    tiles[y, x] = GetWeightedRandom(tileWeights, rand);
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

        /// <summary>
        /// Serializes the current state of the tiles into a two-dimensional array.
        /// </summary>
        /// <returns>A two-dimensional array of integers representing the current tiles.</returns>
        public int[,] SerializeTiles()
        {
            return tiles;
        }

        /// <summary>
        /// Deserializeses the current state of the tiles into a two-dimensional array.
        /// </summary>
        /// <param name="savedTiles">A two-dimensional array of integers representing the current tiles</param>
        public void DeserializeTiles(int[,] savedTiles)
        {
            tiles = savedTiles;
            mapWidth = tiles.GetLength(1);
            mapHeight = tiles.GetLength(0);
        }
    }
}
