using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Decisions/L1Down")]
public class L1Down : Decision
{
    public override bool Decide(Player player)
    {
        return InputManager.instance.GetL1ButtonDown();
    }
}
