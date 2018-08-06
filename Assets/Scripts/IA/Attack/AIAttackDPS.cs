using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttackDPS : AIAttackLogic {

    #region Fields
    public float attackRange;
    public float dps;
    private Animator animator;
    [SerializeField]
    private Transform modelPosition;
    private IDamageable target;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    #endregion

    #region Public Methods
    public override void AttemptAttack(IDamageable _target, Vector3 navigationTarget)
    {
        if (IsInAttackRange(navigationTarget) && !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && !animator.GetBool("Attack"))
        {
            target = _target;
            this.transform.LookAt(_target.transform.position);
            animator.SetBool("Attack",true);
        }
        if(animator.GetBool("Attack") && !IsInAttackRange(navigationTarget))
        {
            animator.SetBool("Attack", false);
        }
        if (animator.GetBool("Attack"))
        {
            animator.SetFloat("Speed", 5.0f);
        }
        else
        {
            animator.SetFloat("Speed", 1.0f);
        }
    }

    public void HitAttack()
    {

        if (!target.IsDead())
        {
            if (Vector3.Distance(modelPosition.position, target.transform.position + Vector3.up) < 2.1f)
            {
                Attack(target);
            }
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
        target.TakeDamage(dps, AttackType.ENEMY);
    }
    #endregion
}
