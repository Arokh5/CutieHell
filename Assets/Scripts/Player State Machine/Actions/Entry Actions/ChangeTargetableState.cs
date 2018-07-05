using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/ChangeTargetableState")]
public class ChangeTargetableState : StateAction
{
    public bool changeTo;

    public override void Act(Player player)
    {
        player.isTargetable = changeTo;
    }
}
