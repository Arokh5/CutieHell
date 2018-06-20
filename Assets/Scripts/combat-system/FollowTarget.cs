using UnityEngine;

public class FollowTarget : PooledParticleSystem
{
    #region Fields

    public AttackType attackType;
    public LayerMask hitLayerMask;
    [SerializeField]
    private float attackSpeed;
    [SerializeField]
    private float lifeTime;
    [SerializeField]
    private int damage;
    [SerializeField]
    private AudioClip explosionSfx;

    private Transform mainCamera;
    private Vector3 camForwardDir;
    private bool directionSet = false;
    private AIEnemy enemy;
    private Transform enemyTransform;
    private Vector3 hitOffset;
    private float time = 0;

    #endregion

    #region MonoBehaviour Methods

    private void Awake()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    private void Update()
    {
        if (enemy)
            CheckForEnemyDeath();
        SetOrbDirection();
        DestroyOrb();
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((1 << other.gameObject.layer & hitLayerMask) != 0)
        {
            AIEnemy enemyHit = other.GetComponent<AIEnemy>();
            if (enemyHit)
            {
                StatsManager.instance.RegisterWeakAttackHit();
                enemyHit.TakeDamage(damage, attackType);
                enemyHit.SetKnockback(this.transform.position);
                SoundManager.instance.PlaySfxClip(explosionSfx);
            }
            else if (attackType == AttackType.WEAK)
            {
                StatsManager.instance.RegisterWeakAttackMissed();
            }
            ReturnToPool();
        }
    }

    #endregion

    #region Public Methods

    public override void Restart()
    {
        time = 0;
        directionSet = false;
        enemyTransform = null;
        enemy = null;
        hitOffset = Vector3.zero;
    }

    public void SetEnemyTransform(Transform enemyTransform)
    {
        this.enemyTransform = enemyTransform;
        if (enemyTransform)
            this.enemy = enemyTransform.GetComponent<AIEnemy>();
    }

    public void SetHitOffset(Vector3 hitOffset)
    {
        this.hitOffset = hitOffset;
    }

    #endregion

    #region Private Methods
    
    private void CheckForEnemyDeath()
    {
        if (enemy.IsDead())
        {
            Vector3 hitPos = new Vector3(enemyTransform.position.x, enemyTransform.position.y + hitOffset.y, enemyTransform.position.z);
            camForwardDir = transform.InverseTransformDirection((hitPos - transform.position).normalized);
            directionSet = true;
            enemyTransform = null;
            enemy = null;
        }
    }

    private void SetOrbDirection()
    {
        if (enemyTransform != null)
        {
            Vector3 hitPos = new Vector3(enemyTransform.position.x, enemyTransform.position.y + hitOffset.y, enemyTransform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, hitPos, attackSpeed * Time.deltaTime);
        }
        else
        {
            if (!directionSet)
            {
                camForwardDir = transform.InverseTransformDirection(mainCamera.forward);
                directionSet = true;
            }
            
            transform.Translate(camForwardDir * attackSpeed * Time.deltaTime);
        }
    }

    private void DestroyOrb()
    {
        time += Time.deltaTime;

        if (time >= lifeTime)
        {
            if (attackType == AttackType.WEAK)
                StatsManager.instance.RegisterWeakAttackMissed();
            ReturnToPool();
        }
    }
    #endregion
}