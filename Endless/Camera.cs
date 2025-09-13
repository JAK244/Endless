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
    public class Camera
    {
        public Vector2 Position;

        public Camera(Vector2 position)
        {
            this.Position = position;
        }

        public void Follow(BoundingRectangle target, Vector2 screenSize)
        {
            Position = new Vector2(target.X + (screenSize.X/2 - target.Width / 2), target.Y + (screenSize.Y /2 - target.Height / 2));
        }

        

    }
}
