using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Decisions/DashEnterDecision")]
public class DashEnterDecision : Decision
{
    public float limitCheckUpwardsOffset = 0.2f;
    public LayerMask dashLimitLayerMask;

    public override bool Decide(Player player)
    {
        return !player.knockbackActive && InputManager.instance.GetL2ButtonDown();
    }
}
