using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/RepairBuildings")]
public class RepairBuildings : StateAction
{
    public override void Act(Player player)
    {
        bool showMessage = false;
        bool showLockedMessage = false;

        // Trap repair
        if (player.nearbyTrap && !player.nearbyTrap.HasFullHealth())
        {
            showLockedMessage = true;
            if (player.nearbyTrap.CanRepair() && player.nearbyTrap.GetRepairCost() <= player.evilLevel)
            {
                showMessage = true;

                if (InputManager.instance.GetTriangleButtonDown())
                {
                    player.nearbyTrap.FullRepair();
                }
            }
        }
        
        // Monnument repair
        if (!player.monument.HasFullHealth() && Vector3.Distance(player.transform.position, player.monument.transform.position) < player.monument.maxRepairDistance)
        {
            showLockedMessage = true;
            if (player.monument.GetRepairCost() <= player.evilLevel)
            {
                showMessage = true;

                if (InputManager.instance.GetTriangleButtonDown())
                {
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
