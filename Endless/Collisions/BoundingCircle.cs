using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Endless.Collisions
{
    /// <summary>
    /// the bounding circle class
    /// </summary>
    public class BoundingCircle
    {
        /// <summary>
        /// the center of the circle
        /// </summary>
        public Vector2 Center;
        
        /// <summary>
        /// the radius of the circle
        /// </summary>
        public float Radius;


        /// <summary>
        /// the circle constructor
        /// </summary>
        /// <param name="center">the center </param>
        /// <param name="radius">the radius</param>
        public BoundingCircle(Vector2 center, float radius)
        {
            Center = center;
            Radius = radius;
        }

        /// <summary>
        /// Check collison with another circle
        /// </summary>
        /// <param name="other">the other circle</param>
        /// <returns>colliuson helper</returns>
        public bool CollidesWith(BoundingCircle other)
        {
            return CollisionHelper.Collides(this, other);
        }

        /// <summary>
        /// Check collison with a rectangle
        /// </summary>
        /// <param name="other">the rectangle</param>
        /// <returns>colliuson helper</returns>
        public bool CollidesWith(BoundingRectangle other)
        {
            return CollisionHelper.Collides(this, other);
        }

        
    }
}
