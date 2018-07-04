using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttackDPS : AIAttackLogic {

    #region Fields
    public float attackRange;
    public float dps;
    private Animator animator;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    #endregion

    #region Public Methods
    public override void AttemptAttack(IDamageable target, Vector3 navigationTarget)
    {
        if (IsInAttackRange(navigationTarget))
        {
            animator.SetTrigger("Attack");
            this.transform.LookAt(target.transform.position);
            Attack(target);
        }
    }

    public override bool IsInAttackRange(Vector3 navigationTarget)
    {
        return Vector3.Distance(transform.position, navigationTarget) < attackRange;
    }
    #endregion

    #region Private Methods
    private void Attack(IDamageable target)
    {
        target.TakeDamage(dps * Time.deltaTime, AttackType.ENEMY);
    }
    #endregion
}
