using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Endless.BaseClasses
{
    /// <summary>
    /// baseClass for buffs
    /// </summary>
    public class ActiveBuff
    {
        public Texture2D Icon;
        public int Count;

        /// <summary>
        /// Active buff constructer
        /// </summary>
        /// <param name="icon">buff icon</param>
        public ActiveBuff(Texture2D icon)
        {
            Icon = icon;
            Count = 1;
        }
    }
}
