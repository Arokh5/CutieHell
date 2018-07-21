
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Decisions/SquareDownDecision")]
public class SquareDownDecision : Decision
{
    public override bool Decide(Player player)
    {
        return InputManager.instance.GetSquareButtonDown();
    }
}
