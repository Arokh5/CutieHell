using UnityEngine;

public class AttackChainState : State
{
    #region Fields
    private State chainState;
    #endregion

    #region Public Methods
    public override void EnterState(Player player)
    {
        chainState = AttackChainsManager.instance.GetNextChainState();
        chainState.EnterState(player);
    }

    public override void UpdateState(Player player)
    {
        chainState.UpdateState(player);
    }

    public override void ExitState(Player player)
    {
        chainState.ExitState(player);
        chainState = null;
    }
    #endregion
}
