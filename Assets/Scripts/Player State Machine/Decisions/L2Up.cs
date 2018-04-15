using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Decisions/L2Up")]
public class L2Up : Decision
{
    public override bool Decide(Player player)
    {
        return InputManager.instance.GetL2ButtonUp();
    }
}
