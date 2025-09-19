using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Endless.Collisions;

namespace Endless
{
    /// <summary>
    /// public camera class
    /// </summary>
    public class Camera
    {
        /// <summary>
        /// the camera position
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// the camera constructor
        /// </summary>
        /// <param name="position">the camera position</param>
        public Camera(Vector2 position)
        {
            this.Position = position;
        }

        /// <summary>
        /// camera follows a target 
        /// </summary>
        /// <param name="target">the given target (player)</param>
        /// <param name="screenSize">the screen size</param>
        public void Follow(BoundingRectangle target, Vector2 screenSize)
        {
            Position = new Vector2(target.X + (screenSize.X/2 - target.Width / 2), target.Y + (screenSize.Y /2 - target.Height / 2));
        }

        

    }
}
