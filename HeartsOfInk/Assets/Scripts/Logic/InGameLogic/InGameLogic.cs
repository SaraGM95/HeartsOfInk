using NETCoreServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Logic
{
    public class InGameLogic
    {
        public bool IsGameFinished(List<Player> cityOwners)
        {
            if (cityOwners.Count == 0)
            {
                throw new Exception("No owners detected when checking victory conditions");
            }
            else if (cityOwners.Count == 1)
            {
                return true;
            }
            // Si todos están sin alianza (Alliance == 0), pero hay más de uno, no ha terminado
            else if (cityOwners.All(o => o.Alliance == 0))
            {
                // If no one have an alliance and exists more than one owner, this means that the game is not finished.
                return false;
            }
                

            // Filtramos alianzas válidas (no 0)
            var alliances = cityOwners
                .Where(o => o.Alliance != 0)
                .Select(o => o.Alliance)
                .Distinct()
                .ToList();

            // If there is only one alliance among all players who belong to an alliance
            // and every player is either in that alliance or has no alliance (Alliance == 0)
            if (alliances.Count == 1 && cityOwners.All(o => o.Alliance == alliances[0] || o.Alliance == 0))
            {
                // Now check that no players remain outside that alliance
                // If all city owners belong to this alliance (no neutrals holding cities)
                return cityOwners.All(o => o.Alliance == alliances[0]);
            }

            return false;
        } 
    }
}
