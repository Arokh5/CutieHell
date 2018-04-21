﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/RepairBuildings")]
public class RepairBuildings : StateAction
{
    public float maxMonumentRepairDistance = 5.0f;

    public override void Act(Player player)
    {
        bool showMessage = false;
        bool showLockedMessage = false;

        if (player.nearbyTrap && !player.nearbyTrap.HasFullHealth())
        {
            showLockedMessage = true;
            if (player.nearbyTrap.GetRepairCost() <= player.evilLevel)
            {
                showMessage = true;

                if (InputManager.instance.GetTriangleButtonDown())
                {
                    player.SetEvilLevel(-player.nearbyTrap.GetRepairCost());
                    player.nearbyTrap.FullRepair();
                }
            }
        }
        
        if (Vector3.Distance(player.transform.position, player.monument.transform.position) < maxMonumentRepairDistance && !player.monument.HasFullHealth())
        {
            showLockedMessage = true;
            if (player.monument.GetRepairCost() <= player.evilLevel)
            {
                showMessage = true;

                if (InputManager.instance.GetTriangleButtonDown())
                {
                    player.SetEvilLevel(-player.monument.GetRepairCost());
                    player.monument.FullRepair();
                }
            }
        }

        if (showMessage)
            UIManager.instance.ShowRepairText();
        else if (showLockedMessage)
            UIManager.instance.ShowLockedRepairText();
        else
            UIManager.instance.HideRepairText();
    }
}
