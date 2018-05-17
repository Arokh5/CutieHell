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

    private Transform mainCamera;
    private Vector3 camForwardDir;
    private bool directionSet = false;
    private Transform enemy = null;
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
    }

    public void SetEnemy(Transform enemy)
    {
        this.enemy = enemy;
    }

    public void SetHitOffset(Vector3 hitOffset)
    {
        this.hitOffset = hitOffset;
    }

    #endregion

    #region Private Methods

    private void SetOrbDirection()
    {
        if (enemy != null)
        {
            Vector3 hitPos = new Vector3(enemy.position.x, enemy.position.y + hitOffset.y, enemy.position.z);
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