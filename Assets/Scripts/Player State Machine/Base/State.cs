using UnityEngine;

public abstract class State : ScriptableObject
{
    public abstract void EnterState(Player player);
    public abstract void UpdateState(Player player);
    public abstract void ExitState(Player player);
}
