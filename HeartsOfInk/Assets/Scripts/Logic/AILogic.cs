﻿using Assets.Scripts.Data;
using NETCoreServer.Models;
using Rawgen.Math.Logic.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AILogic
{
    private GlobalLogicController globalLogic;
    public Player Player { get; }

    public AILogic(Player player, GlobalLogicController globalLogicController)
    {
        this.Player = player;
        globalLogic = globalLogicController;
    }

    public void TroopMovementRequest(TroopModel troopModel)
    {
        if (troopModel.Target == null)
        {
            troopModel.SetTarget(GetAttackTarget(troopModel), globalLogic);
        }
        else 
        {
            CityController cityController = troopModel.Target.GetComponent<CityController>();

            if (cityController != null && cityController.Owner == troopModel.Player)
            {
                troopModel.SetTarget(GetAttackTarget(troopModel), globalLogic);
            }
        }
    }

    private GameObject GetAttackTarget(TroopModel troopModel)
    {
        GameObject target = null;
        Vector2 currentPos = troopModel.CurrentPosition;
        GameObject[] cities = GameObject.FindGameObjectsWithTag(Tags.City);
        Dictionary<int, float> distances = new Dictionary<int, float>(); 

        for (int index = 0; index < cities.Length; index++)
        {
            CityController cityController = cities[index].GetComponent<CityController>();

            if (cityController.Owner != Player)
            {
                if (Player.Alliance == 0 || Player.Alliance != cityController.Owner.Alliance)
                {
                    distances.Add(index, MathUtils.ExperimentalDistance(currentPos.x, currentPos.y, cities[index].transform.position.x, cities[index].transform.position.y));
                }
            }
        }

        if (distances.Count > 0)
        {
            int key = distances.OrderBy(item => item.Value).First().Key;
            target = cities[key];
        }
        else if (distances.Count == 1)
        {
            target = cities[distances.Keys.First()];
        }

        return target;
    }
}
