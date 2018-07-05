using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Decisions/FogEnterDecision")]
public class FogEnterDecision : Decision
{
    public override bool Decide(Player player)
    {
        return InputManager.instance.GetSquareButtonDown();
    }
}
