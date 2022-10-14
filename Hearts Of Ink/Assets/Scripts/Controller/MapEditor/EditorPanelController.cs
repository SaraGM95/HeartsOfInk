﻿using Assets.Scripts.Controller.InGame;
using Assets.Scripts.Data;
using Assets.Scripts.Data.EditorModels;
using Assets.Scripts.Data.GlobalInfo;
using Assets.Scripts.DataAccess;
using Assets.Scripts.Logic;
using Assets.Scripts.Utils;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class EditorPanelController : MonoBehaviour
{
    private MapModelHeader mapModelHeader;
    private MapModel mapModel;
    private GlobalInfo globalInfo;
    private List<Dropdown> factions;
    private List<MapModelHeader> availableMaps;
    public MapEditorLogicController mapEditorLogic;
    public Transform citiesHolder;
    public Transform troopsHolder;
    public Dropdown cbMaps;
    public InputField mapName;
    public Toggle isForMultiplayer;
    public Toggle isForSingleplayer;
    public int startFactionLines;
    public int spacing;

    public MapModel MapModel { get { return mapModel; } }

    void Start()
    {
        factions = new List<Dropdown>();
        LoadAvailableMaps();
        cbMaps.onValueChanged.AddListener(delegate { LoadMap(); });
        LoadMap();
    }

    void Update()
    {
        
    }

    public void LoadAvailableMaps()
    {
        cbMaps.options.Clear();
        availableMaps = MapDAC.GetAvailableMaps();
        availableMaps.ForEach(map => cbMaps.options.Add(new Dropdown.OptionData(map.DisplayName)));
        cbMaps.RefreshShownValue();
    }

    public void LoadMap()
    {
        Debug.Log("Loading map: " + cbMaps.itemText.text);
        mapModelHeader = availableMaps.Find(map => map.DisplayName == cbMaps.options[cbMaps.value].text);
        mapModel = MapDAC.LoadMapInfo(mapModelHeader.DefinitionName);
        globalInfo = MapDAC.LoadGlobalMapInfo();

        mapName.text = mapModel.DisplayName;
        isForMultiplayer.isOn = mapModel.AvailableForMultiplayer;
        isForSingleplayer.isOn = mapModel.AvailableForSingleplayer;
        MapController.Instance.UpdateMap(mapModel.SpritePath);
        UpdateCanvas();
    }

    public void AddNewFactionLine()
    {
        byte maxSocketId = 0;
        MapPlayerModel playerModel;

        mapModel.Players.ForEach(player => maxSocketId = player.MapSocketId > maxSocketId ? player.MapSocketId : maxSocketId);
        maxSocketId++;
        playerModel = new MapPlayerModel(maxSocketId);
        mapModel.Players.Add(playerModel);
        LoadFactionLine(playerModel);
    }

    public void OnClick_PlayerColor(Image colorImage)
    {
        Debug.Log($"Color changed for image {colorImage.name}; color: {colorImage.color}");
        colorImage.color = ColorUtils.NextColor(colorImage.color, globalInfo.AvailableColors);
    }

    public void ShowMapInfoPanel_OnClick()
    {
        gameObject.SetActive(!isActiveAndEnabled);
    }

    public void ChangeOption_OnClick(GameObject sender)
    {
        
    }

    private void LoadFactionLines()
    {
        CleanFactionLines();
        foreach (MapPlayerModel player in mapModel.Players)
        {
            LoadFactionLine(player);
        }
    }

    private void LoadFactionLine(MapPlayerModel player)
    {
        string prefabPath = "Prefabs/editorFactionLine";
        Transform newObject;
        Vector3 position;
        GlobalInfoFaction faction;
        Dropdown cbFaction;
        Dropdown cbPlayerType;
        Image colorFactionImage;
        Button btnColorFaction;
        Toggle tgIsPlayable;

        faction = globalInfo.Factions.Find(item => item.Id == player.FactionId);
        position = new Vector3(0, startFactionLines);
        position.y -= spacing * factions.Count;
        newObject = ((GameObject)Instantiate(Resources.Load(prefabPath), position, transform.rotation)).transform;
        newObject.name = "factionLine_" + player.MapSocketId;
        newObject.SetParent(this.transform, false);

        cbFaction = newObject.Find("cbFaction").GetComponent<Dropdown>();
        cbPlayerType = newObject.Find("cbPlayerType").GetComponent<Dropdown>();
        colorFactionImage = newObject.Find("btnColorFaction").GetComponent<Image>();
        btnColorFaction = newObject.Find("btnColorFaction").GetComponent<Button>();
        tgIsPlayable = newObject.Find("tgIsPlayable").GetComponent<Toggle>();

        btnColorFaction.onClick.AddListener(delegate { OnClick_PlayerColor(colorFactionImage); });
        cbPlayerType.value = player.IaId;
        colorFactionImage.color = ColorUtils.GetColorByString(player.Color);
        LoadFactionsCombo(cbFaction, player.FactionId);
        tgIsPlayable.isOn = player.IsPlayable;

        factions.Add(cbFaction);
    }

    public void SaveMap()
    {
        UpdateModel();
        MapDAC.SaveMapDefinition(mapModel);
        MapDAC.SaveMapHeader(mapModelHeader);
    }

    private void UpdateCanvas()
    {
        LoadFactionLines();
        SetCitiesInCanvas();
        SetTroopsInCanvas();
    }

    private void SetCitiesInCanvas()
    {
        foreach (MapCityModel mapCityModel in mapModel.Cities)
        {
            mapEditorLogic.SetCityInCanvas(mapCityModel);
        }
    }

    private void SetTroopsInCanvas()
    {
        foreach (MapTroopModel mapTroopModel in mapModel.Troops)
        {
            mapEditorLogic.SetTroopInCanvas(mapTroopModel);
        }
    }

    private void UpdateModel()
    {
        SetMapInfoOnModel();
        SaveFactionsInModel();
        SaveCitiesInModel();
        SaveTroopsInModel();
    }

    private void LoadFactionsCombo(Dropdown factionsCombo, int factionId)
    {
        List<Dropdown.OptionData> dropdownOptions = new List<Dropdown.OptionData>();

        foreach (GlobalInfoFaction factionInfo in globalInfo.Factions)
        {
            dropdownOptions.Add(new Dropdown.OptionData()
            {
                text = factionInfo.NameLiteral
            });
        }

        factionsCombo.AddOptions(dropdownOptions);
        factionsCombo.RefreshShownValue();
        factionsCombo.value = factionId;
    }

    private void SetMapInfoOnModel()
    {
        string image = cbMaps.options[cbMaps.value].text;

        mapModel.DisplayName = mapName.text;
        mapModel.AvailableForMultiplayer = isForMultiplayer.isOn;
        mapModel.AvailableForSingleplayer = isForSingleplayer.isOn;

        mapModelHeader.DisplayName = mapName.text;
        mapModelHeader.AvailableForMultiplayer = isForMultiplayer.isOn;
        mapModelHeader.AvailableForSingleplayer = isForSingleplayer.isOn;
    }

    private void SaveFactionsInModel()
    {
        Transform lineObject;
        MapPlayerModel playerModel;
        Dropdown cbPlayerType;
        Image btnColorFaction;
        Toggle tgIsPlayable;
        Text txtAlliance;

        foreach (Dropdown cbFaction in factions)
        {
            byte mapSocketId;

            lineObject = cbFaction.transform.parent;
            mapSocketId = Convert.ToByte(lineObject.name.Split('_')[1]);
            playerModel = mapModel.Players.Find(item => item.MapSocketId == mapSocketId);

            if (playerModel == null)
            {
                playerModel = new MapPlayerModel(mapSocketId);
                mapModel.Players.Add(playerModel);
            }
            
            cbPlayerType = lineObject.Find("cbPlayerType").GetComponent<Dropdown>();
            btnColorFaction = lineObject.Find("btnColorFaction").GetComponent<Image>();
            tgIsPlayable = lineObject.Find("tgIsPlayable").GetComponent<Toggle>();
            txtAlliance = lineObject.Find("btnAlliance").GetComponentInChildren<Text>();

            playerModel.IaId = cbPlayerType.value;
            playerModel.FactionId = globalInfo.Factions.Find(item =>
                    item.NameLiteral == cbFaction.options[cbFaction.value].text).Id;
            playerModel.Alliance = Convert.ToInt32(txtAlliance);
            playerModel.IsPlayable = tgIsPlayable.isOn;
            playerModel.Color = ColorUtils.GetStringByColor(btnColorFaction.color);
        }
    }

    private void SaveCitiesInModel()
    {
        mapModel.Cities = new List<MapCityModel>();

        foreach (Transform city in citiesHolder)
        {
            EditorCityController editorCity = city.GetComponent<EditorCityController>();

            mapModel.Cities.Add(new MapCityModel()
            {
                MapSocketId = Convert.ToByte(editorCity.ownerSocketId),
                Name = editorCity.name,
                Position = new float[] { editorCity.transform.position.x, editorCity.transform.position.y },
                Type = editorCity.isCapital ? 0 : 1
            });
        }
    }

    private void SaveTroopsInModel()
    {
        TextMeshProUGUI unitsText;
        mapModel.Troops = new List<MapTroopModel>();

        foreach (Transform troop in troopsHolder)
        {
            EditorTroopController editorCity = troop.GetComponent<EditorTroopController>();
            unitsText = troop.GetComponent<TextMeshProUGUI>();

            mapModel.Troops.Add(new MapTroopModel()
            {
                MapSocketId = Convert.ToByte(editorCity.ownerSocketId),
                Position = new float[] { editorCity.transform.position.x, editorCity.transform.position.y },
                Units = Convert.ToInt32(unitsText.text)
            });
        }
    }

    private void CleanFactionLines()
    {
        foreach (Dropdown cbFaction in factions)
        {
            Destroy(cbFaction.transform.parent.gameObject);
        }

        factions.Clear();
    }
}