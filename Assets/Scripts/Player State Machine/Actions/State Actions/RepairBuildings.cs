using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/RepairBuildings")]
public class RepairBuildings : StateAction
{
    public float maxMonumentRepairDistance = 5.0f;

    public override void Act(Player player)
    {
        bool showMessage = false;
        if (player.nearbyTrap && !player.nearbyTrap.HasFullHealth() && player.nearbyTrap.GetRepairCost() <= player.evilLevel)
        {
            showMessage = true;

            if (InputManager.instance.GetTriangleButtonDown())
            {
                player.SetEvilLevel(-player.nearbyTrap.GetRepairCost());
                player.nearbyTrap.FullRepair();
            }
        }
        
        if (!player.monument.HasFullHealth() && player.monument.GetRepairCost() <= player.evilLevel && Vector3.Distance(player.transform.position, player.monument.transform.position) < maxMonumentRepairDistance)
        {
            showMessage = true;

            if (InputManager.instance.GetTriangleButtonDown())
            {
                player.SetEvilLevel(-player.monument.GetRepairCost());
                player.monument.FullRepair();
            }
        }

        if (showMessage)
            UIManager.instance.ShowRepairText();
        else
            UIManager.instance.HideRepairText();
    }
}
