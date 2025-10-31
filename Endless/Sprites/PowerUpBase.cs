using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Endless.Sprites
{
    /// <summary>
    /// an enum containg the rarity of items
    /// </summary>
    public enum PowerUpRarity
    {
        Common,
        Rare,
        Epic,
        Legendary
    }

    /// <summary>
    /// Power ups base class
    /// </summary>
    public abstract class PowerUpBase
    {
        /// <summary>
        /// the name of the powerUps
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// the description of the power Ups
        /// </summary>
        public string Description { get; protected set; }

        /// <summary>
        /// the powerUps rarity
        /// </summary>
        public PowerUpRarity Rarity { get; protected set; }

        /// <summary>
        /// the powerUps Icon
        /// </summary>
        public Texture2D Icon { get; protected set; }

        /// <summary>
        /// updates the powerUps with gameTime
        /// </summary>
        /// <param name="gameTime">the gameTime</param>
        /// <param name="player">the player</param>
        public virtual void Update(GameTime gameTime, TravelerSprite player)
        {
            //time limit buffs
        }

        /// <summary>
        /// removes the affects from the player
        /// </summary>
        /// <param name="player">the player</param>
        public virtual void RemoveEffect(TravelerSprite player) 
        {
            // removing buffs
        }
    }
}
