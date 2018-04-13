using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/RepairTrap")]
public class RepairTrap : StateAction
{

    public override void Act(Player player)
    {
        bool showMessage = false;
        if (player.nearbyTrap)
        {
            if (!player.nearbyTrap.HasFullHealth() && player.nearbyTrap.GetRepairCost() <= player.evilLevel)
            {
                showMessage = true;

                if (InputManager.instance.GetTriangleButtonDown())
                {
                    player.SetEvilLevel(-player.nearbyTrap.GetRepairCost());
                    player.nearbyTrap.FullRepair();
                    showMessage = false;
                }
            }
        }

        if (showMessage)
            UIManager.instance.ShowRepairTrapText();
        else
            UIManager.instance.HideRepairTrapText();
    }
}
