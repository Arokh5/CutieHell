using UnityEngine;

public class FollowTarget : MonoBehaviour
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
    private Vector3 hitPoint;
    private float time = 0;

    #endregion

    #region Properties

    public void SetEnemy(Transform enemy)
    {
        this.enemy = enemy;
    }

    public void SetHitPoint(Vector3 hitPoint)
    {
        this.hitPoint = hitPoint;
    }

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
                enemyHit.TakeDamage(damage, AttackType.WEAK);
                Destroy(gameObject);
            }
            else if (attackType != AttackType.TRAP_BASIC)
            {
                StatsManager.instance.RegisterWeakAttackMissed();
            }
            Destroy(gameObject);
        }
    }

    #endregion

    #region Private Methods

    private void SetOrbDirection()
    {
        if (enemy != null)
        {
            float yOffset = Mathf.Abs(enemy.position.y - hitPoint.y);
            Vector3 hitPos = new Vector3(enemy.position.x, enemy.position.y + yOffset, enemy.position.z);

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
            StatsManager.instance.RegisterWeakAttackMissed();
            Destroy(gameObject);
        }
    }

	#endregion
}