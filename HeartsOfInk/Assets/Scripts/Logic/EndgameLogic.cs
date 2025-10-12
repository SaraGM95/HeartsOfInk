using Assets.Scripts.Data;
using NETCoreServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Logic
{
    public class EndgameLogic
    {
        public bool IsPlayerVictory(PlayerStatistics thisPcPlayer, List<PlayerStatistics> allPlayers)
        {
            if (thisPcPlayer.CitiesAtEnd > 0)
            {
                return true;
            }
            else if (thisPcPlayer.Player.Alliance == 0)
            {
                //This player doesn't have alliances, and don't have cities. Have lost
                return false;
            }
            else
            {
                // If any ally have cities this means that the alliance is victorius
                List<PlayerStatistics> allies = allPlayers.FindAll(player => player.Player.Alliance == thisPcPlayer.Player.Alliance);

                return allies.Any(ally => ally.CitiesAtEnd > 0);
            }
        }
    }
}
