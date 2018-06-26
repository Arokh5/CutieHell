using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttackCooldown : AIAttackLogic
{
    #region Fields
    public EnemyType enemyType;
    public float attackRange;
    public float attackDamage;
    [Tooltip("Time expressed in seconds.")]
    public float cooldownDuration;

    public Transform attackSpawnPoint;
    public EnemyRangeAttack attackPrefab;
    private Building attackTarget = null;

    private float lastAttackTime = 0;
    private Animator animator;

    [SerializeField]
    private AudioSource bearAttackSource;
    [SerializeField]
    private AudioClip bearAttackClip;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        animator = GetComponent<Animator>();
        bearAttackSource.clip = bearAttackClip;
    }
    #endregion

    #region Public Methods
    public override void AttemptAttack(Building attackTarget, Vector3 navigationTarget)
    {
        if (IsInAttackRange(navigationTarget))
        {
            if (Time.time - lastAttackTime > cooldownDuration)
            {
                animator.SetTrigger("Attack");
                /* Actual attack will be launched by the Animation */
                this.attackTarget = attackTarget;
                lastAttackTime = Time.time;
            }
        }
        else
        {
            this.attackTarget = null;
        }
    }

    public override bool IsInAttackRange(Vector3 navigationTarget)
    {
        return Vector3.Distance(this.transform.position, navigationTarget) < attackRange;
    }

    public void LaunchAttack()
    {
        if (attackTarget)
        {
            Attack(attackTarget);
        }
        attackTarget = null;
    }
    #endregion

    #region Private Methods
    private void Attack(Building target)
    {
        EnemyRangeAttack currentAttack = AttacksPool.instance.GetAttackObject(enemyType, attackSpawnPoint.position, attackSpawnPoint.rotation).GetComponent<EnemyRangeAttack>();
        currentAttack.Fire(target, attackDamage);
        bearAttackSource.Play();
    }
    #endregion
}
