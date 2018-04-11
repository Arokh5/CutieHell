using UnityEngine;

public class StateMachineController : MonoBehaviour {

    [SerializeField]
    protected State currentState;

    public virtual void TransitionToState(State targetState)
    {
        if (currentState.onExitAction)
            currentState.onExitAction.Act(this);

        if (targetState.onEnterAction)
            targetState.onEnterAction.Act(this);

        currentState = targetState;        
    }
}
