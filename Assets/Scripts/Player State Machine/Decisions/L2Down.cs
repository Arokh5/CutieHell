using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Decisions/L2Down")]
public class L2Down : Decision
{
    public override bool Decide(Player player)
    {
        return InputManager.instance.GetL2ButtonDown();
    }
}
