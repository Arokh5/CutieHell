using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Decisions/TrapExitCheck")]
public class TrapExitCheck : Decision {

    public override bool Decide(Player player)
    {
        return player.shouldExitTrap || InputManager.instance.GetXButtonDown();
        if (player.shouldExitTrap)
            return true;
        if (InputManager.instance.GetXButtonDown())
            return true;
        return false;
    }
}
