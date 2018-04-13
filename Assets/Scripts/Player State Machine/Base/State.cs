using UnityEngine;

[CreateAssetMenu (menuName ="Player State Machine/State")]
public class State : ScriptableObject
{
    #region Fields
    public StateAction[] onEnterActions;
    public StateAction[] onExitActions;
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
