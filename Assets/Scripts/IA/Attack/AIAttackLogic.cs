using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIAttackLogic : MonoBehaviour {

    #region Public Methods
    public abstract void AttemptAttack(Building attackTarget, Vector3 navigationTarget);
    public abstract bool IsInAttackRange(Vector3 navigationTarget);
    #endregion
}
