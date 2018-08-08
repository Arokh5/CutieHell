using UnityEngine;

public class FollowTarget : PooledParticleSystem
{
    #region Fields

    [Header("General")]
    public AttackType attackType;
    public LayerMask hitLayerMask;
    [SerializeField]
    private float lifeTime;
    [SerializeField]
    private float damage;
    [SerializeField]
    private AudioClip explosionSfx;

    private Transform mainCamera;
    private Vector3 camForwardDir;
    private Vector3 camBackwardDir;
    private AIEnemy enemy;
    private Transform enemyTransform;
    private Vector3 hitOffset;

    [Header("Motion")]
    [SerializeField]
    private float goWaySpeed = 20f;
    [SerializeField]
    private float returnWaySpeed = 25f;
    [SerializeField]
    private float maxDistance = 12f;
    [SerializeField]
    private float goWayFinalWaitTime = 0.5f;
    [SerializeField]
    private ParticleSystem impulseMotionVFX;

    [Header("Ground avoidance")]
    [SerializeField]
    private bool hugGround = false;
    [SerializeField]
    private float minGroundClearance = 1.0f;
    [SerializeField]
    private LayerMask walkableLayer;

    private Vector3 initPos;
    private float time;

    private enum AttackStates { GoWay, ReturnWay, Stay };
    private AttackStates attackState;
    private Player player;

    #endregion

    #region MonoBehaviour Methods

    private void Awake()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
        player = GameManager.instance.GetPlayer1();
    }

    private void Update()
    {
        SetOrbDirection();
        DestroyOrb();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Helpers.GameObjectInLayerMask(other.gameObject, hitLayerMask))
        {
            AIEnemy enemyHit = other.GetComponent<AIEnemy>();
            if (enemyHit)
            {
                enemyHit.TakeDamage(damage, attackType);
                enemyHit.SetKnockback(this.transform.position);
                SoundManager.instance.PlaySfxClip(explosionSfx);
            }
        }
    }

    #endregion

    #region Public Methods

    public override void Restart()
    {
        time = 0f;
        initPos = transform.position;
        attackState = AttackStates.GoWay;
        camForwardDir = transform.InverseTransformDirection(mainCamera.forward);
        enemyTransform = null;
        enemy = null;
        hitOffset = Vector3.zero;
    }

    public void Fire(Transform enemyTransform, Vector3 hitOffset)
    {
        this.enemyTransform = enemyTransform;
        if (enemyTransform)
            this.enemy = enemyTransform.GetComponent<AIEnemy>();

        this.hitOffset = hitOffset;
        ++player.basicAttacksCount;
        player.SetIsBoomerangOn(true);
    }

    #endregion

    #region Private Methods

    private void SetOrbDirection()
    {
        switch (attackState)
        {
            case AttackStates.GoWay:
                if (Vector3.Distance(initPos, transform.position) < maxDistance)
                {
                    transform.Translate(camForwardDir * goWaySpeed * Time.deltaTime);
                    CheckGroundClearance();
                }
                else
                {
                    attackState = AttackStates.Stay;
                    ParticlesManager.instance.LaunchParticleSystem(impulseMotionVFX, this.transform.position, Quaternion.LookRotation(player.transform.position - this.transform.position));
                }
                break;

            case AttackStates.Stay:
                time += Time.deltaTime;
                if (time >= goWayFinalWaitTime)
                {
                    
                    attackState = AttackStates.ReturnWay;
                }
                break;

            case AttackStates.ReturnWay:
                transform.position = Vector3.MoveTowards(transform.position, GameManager.instance.GetPlayer1().transform.position, returnWaySpeed * Time.deltaTime);
                CheckGroundClearance();
                break;
        }
    }

    private void DestroyOrb()
    {
        if (attackState == AttackStates.ReturnWay)
        {
            if (Vector3.Distance(GameManager.instance.GetPlayer1().transform.position, transform.position) < 1f)
            {
                --player.basicAttacksCount;
                ReturnToPool();
                player.SetIsBoomerangOn(false);
            }
        }
    }

    private void CheckGroundClearance()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, 20, walkableLayer))
        {
            bool tooCloseToGround = minGroundClearance - hit.distance > 0.01f;
            bool tooFarFromGround = minGroundClearance - hit.distance < -0.01f;
            if ( tooCloseToGround || hugGround && tooFarFromGround)
            {
                transform.position += Vector3.up * (minGroundClearance - hit.distance);
            }
        }
    }

    #endregion
}
