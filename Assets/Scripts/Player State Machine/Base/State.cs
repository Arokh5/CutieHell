using UnityEngine;

[CreateAssetMenu (menuName ="State Machine/State")]
public class State : ScriptableObject
{
    #region Fields
    public StateAction onEnterAction;
    public StateAction onExitAction;
    public StateAction[] stateActions;
    public Transition[] transitions;
    #endregion

    #region Public Methods
    public void UpdateState(Player player)
    {
        foreach (StateAction action in stateActions)
        {
            action.Act(player);
        }

        foreach(Transition transition in  transitions)
        {
            if (transition.decision.Decide(player))
            {
                player.TransitionToState(transition.targetState);
                break;
            }
        }
    }
    #endregion
}
