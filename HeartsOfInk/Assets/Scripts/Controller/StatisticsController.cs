using Assets.Scripts.Data;
using Assets.Scripts.Logic;
using NETCoreServer.Models;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StatisticsController : MonoBehaviour
{
    private readonly StatisticsLogic logic = new StatisticsLogic();
    public List<PlayerStatistics> playersStats = new List<PlayerStatistics>();
    public PlayerStatistics ThisPcPlayerStats 
    {   get 
        {
            if (playersStats.Count == 0)
            {
                throw new System.Exception("playersStats is empty, don't have players");
            }
            else
            {
                return playersStats.FirstOrDefault(playerStats => playerStats.Player.IaId == Player.IA.PLAYER);
            }
        } 
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public PlayerStatistics GetPlayerStats(Player factionId)
    {
        return playersStats.Find(item => item.Player == factionId);
    }

    public PlayerStatistics GetPlayerStatsByPlayername(string playerName)
    {
        if (playersStats != null)
        {
            return playersStats.Find(item => item.Player.ToString() == playerName);
        }
        else
        {
            return null;
        }
    }

    public void ReportArmyDefeated(TroopController destroyed, TroopController destroyer)
    {
        Player destroyedFaction = destroyed.troopModel.Player;
        Player destroyerFaction = destroyer.troopModel.Player;

        Debug.Log($"ReportArmyDefeated - start. Destroyer: {destroyer}; destroyed: {destroyed}");
        logic.ReportArmyDefeated(destroyedFaction, destroyerFaction, ref playersStats);
    }

    public void CreatePlayerStatsFromGame(GameModel gameModel)
    {
        logic.CreatePlayerStatsFromGame(gameModel, ref playersStats);
    }

    public void ReportGameEnd(List<CityController> cities)
    {
        foreach (PlayerStatistics playerStats in playersStats)
        {
            playerStats.CitiesAtEnd = 0;

            foreach (CityController city in cities)
            {
                if (city.Owner == playerStats.Player)
                {
                    playerStats.CitiesAtEnd++;
                }
            }
        }
    }
}
