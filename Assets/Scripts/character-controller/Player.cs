using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    public delegate void VoidCallback();

    [System.Serializable]
    public class CooldownInfo
    {
        public float cooldownTime;
        public CooldownUI cooldownUI;
        [ShowOnly]
        public float timeSinceLastAction;
    }

    #region Fields
    [Header("Tutorial Events")]
    [SerializeField]
    private int enemyDamageIndex = -1;
    [SerializeField]
    private int cuteGroundIndex = -1;
    [SerializeField]
    private int healthMechanicsIndex = -1;

    [Header("Movement")]
    [ShowOnly]
    public TeleportTarget currentTelepotTarget;
    public ParticleSystem footStepsVFX;
    public float floorClearance;
    [HideInInspector]
    public bool canMove;
    [HideInInspector]
    public Vector3 currentSpeed;
    [HideInInspector]
    public Vector3 lastValidPosition;

    private VoidCallback expelCallback = null;

    [Header("Dash")]
    public CooldownInfo dashCooldown;
    public float dashDistance = 10.0f;
    public float dashDuration = 0.25f;
    [HideInInspector]
    public Vector3 dashDirection;
    [HideInInspector]
    public float dashElapsedTime = 0.0f;
    [HideInInspector]
    public float dashRemainingDistance;

    [Header("Zone change")]
    public AIZoneController startingZone;
    [ShowOnly]
    [SerializeField]
    private AIZoneController currentZoneController;
    public float knockbackForce = 25.0f;
    [SerializeField]
    private ParticleSystem knockbackVFX;
    [HideInInspector]
    public float knockbackCurrentForce;
    [HideInInspector]
    public Vector3 knockbackDirection;
    [HideInInspector]
    public bool knockbackActive = false;

    [Header("Health")]
    [SerializeField]
    private float baseHealth = 200.0f;
    [Tooltip("The time (in seconds) that must pass after recieving an attack before auto-healing starts.")]
    public float autoHealDelay = 2.0f;
    [Tooltip("The time (in seconds) it takes for the full health to be recover by auto-healing (IF the player is still alive).")]
    public float fullAutoHealDuration = 2.0f;
    [Tooltip("The time (in seconds) it takes for the full health to be recover by post-death healing.")]
    public float recoveryDuration = 5.0f;
    [ShowOnly]
    public bool isTargetable = true;
    public InfoPanel cuteGroundsInfoPanel;

    [HideInInspector]
    public float elapsedRecoveryTime;
    [HideInInspector]
    public float elapsedDelayTime;
    [HideInInspector]
    public bool isGrounded = false;
    [HideInInspector]
    public float timeSinceLastHit;
    [SerializeField]
    [ShowOnly]
    private float currentHealth;

    [Header("Damage Testing")]
    public float healthToReduce = 100;
    public bool hit;

    [HideInInspector]
    public Rigidbody rb;
    [HideInInspector]
    public Vector3 initialBulletSpawnPointPos;
    private Renderer[] renderers;
    private Collider[] colliders;
    [HideInInspector]
    public Animator animator;

    /* Teleport variables */
    [HideInInspector]
    public bool teleported;
    [HideInInspector]
    public float timeSinceLastTeleport;
    [HideInInspector]
    public JumpStates teleportState;

    [Header("Attacks")]
    [SerializeField]
    public Transform bulletSpawnPoint;
    public ParticleSystem shootBirth;

    [HideInInspector]
    public CooldownInfo[] cooldownInfos;

    private float timeSinceLastMonumentChecking = 0f;
    private float checkingMonumentRepetitionTime = 1f;

    private Monument[] allMonuments;
    [HideInInspector]
    public Monument monument;

    [Header("Player States")]
    [SerializeField]
    private State defaultState;
    [SerializeField]
    private State stoppedState;
    [SerializeField]
    private State teleportExpelState;
    [ShowOnly]
    [SerializeField]
    private State currentState;
    public CameraState cameraState;
    public Camera mainCamera;
    [HideInInspector]
    public CameraController mainCameraController;
    private float lastTransitionTime = -1.0f;

    [Header("Basic Attacks")]
    [HideInInspector]
    public float timeSinceLastAttack;
    [HideInInspector]
    public int basicAttacksCount = 0;
    [HideInInspector]
    public AIEnemy currentBasicAttackTarget = null;
    [HideInInspector]
    public bool animatingAttack = false;
    [HideInInspector]
    public Vector3 weakAttackTargetHitOffset;
    [HideInInspector]
    public Transform weakAttackTargetTransform;
    private bool isBoomerangOn = false;

    [Header("Strong Attack")]
    public CooldownInfo strongAttackCooldown;
    public StrongAttackDetection strongAttackCollider;
    public GameObject strongAttackMotionLimiter;
    [HideInInspector]
    public float strongAttackTimer;
    [HideInInspector]
    public List<AIEnemy> currentStrongAttackTargets = new List<AIEnemy>();
    [HideInInspector]
    public bool comeBackFromStrongAttack;
    [HideInInspector]
    public bool canChargeStrongAttack;
    [HideInInspector]
    public bool isChargingStrongAttack;

    [Header("Cone Attack")]
    public CooldownInfo coneAttackCooldown;
    public ParticleSystem coneAttackVFX;
    public ParticleSystem coneAttackFollowVFX;
    public int coneAttacklinkedAchievementID = 0;
    [HideInInspector]
    public bool comeBackFromConeAttack;

    [Header("Mine Attack")]
    public CooldownInfo mineAttackCooldown;
    public ParticleSystem minePrefab;
    public int maxCurrentMinesNumber;
    public float timeToGetAnotherMine;
    [HideInInspector]
    public float timeSinceLastMine;
    public ActivateMineExplosion[] mines;
    private int maxTotalMinesNumber = 100;
    [ShowOnly]
    [SerializeField]
    private int availableMinesNumber;
    [SerializeField]
    private MineCounterUI mineCounterUI;

    [Header("Meteorite Attack")]
    public CooldownInfo meteoriteAttackCooldown;

    private bool isMeteoritesOn = false;

    [Header("Sounds")]
    public AudioClip[] footstepsSFX;
    public AudioClip knockbackSFX;
    public AudioSource audioSource;
    public AudioSource quietSource;
    public Transform leftFoot, rightFoot;
    [HideInInspector]
    public int footstepID;

    #endregion

    public enum CameraState { STILL, MOVE, STRONG_ATTACK, TRANSITION, ZOOMOUT, ZOOMIN, METEORITEAIM, CONEATTACK, DASH, DEATH}
    public enum JumpStates { JUMP, MOVE, LAND, DELAY}
    

    #region MonoBehaviour Methods
    private void Awake() 
    {
        UnityEngine.Assertions.Assert.IsNotNull(startingZone, "ERROR: startingZone (AIZoneController) not assigned for Player in gameObject '" + gameObject.name + "'");
        currentZoneController = startingZone;

        strongAttackMotionLimiter.SetActive(false);

        cameraState = CameraState.MOVE;
        mainCameraController = mainCamera.GetComponent<CameraController>();
        teleportState = JumpStates.JUMP;
        initialBulletSpawnPointPos = new Vector3(0.8972f, 1.3626f, 0.1209f);

        renderers = this.GetComponentsInChildren<Renderer>();

        List<Collider> colls = new List<Collider>();
        this.GetComponentsInChildren<Collider>(colls);
        colls.RemoveAll((Collider coll) => coll.isTrigger);
        colliders = colls.ToArray();

        mines = new ActivateMineExplosion[maxTotalMinesNumber];
        rb = this.GetComponent<Rigidbody>();
        animator = this.GetComponent<Animator>();

        GameObject[] allMonumentsGameObjects = GameObject.FindGameObjectsWithTag("Monument");
        allMonuments = new Monument[allMonumentsGameObjects.Length];
        for (int i = 0; i < allMonuments.Length; ++i)
        {
            allMonuments[i] = allMonumentsGameObjects[i].GetComponent<Monument>();
        }

        UpdateNearestMonument();

        canMove = true;
        comeBackFromStrongAttack = false;
        comeBackFromConeAttack = false;
        canChargeStrongAttack = false;
        isChargingStrongAttack = false;

        currentState = stoppedState;

        cooldownInfos = new CooldownInfo[] { dashCooldown, coneAttackCooldown, strongAttackCooldown, meteoriteAttackCooldown, mineAttackCooldown };
    }

    private void Start () 
    {
        timeSinceLastAttack = 1000.0f;
        timeSinceLastTeleport = 0.0f;
        teleported = false;
        currentHealth = baseHealth;
        timeSinceLastHit = autoHealDelay;
        UIManager.instance.SetPlayerHealth(1.0f);
        availableMinesNumber = maxCurrentMinesNumber;
        timeSinceLastMine = 0.0f;

        dashCooldown.timeSinceLastAction = dashCooldown.cooldownTime;
        coneAttackCooldown.timeSinceLastAction = coneAttackCooldown.cooldownTime;
        strongAttackCooldown.timeSinceLastAction = strongAttackCooldown.cooldownTime;
        meteoriteAttackCooldown.timeSinceLastAction = meteoriteAttackCooldown.cooldownTime;
        mineAttackCooldown.timeSinceLastAction = mineAttackCooldown.cooldownTime;

        mineCounterUI.SetCurrentCount(availableMinesNumber);
        mineCounterUI.SetTotalCount(availableMinesNumber);

        currentState.EnterState(this);

        // Quick fix. More info above ReEnable method.
        enabled = false;
        Invoke("ReEnable", 0);
    }

    private void Update()
    {
        if ( timeSinceLastMonumentChecking >= checkingMonumentRepetitionTime)
        {
            UpdateNearestMonument();
            timeSinceLastMonumentChecking -= checkingMonumentRepetitionTime;
        }

        timeSinceLastMonumentChecking += Time.deltaTime;
        timeSinceLastAttack += Time.deltaTime;
        strongAttackTimer += Time.deltaTime;

        if (GameManager.instance.CanUpdatePlayer() && lastTransitionTime != Time.time)
        {
            currentState.UpdateState(this);
        }

        // Testing
        if (hit)
        {
            hit = false;
            TakeDamage(healthToReduce, AttackType.NONE);
        }
    }
    #endregion

    #region Public Methods
    public void SetZoneController(AIZoneController zoneController, Vector3 knockbackDirection = default(Vector3))
    {
        if (!zoneController.isConquered)
        {
            currentZoneController = zoneController;
        }
        else
        {
            knockbackActive = true;
            SoundManager.instance.PlaySfxClip(knockbackSFX);
            ParticlesManager.instance.LaunchParticleSystem(knockbackVFX, this.transform.position + Vector3.up * 1.55f, knockbackVFX.transform.rotation);
            knockbackCurrentForce = knockbackForce;
            this.knockbackDirection = knockbackDirection.normalized;
        }
    }

    public void ExpelFromZone(AIZoneController sourceZone, TeleportTarget teleportTarget, VoidCallback postExpelCallback = null)
    {
        if (currentZoneController == sourceZone)
        {
            currentTelepotTarget = teleportTarget;
            currentZoneController = teleportTarget.zoneController;
            TransitionToState(teleportExpelState);
            expelCallback = postExpelCallback;
        }
        else
        {
            if (postExpelCallback != null)
                postExpelCallback();
        }
    }

    public void InstantiateMine()
    {
        availableMinesNumber--;
        if (availableMinesNumber == 0)        
            mineCounterUI.SetNoAvailableMines(true);
        
        mineCounterUI.SetCurrentCount(availableMinesNumber);
        for (int i = 0; i < maxTotalMinesNumber; i++)
        {
            if (mines[i] == null)
            {
                mines[i] = ParticlesManager.instance.LaunchParticleSystem(minePrefab, this.transform.position, minePrefab.transform.rotation).GetComponent<ActivateMineExplosion>();
                return;
            }
        }
        mines[0].DestroyMine();
        mines[0] = null;
        SortMines();
        mines[maxTotalMinesNumber - 1] = ParticlesManager.instance.LaunchParticleSystem(minePrefab, this.transform.position, minePrefab.transform.rotation).GetComponent<ActivateMineExplosion>();
            Debug.Log(availableMinesNumber);
       
    }


    public void RemoveMine(ActivateMineExplosion mineToRemove)
    {
        for(int i = 0; i < maxTotalMinesNumber; i++)
        {
            if(mines[i] == mineToRemove)
            {
                mines[i] = null;
                break;
            }
        }
        SortMines();
    }

    public void GetNewMine()
    {
        if (availableMinesNumber < maxCurrentMinesNumber)
        {
            availableMinesNumber++;
            mineCounterUI.SetCurrentCount(availableMinesNumber);
            mineCounterUI.SetNoAvailableMines(false);
        }
    }
    
    public int GetAvailableMines()
    {
        return availableMinesNumber;
    }

    public void SetPercentageToNextMine(float percentage)
    {
        mineCounterUI.SetPercentageToNextMine(percentage);
    }

    public void SetCurrentHealth(float normalizedHealth)
    {
        if (normalizedHealth < 0 || normalizedHealth > 1)
        {
            Debug.LogError("ERROR: Player.SetCurrentHealth called with an argument outside of range [0, 1]!");
            return;
        }

        currentHealth = baseHealth * normalizedHealth;
        UIManager.instance.SetPlayerHealth(normalizedHealth);
    }

    // IDamageable
    public float GetMaxHealth()
    {
        return baseHealth;
    }

    // IDamageable
    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    // IDamageable
    public bool IsDead()
    {
        return isGrounded;
    }

    // IDamageable
    public void TakeDamage(float damage, AttackType attackType)
    {
        if (isGrounded)
            return;

        LaunchTutorialEvents(attackType);

        timeSinceLastHit = 0;
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            PlayerGrounded();
        }
        UIManager.instance.SetPlayerHealth(currentHealth / baseHealth);
    }

    // IDamageable
    public bool IsTargetable()
    {
        return isTargetable;
    }

    public virtual void TransitionToState(State targetState)
    {
        currentState.ExitState(this);
        targetState.EnterState(this);
        currentState = targetState;
        lastTransitionTime = Time.time;

        if (expelCallback != null)
        {
            VoidCallback callback = expelCallback;
            expelCallback = null;
            callback();
        }
    }

    public void SetRenderersVisibility(bool visible)
    {
        for(int i = 0; i < renderers.Length; i++)
        {
            renderers[i].enabled = visible;
        }
        SetCollidersActiveState(visible);
    }

    public void SetCollidersActiveState(bool isActive)
    {
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = isActive;
        }
    }

    public void InstantiateAttack(ParticleSystem attackPrefab, Transform enemy, Vector3 hitOffset)
    {
        Vector3 spawningPos = bulletSpawnPoint.position;

        ParticleSystem attack = ParticlesManager.instance.LaunchParticleSystem(attackPrefab, spawningPos, transform.rotation);
        FollowTarget attackClone = attack.GetComponent<FollowTarget>();
        attackClone.Fire(enemy, hitOffset);
    }

    public void OnRoundStarted()
    {
        TransitionToState(defaultState);
    }

    public void OnRoundOver()
    {
        TransitionToState(stoppedState);
    }

    public void SetIsMeteoritesOn(bool meteorites)
    {
        isMeteoritesOn = meteorites;
    }

    public void SetIsBoomerangOn(bool boomerang)
    {
        isBoomerangOn = boomerang;
    }

    public bool GetIsBoomerangOn()
    {
        return isBoomerangOn;
    }

    public bool GetIsMeteoritesOn()
    {
        return isMeteoritesOn;
    }

    public void FootStep(int i)
    {
        audioSource.pitch = Random.Range(0.935f, 1.065f);
        SoundManager.instance.PlaySfxClip(quietSource, footstepsSFX[footstepID], true);
        footstepID++;
        if(i == 1)
        {
            ParticlesManager.instance.LaunchParticleSystem(footStepsVFX, leftFoot.position, footStepsVFX.transform.rotation);
        }
        else
        {
            ParticlesManager.instance.LaunchParticleSystem(footStepsVFX, rightFoot.position, footStepsVFX.transform.rotation);
        }
        if (footstepID >= footstepsSFX.Length)
        {
            footstepID = 1;
        }
    }

    public void SetConeAttackLinkedAchievementID(int value)
    {
        coneAttacklinkedAchievementID = value;
    }

    public int GetConeAttackLinkedAchievementID()
    {
        return coneAttacklinkedAchievementID;
    }

    public void IncreaseStrongAttackColliderSize(float increase)
    {
        strongAttackCollider.IncreaseSize(increase);
    }

    public void ResetStrongAttackColliderSize()
    {
        strongAttackCollider.ResetSize();
    }

    public void ChangeDecalColor(float time)
    {
        strongAttackCollider.ChangeDecalColor(time);
    }

    #endregion

    #region Private Methods
    private void LaunchTutorialEvents(AttackType attackType)
    {
        if (attackType == AttackType.ENEMY)
        {
            GameManager.instance.LaunchTutorialEvent(enemyDamageIndex, () =>
            {
                GameManager.instance.LaunchTutorialEvent(healthMechanicsIndex);
            });
        }
        else if (attackType == AttackType.CUTE_AREA)
        {
            GameManager.instance.LaunchTutorialEvent(cuteGroundIndex, () =>
            {
                GameManager.instance.LaunchTutorialEvent(healthMechanicsIndex);
            });
            
        }
    }

    private void SortMines()
    {
        for(int i = 0; i < maxTotalMinesNumber - 1; i++)
        {
            if(mines[i] == null)
            {
                mines[i] = mines[i + 1];
                mines[i + 1] = null;
            }
        }
    }

    // Used for a quick fix on a bug where the player wouldn't be able to move after reloading the Game scene
    private void ReEnable()
    {
        enabled = true;
    }

    private void UpdateNearestMonument()
    {
        float nearestMonumentDistance = float.MaxValue;
        float evaluatedMonumentDistance = 0;
        
        for(int i = 0; i < allMonuments.Length; i++)
        {
            evaluatedMonumentDistance = Vector3.Distance(this.transform.position, allMonuments[i].transform.position);
            if (evaluatedMonumentDistance < nearestMonumentDistance)
            {
                nearestMonumentDistance = evaluatedMonumentDistance;
                monument = allMonuments[i];
            }
        }
    }

    private void PlayerGrounded()
    {
        isGrounded = true;
    }
    #endregion
}
