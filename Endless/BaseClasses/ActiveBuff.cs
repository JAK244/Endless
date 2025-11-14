using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Endless.BaseClasses
{
    public class ActiveBuff
    {
        public Texture2D Icon;
        public int Count;

        public ActiveBuff(Texture2D icon)
        {
            Icon = icon;
            Count = 1;
        }
    }
}
