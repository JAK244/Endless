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
    /// the collisionHelper Class
    /// </summary>
    public static class CollisionHelper
    {
        /// <summary>
        /// checks collision between two circles
        /// </summary>
        /// <param name="a">circle a</param>
        /// <param name="b">circle b</param>
        /// <returns>collision point</returns>
        public static bool Collides(BoundingCircle a, BoundingCircle b)
        {
            return Math.Pow(a.Radius + b.Radius, 2) >= Math.Pow(a.Center.X -  b.Center.X, 2) + Math.Pow(a.Center.Y - b.Center.Y,2);
        }

        /// <summary>
        /// checks collisions between two rectangles
        /// </summary>
        /// <param name="a">rectangle a</param>
        /// <param name="b">rectangle b</param>
        /// <returns>collision point</returns>
        public static bool Collides(BoundingRectangle a,  BoundingRectangle b)
        {
            return !(a.Right < b.Left || a.Left > b.Right || a.Top > b.Bottom || a.Bottom < b.Top);
        }

        /// <summary>
        /// checks the collison between a circle and a rectangle
        /// </summary>
        /// <param name="c">the circle</param>
        /// <param name="r">the rectangle</param>
        /// <returns>collision point</returns>
        public static bool Collides(BoundingCircle c, BoundingRectangle r)
        {
            float nearestX = MathHelper.Clamp(c.Center.X, r.Left, r.Right);
            float nearestY = MathHelper.Clamp(c.Center.Y, r.Top, r.Bottom);

            return Math.Pow(c.Radius,2) >= Math.Pow(c.Center.X - nearestX, 2) + Math.Pow(c.Center.Y - nearestY, 2);
        }

        /// <summary>
        /// checks the collison between a rectangle and a circle
        /// </summary>
        /// <param name="c">the circle</param>
        /// <param name="r">the rectangle</param>
        /// <returns>collision point</returns>
        public static bool Collides(BoundingRectangle r, BoundingCircle c) => Collides(c,r);
    }
}
