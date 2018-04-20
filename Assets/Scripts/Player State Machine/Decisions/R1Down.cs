using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Decisions/R1Down")]
public class R1Down : Decision
{
    public override bool Decide(Player player)
    {
        return InputManager.instance.GetR1ButtonDown();
    }
}
