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
    
    private AudioSource bearSource;
    [SerializeField]
    private AudioClip bearClip;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        animator = GetComponent<Animator>();
        bearSource = GetComponent<AudioSource>();
        bearSource.clip = bearClip;
    }
    #endregion

    #region Public Methods
    public override void AttemptAttack(Building target)
    {
        if (Time.time - lastAttackTime > cooldownDuration && Vector3.Distance(transform.position, target.transform.position) < attackRange)
        {
            animator.SetTrigger("Attack");
            /* Actual attack will be launched by the Animation */
            attackTarget = target;
            lastAttackTime = Time.time;
        }
    }

    public override bool IsInAttackRange(Building target)
    {
        return Vector3.Distance(transform.position, target.transform.position) < attackRange;
    }

    public void LaunchAttack()
    {
        if (attackTarget && IsInAttackRange(attackTarget))
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
        bearSource.Play();
    }
    #endregion
}
