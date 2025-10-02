using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Endless.Collisions
{
    /// <summary>
    /// the rectangel bounds class
    /// </summary>
    public class BoundingRectangle
    {
        /// <summary>
        /// the X of the rectangel
        /// </summary>
        public float X;

        /// <summary>
        /// the Y of the rectangle
        /// </summary>
        public float Y;

        /// <summary>
        /// the width of the rectangle
        /// </summary>
        public float Width;

        /// <summary>
        /// the height of the rectangle
        /// </summary>
        public float Height;

        /// <summary>
        /// the left side of the rectangle
        /// </summary>
        public float Left => X;

        /// <summary>
        /// the right side of the rectangle
        /// </summary>
        public float Right => X + Width;

        /// <summary>
        /// the top of the rectangle 
        /// </summary>
        public float Top => Y;

        /// <summary>
        /// the bottom of the rectangle
        /// </summary>
        public float Bottom => Y + Height;

        /// <summary>
        /// the boundingRectangle Constructor
        /// </summary>
        /// <param name="x">the x</param>
        /// <param name="y">the y</param>
        /// <param name="width">the width</param>
        /// <param name="height">the height</param>
        public BoundingRectangle(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// the boundingRectangle Constructor
        /// </summary>
        /// <param name="x">the x</param>
        /// <param name="y">the y</param>
        /// <param name="width">the width</param>
        /// <param name="height">the height</param>
        public BoundingRectangle(Vector2 position, float width, float height)
        {
            X = position.X;
            Y = position.Y;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// checks collison with another rectangle
        /// </summary>
        /// <param name="other">the other rectangle</param>
        /// <returns>the collision helper</returns>
        public bool CollidesWith(BoundingRectangle other)
        {
            return CollisionHelper.Collides(this, other);
        }

        /// <summary>
        /// checks collison with a circle
        /// </summary>
        /// <param name="other">the  circle</param>
        /// <returns>the collision helper</returns>
        public bool CollidesWith(BoundingCircle other)
        {
            return CollisionHelper.Collides(other, this);
        }


    }
}
