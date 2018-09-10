using UnityEngine;

[System.Serializable]
public class FollowUpAttack
{
    [Header("Follow Up Attack")]
    public AttackType attack;
    public State relatedState;
    public TimingInfo timing;

    public bool IsInAttackTimeFrame(float testTime)
    {
        return testTime >= timing.start && testTime <= timing.end;
    }

    public bool IsInAlertTimeFrame(float testTime)
    {
        return testTime >= timing.alert && testTime < timing.start;
    }
}
