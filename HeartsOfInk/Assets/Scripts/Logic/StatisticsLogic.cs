using Assets.Scripts.Data;
using NETCoreServer.Models;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Logic
{
    public class StatisticsLogic
    {
        [Obsolete("Not obsolete: Pending unit test coverage.")]
        public void ReportArmyDefeated(Player destroyedFaction, Player destroyerFaction, ref List<PlayerStatistics> playersStats)
        {
            foreach (PlayerStatistics factionStatistics in playersStats)
            {
                if (factionStatistics.Player == destroyedFaction ||
                    factionStatistics.Player == destroyerFaction)
                {
                    factionStatistics.ReportArmyDefeated(destroyedFaction);
                }
            }
        }

        [Obsolete("Not obsolete: Pending unit test coverage.")]
        public void CreatePlayerStatsFromGame(GameModel gameModel, ref List<PlayerStatistics> playersStats)
        {
            foreach (Player player in gameModel.Players)
            {
                playersStats.Add(new PlayerStatistics(player));
            }
        }
    }
}
