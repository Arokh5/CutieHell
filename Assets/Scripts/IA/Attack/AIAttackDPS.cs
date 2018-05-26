using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttackDPS : AIAttackLogic {

    #region Fields
    public float attackRange;
    public float dps;
    #endregion

    #region Public Methods
    public override void AttemptAttack(Building target, Vector3 navigationTarget)
    {
        if (IsInAttackRange(navigationTarget))
        {
            Attack(target);
        }
    }

    public override bool IsInAttackRange(Vector3 navigationTarget)
    {
        return Vector3.Distance(transform.position, navigationTarget) < attackRange;
    }
    #endregion

    #region Private Methods
    private void Attack(Building target)
    {
        target.TakeDamage(dps * Time.deltaTime, AttackType.ENEMY);
    }
    #endregion
}
