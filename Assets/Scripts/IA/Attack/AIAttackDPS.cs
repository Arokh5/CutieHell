using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttackDPS : AIAttackLogic {

    #region Fields
    public float attackRange;
    public float dps;
    #endregion

    #region Public Methods
    public override void AttemptAttack(Building target)
    {
        if (Vector3.Distance(transform.position, target.transform.position) < attackRange)
        {
            Attack(target);
        }
    }
    #endregion

    #region Private Methods
    private void Attack(Building target)
    {
        target.TakeDamage(dps * Time.deltaTime, AttackType.ENEMY);
    }
    #endregion
}
