using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Endless.Sprites;
using Microsoft.Xna.Framework;

namespace Endless.Managers
{
    public class PowerUpManager
    {
        private List<PowerUpBase> activePowerUps = new List<PowerUpBase>();
        private Random rand = new Random();

        private List<Type> avilablePowerUps = new List<Type>
        {
            //add types later
        };

        public IReadOnlyList<PowerUpBase> ActivePowerUps => activePowerUps.AsReadOnly();

        public void AddPowerUp(PowerUpBase powerUp, TravelerSprite player)
        {
            //powerUp.ApplyEffect(player);
            activePowerUps.Add(powerUp);
        }

        public void Update(GameTime gameTime, TravelerSprite player)
        {
            foreach (var p in activePowerUps)
                p.Update(gameTime, player);
        }

        

        /// <summary>
        /// Removes all temporary or expired effects, if needed.
        /// </summary>
        public void ClearExpired(TravelerSprite player)
        {
            for (int i = activePowerUps.Count - 1; i >= 0; i--)
            {
                var p = activePowerUps[i];
                // You can add a flag like p.IsExpired if you want
                // For now this is just a placeholder
            }
        }
    }
}
