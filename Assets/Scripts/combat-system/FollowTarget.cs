using UnityEngine;
using EZCameraShake;

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
    private AudioClip hitSfx;
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private bool hasKnockback = false;

    private Transform mainCamera;
    private Vector3 camForwardDir;
    private Vector3 camBackwardDir;

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

    private int linkedAchievementID;

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
                if (hasKnockback)
                    enemyHit.SetKnockback(this.transform.position);
                audioSource.pitch = Random.Range(0.3f, 0.8f);
                SoundManager.instance.PlaySfxClip(audioSource, hitSfx,true);
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
    }

    public void Fire(Transform enemyTransform, Vector3 hitOffset)
    {
        ++player.basicAttacksCount;
        player.SetIsBoomerangOn(true);

        linkedAchievementID =  Achievements.instance.InstanceNewAchievement(Achievements.instance.GetMarksman());
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
                    CameraShaker.Instance.ShakeOnce(0.2f, 3.5f, 0.1f, 0.3f);
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
                Achievements.instance.DestroyAchievementInstantiation(AchievementType.CONSECUTIVEHITTING, linkedAchievementID);
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
