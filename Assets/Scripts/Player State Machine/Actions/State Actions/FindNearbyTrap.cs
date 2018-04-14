using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/FindNearbyTrap")]
public class FindNearbyTrap : StateAction {

    public override void Act(Player player)
    {
        if (player.nearbyTrap == null)
        {
            /* Find nearbyTrap */
            for (int i = 0; i < player.allTraps.Length; i++)
            {
                if (Vector3.Distance(player.transform.position, player.allTraps[i].transform.position) < player.trapMaxUseDistance)
                {
                    player.nearbyTrap = player.allTraps[i];
                }
            }
        }
        else
        {
            /* Potentially nullify nearbyTrap*/
            if (Vector3.Distance(player.transform.position, player.nearbyTrap.transform.position) >= player.trapMaxUseDistance)
                player.nearbyTrap = null;
        }
    }
}
