using UnityEngine;

[System.Serializable]
public class AttackChain
{
    [Header("Attack Chain")]
    public string name;
    public AttackType startAttack;
    public FollowUpAttack[] followUps;
    [HideInInspector]
    public FollowUpAttack currentFollowUp = null;
}
