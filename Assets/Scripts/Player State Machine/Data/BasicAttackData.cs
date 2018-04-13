using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Data/BasicAttackData")]
public class BasicAttackData : ScriptableObject {

    public LayerMask layerMask;
    public float sphereCastRadius;
    public float attackCadency;
    public FollowTarget attackPrefab;
}
