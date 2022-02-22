﻿using Assets.Scripts.Data;
using Assets.Scripts.Utils;
using NETCoreServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CityController : MonoBehaviour
{
    private SpriteRenderer SpriteRenderer;
    private GlobalLogicController globalLogic;
    private List<TroopController> troopsInZone;
    private float recruitmentProgress = 0;
    public Player Owner;
    public bool IsCapital;

    // Start is called before the first frame update
    void Start()
    {
        globalLogic = FindObjectOfType<GlobalLogicController>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        troopsInZone = new List<TroopController>();
        SpriteRenderer.color = ColorUtils.GetColorByString(Owner.Color);
    }

    // Update is called once per frame
    void Update()
    {
        CleanUnitsInZoneList();
        UpdateOwner();
        RecruitTroops();
    }

    private void CleanUnitsInZoneList()
    {
        troopsInZone.RemoveAll(item => item == null || item.name == null);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        TroopController troopController;

        troopController = collision.gameObject.GetComponent<TroopController>();

        if (troopController != null)
        {
            troopsInZone.Add(troopController);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        TroopController troopController;

        troopController = collision.gameObject.GetComponent<TroopController>();

        if (troopController != null)
        {
            troopsInZone.Remove(troopController);
        }
    }

    private void OnMouseDown()
    {
        globalLogic.ClickReceivedFromCity(this);
        Debug.Log("On click: " + this);
    }

    private void UpdateOwner()
    {
        if (troopsInZone.Count == 1)
        {
            if (troopsInZone[0].troopModel.Player != Owner)
            {
                if (Owner.Alliance == Player.NoAlliance || troopsInZone[0].troopModel.Player.Alliance != Owner.Alliance)
                {
                    ChangeOwner(troopsInZone[0].troopModel.Player);
                }
            }
        }
        else if (troopsInZone.Count < 0)
        {
            bool ownerPresent = false;
            Dictionary<string, float> unitsInCombat = new Dictionary<string, float>();

            foreach (TroopController troopController in troopsInZone)
            {
                string troopFaction = troopController.troopModel.Player.Name;

                if (troopFaction == Owner.Name)
                {
                    ownerPresent = true;
                    break;
                }
                else if (unitsInCombat.ContainsKey(troopFaction))
                {
                    unitsInCombat[troopFaction] += troopController.troopModel.Units;
                }
                else
                {
                    unitsInCombat.Add(troopFaction, troopController.troopModel.Units);
                }
            }

            if (!ownerPresent)
            {
                unitsInCombat.OrderByDescending(item => item.Value);
                ChangeOwner(globalLogic.gameModel.Players.First(player => player.Name == unitsInCombat.First().Key));
            }
        }
    }

    private void ChangeOwner(Player newOwner)
    {
        Owner = newOwner;
        SpriteRenderer.color = ColorUtils.GetColorByString(Owner.Color);
        recruitmentProgress = 0;
    }

    private void RecruitTroops()
    {
        const int CompanySize = 80;
        const float recruitmentSpeed = 0.5f;
        float capitalBonus = 1;

        if (IsCapital)
        {
            capitalBonus = 1.5f;
        }

        recruitmentProgress += Time.deltaTime * globalLogic.GameSpeed * capitalBonus * recruitmentSpeed;

        if (recruitmentProgress > CompanySize)
        {
            globalLogic.InstantiateTroop(CompanySize, this.transform.position, Owner);
            recruitmentProgress = 0;
        }
    }
}