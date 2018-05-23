using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/CanonTrapExitAction")]
public class CanonTrapExitAction : StateAction
{

    public override void Act(Player player)
    {
        GameManager.instance.SetCrosshairActivate(true);
        player.currentTrap.canonTargetDecal.gameObject.SetActive(false);
        player.currentTrap.GetCanonAmmoIndicator().gameObject.SetActive(false);
    }
}
