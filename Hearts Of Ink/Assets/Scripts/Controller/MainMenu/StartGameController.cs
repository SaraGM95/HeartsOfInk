﻿using Assets.Scripts.Data;
using Assets.Scripts.Data.GlobalInfo;
using Assets.Scripts.Utils;
using NETCoreServer.Models;
using System;
using UnityEngine;
using UnityEngine.UI;

public class StartGameController : MonoBehaviour
{
    private SceneChangeController sceneChangeController;
    public Transform factionDropdownsHolder;
    public GameOptionsController gameOptionsController;

    private void Start()
    {
        sceneChangeController = FindObjectOfType<SceneChangeController>();
    }

    public void StartGame()
    {
        GameModel gameModel = new GameModel(0);
        gameModel.Gametype = GameModel.GameType.Single;
        
        GetPlayerOptions(gameModel);
        sceneChangeController.ChangeScene(transform);
        gameOptionsController.gameModel = gameModel;
    }

    private void GetPlayerOptions(GameModel gameModel)
    {
        string globalInfoPath = Application.streamingAssetsPath + "/MapDefinitions/_GlobalInfo.json";
        GlobalInfo globalInfo = JsonCustomUtils<GlobalInfo>.ReadObjectFromFile(globalInfoPath);

        foreach (Transform holderChild in factionDropdownsHolder)
        {
            if (holderChild.name.StartsWith(GlobalConstants.FactionLineStart))
            {
                Player player = new Player();
                string[] holderNameSplitted = holderChild.name.Split('_');
                string factionId = holderNameSplitted[1];
                string mapSocketId = holderNameSplitted[2];
                Image btnColorFaction = holderChild.Find("btnColorFaction").GetComponent<Image>();
                Text txtBtnAlliance = holderChild.Find("btnAlliance").Find("txtBtnAlliance").GetComponent<Text>();
                Dropdown iaSelector = holderChild.GetComponentInChildren<Dropdown>();

                player.Faction.Id = Convert.ToInt32(factionId);
                player.Faction.Bonus = new Bonus((Bonus.Id) globalInfo.Factions.Find(item => item.Id == player.Faction.Id).BonusId);
                player.MapSocketId = Convert.ToByte(mapSocketId);
                player.IaId = (Player.IA)(Convert.ToInt32(iaSelector.value));
                player.Color = ColorUtils.GetStringByColor(btnColorFaction.color);
                player.Alliance = string.IsNullOrEmpty(txtBtnAlliance.text) ? (byte) 0 : Convert.ToByte(txtBtnAlliance.text);
                
                if (player.IaId == Player.IA.PLAYER)
                {
                    player.Name = "Player";
                }
                else
                {
                    // Todo: Indicar nombre obtenido en el mapa.
                    player.Name = factionId;
                }

                gameModel.Players.Add(player);
            }
        }
    }
}
