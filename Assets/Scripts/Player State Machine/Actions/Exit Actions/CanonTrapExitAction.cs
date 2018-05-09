using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/CanonTrapExitAction")]
public class CanonTrapExitAction : StateAction
{

    public override void Act(Player player)
    {
        GameManager.instance.SetCrosshairActivate(true);
        player.currentTrap.canonTargetDecal.transform.position = new Vector3(player.currentTrap.transform.position.x, 3.20f, player.currentTrap.transform.position.z);
        Destroy(GameObject.FindWithTag("CanonBallLaunchDecal"));
        player.currentTrap.canonTargetDecal.gameObject.SetActive(false);        
    }
}
