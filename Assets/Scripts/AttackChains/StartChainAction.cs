using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/StartChainAction")]
public class StartChainAction : StateAction
{
    public AttackType attack;

    public override void Act(Player player)
    {
        AttackChainsManager.instance.ReportStartChainAttempt(attack);
    }
}
