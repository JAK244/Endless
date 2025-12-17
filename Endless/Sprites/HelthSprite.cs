using Endless.Collisions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Endless.Sprites
{
    /// <summary>
    /// the healthsprite class
    /// </summary>
    public class HelthSprite
    {
        private Model heartModel;

        private Matrix world;
        private float scale = 0.4f; // tweak here
        private float rotation = 0f;

        /// <summary>
        /// the position of the sprite
        /// </summary>
        public Vector2 position;

        /// <summary>
        /// Damage indicader
        /// </summary>
        public bool Damaged { get; set; } = false;


        /// <summary>
        /// loads the content using a content manager
        /// </summary>
        /// <param name="content">the content mangager</param>
        public void LoadContent(ContentManager content)
        {
            heartModel = content.Load<Model>("LowPoly_Heart");
        }

        /// <summary>
        /// Updates the sprite;s postion on users input
        /// </summary>
        /// <param name="gameTime">the game time</param>
        public void Update(GameTime gameTime)
        {
            rotation += 1.5f * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        /// <summary>
        /// Draws the sprite
        /// </summary>
        /// <param name="gameTime">the game time</param>
        /// <param name="spriteBatch">the sprite batch to render with</param>
        public void Draw3D(GraphicsDevice graphics, Matrix view, Matrix projection)
        {
            if (Damaged) return;

            
            Vector3 uiPosition3D = new Vector3(position.X + 30, position.Y + 30, 0);

            world =
            Matrix.CreateScale(scale) *
            Matrix.CreateRotationX(MathF.PI) *        
            Matrix.CreateRotationY(rotation) *
            Matrix.CreateTranslation(uiPosition3D);


            foreach (ModelMesh mesh in heartModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = world;
                    effect.View = view;
                    effect.Projection = projection;
                    effect.EnableDefaultLighting();
                }

                mesh.Draw();
            }

        }
    }
}
