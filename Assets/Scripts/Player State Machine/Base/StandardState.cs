using UnityEngine;

[CreateAssetMenu (menuName ="Player State Machine/States/StandardState")]
public class StandardState : State
{
    #region Fields
    public StateAction[] onEnterActions;
    public StateAction[] onExitActions;
    public StateAction[] stateActions;
    public Transition[] transitions;
    #endregion

    #region Public Methods
    public override void EnterState(Player player)
    {
        for (int i = 0; i < onEnterActions.Length; ++i)
            onEnterActions[i].Act(player);
    }

    public override void UpdateState(Player player)
    {
        foreach (StateAction action in stateActions)
        {
            action.Act(player);
        }

        foreach (Transition transition in transitions)
        {
            if (transition.decision.Decide(player))
            {
                player.TransitionToState(transition.targetState);
                break;
            }
        }
    }    

    public override void ExitState(Player player)
    {
        for (int i = 0; i < onExitActions.Length; ++i)
            onExitActions[i].Act(player);
    }
    #endregion
}
