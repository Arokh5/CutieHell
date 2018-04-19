using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttackCooldown : AIAttackLogic
{
    #region Fields
    public float attackRange;
    public float attackDamage;
    [Tooltip("Time expressed in seconds.")]
    public float cooldownDuration;

    public Transform attackSpawnPoint;
    public EnemyRangeAttack attackPrefab;
    public bool makeAttack;

    private float lastAttackTime = 0;
    private Animator animator;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        animator = GetComponent<Animator>();
        makeAttack = false;
    }
    #endregion

    #region Public Methods
    public override void AttemptAttack(Building target)
    {
        if (Time.time - lastAttackTime > cooldownDuration && Vector3.Distance(transform.position, target.transform.position) < attackRange)
        {
            animator.SetTrigger("Attack");
            lastAttackTime = Time.time;
        }
        if (makeAttack)
        {
            Attack(target);
            makeAttack = false;
        }
    }
    #endregion

    #region Private Methods
    private void Attack(Building target)
    {
        EnemyRangeAttack currentAttack = Instantiate(attackPrefab, attackSpawnPoint.position, attackSpawnPoint.rotation);
        currentAttack.Fire(target, attackDamage);
        //target.TakeDamage(attackDamage, AttackType.ENEMY);
    }
    #endregion
}
