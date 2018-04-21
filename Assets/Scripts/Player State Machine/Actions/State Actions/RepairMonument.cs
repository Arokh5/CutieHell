using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/RepairMonument")]
public class RepairMonument : StateAction
{
    public float maxRepairDistance = 5.0f;

    public override void Act(Player player)
    {
        bool showMessage = false;
        if (!player.monument.HasFullHealth() && player.monument.GetRepairCost() <= player.evilLevel && Vector3.Distance(player.transform.position, player.monument.transform.position) < maxRepairDistance)
        {
            showMessage = true;

            if (InputManager.instance.GetTriangleButtonDown())
            {
                player.SetEvilLevel(-player.monument.GetRepairCost());
                player.monument.FullRepair();
                showMessage = false;
            }
        }

        if (showMessage)
            UIManager.instance.ShowRepairText();
        else
            UIManager.instance.HideRepairText();
    }
}
