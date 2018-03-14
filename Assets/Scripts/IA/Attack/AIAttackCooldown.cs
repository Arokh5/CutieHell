﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttackCooldown : AIAttackLogic
{
    #region Fields
    public float attackRange;
    public float attackDamage;
    [Tooltip("Time expressed in seconds.")]
    public float cooldownDuration;

    private float lastAttackTime = 0;
    #endregion

    #region Public Methods
    public override void AttemptAttack(Building target)
    {
        if (Time.time - lastAttackTime > cooldownDuration && Vector3.Distance(transform.position, target.transform.position) < attackRange)
        {
            Attack(target);
            lastAttackTime = Time.time;
        }
    }
    #endregion

    #region Private Methods
    private void Attack(Building target)
    {
        target.TakeDamage(attackDamage, AttackType.ENEMY);
    }
    #endregion
}
