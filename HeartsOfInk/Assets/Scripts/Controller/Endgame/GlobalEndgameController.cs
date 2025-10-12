using Assets.Scripts.Data;
using Assets.Scripts.Data.Literals;
using Assets.Scripts.Logic;
using NETCoreServer.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GlobalEndgameController : MonoBehaviour
{
    private EndgameLogic endgameLogic;
    private StatisticsController statisticsController;
    private SceneChangeController sceneChangeController;
    public Text resultTitle;
    public Transform btnGoToMenu;

    // Start is called before the first frame update
    void Start()
    {
        endgameLogic = new EndgameLogic();
        statisticsController = FindObjectOfType<StatisticsController>();
        sceneChangeController = FindObjectOfType<SceneChangeController>();

        // Puede ser null solo en caso de iniciarse el juego directamente en la pantalla.
        if (statisticsController != null)
        {
            SetResultTitle();
        }
        else
        {
            Debug.LogWarning("statisticsController is null");
        }
    }

    private void SetResultTitle()
    {
        // Victory or defeat
        if (endgameLogic.IsPlayerVictory(statisticsController.ThisPcPlayerStats, statisticsController.playersStats))
        {
            resultTitle.text = "Victoria";
        }
        else
        {
            resultTitle.text = "Derrota";
        }
    }

    public void GoBackToMenu()
    {
        if (statisticsController != null)
        {
            Destroy(statisticsController.gameObject);
        }
        
        sceneChangeController.ChangeScene(btnGoToMenu);
    }
}
