using UnityEngine;

[System.Serializable]
public class FollowUpAttack
{
    [Header("Follow Up Attack")]
    public AttackType attack;
    public State relatedState;
    public TimingInfo timing;
}
