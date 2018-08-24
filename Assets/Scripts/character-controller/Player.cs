using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    [System.Serializable]
    public class CooldownInfo
    {
        public float cooldownTime;
        public CooldownUI cooldownUI;
        [ShowOnly]
        public float timeSinceLastAction;
    }

    #region Fields
    [Header("Movement Variabes")]
    [ShowOnly]
    public TeleportTarget currentTelepotTarget;
    public Transform[] teleportTargets;
    public GameObject footSteps;

    [Header("Movement")]
    public float floorClearance;
    [HideInInspector]
    public bool canMove;
    [HideInInspector]
    public Vector3 currentSpeed;
    [HideInInspector]
    public Vector3 lastValidPosition;

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
    [Tooltip("The time (in seconds) that is reduced from the recoveryDuration per button mash.")]
    public float timeSavedPerClick = 0.5f;
    [Tooltip("The delay (in seconds) after fully recovering health (post-death) and before control returns to the player.")]
    public float postRecoveryDelay = 0.5f;
    [ShowOnly]
    public bool isTargetable = true;

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
    public TeleportStates teleportState;

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
    [HideInInspector]
    public float strongAttackTimer;
    [HideInInspector]
    public List<AIEnemy> currentStrongAttackTargets = new List<AIEnemy>();
    [HideInInspector]
    public bool comeBackFromStrongAttack;

    [Header("Cone Attack")]
    public CooldownInfo coneAttackCooldown;
    public ParticleSystem coneAttackVFX;
    [HideInInspector]
    public bool comeBackFromConeAttack;

    [Header("Mine Attack")]
    public CooldownInfo mineAttackCooldown;
    public ParticleSystem minePrefab;
    public int maxMinesNumber;
    public int availableMinesNumber;
    public float timeToGetAnotherMine;
    [HideInInspector]
    public float timeSinceLastMine;
    public ActivateMineExplosion[] mines;
    [ShowOnly]
    [SerializeField]
    private int minesCount = 0;
    [SerializeField]
    private MineCounterUI mineCounterUI;

    [Header("Meteorite Attack")]
    public CooldownInfo meteoriteAttackCooldown;

    private bool isMeteoritesOn = false;
    
    [Header("Sounds")]
    public AudioClip[] footstepsSFX;
    public AudioClip knockbackSFX;
    public AudioSource audioSource;

    #endregion

    public enum CameraState { STILL, MOVE, STRONG_ATTACK, TRANSITION, ZOOMOUT, ZOOMIN, METEORITEAIM, CONEATTACK, DASH}
    public enum TeleportStates { OUT, TRAVEL, IN, DELAY}
    

    #region MonoBehaviour Methods
    private void Awake() 
    {
        UnityEngine.Assertions.Assert.IsNotNull(startingZone, "ERROR: startingZone (AIZoneController) not assigned for Player in gameObject '" + gameObject.name + "'");
        currentZoneController = startingZone;

        cameraState = CameraState.MOVE;
        mainCameraController = mainCamera.GetComponent<CameraController>();
        teleportState = TeleportStates.OUT;
        initialBulletSpawnPointPos = new Vector3(0.8972f, 1.3626f, 0.1209f);

        renderers = this.GetComponentsInChildren<Renderer>();

        List<Collider> colls = new List<Collider>();
        this.GetComponentsInChildren<Collider>(colls);
        colls.RemoveAll((Collider coll) => coll.isTrigger);
        colliders = colls.ToArray();

        mines = new ActivateMineExplosion[maxMinesNumber];
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

        currentState = defaultState;

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
        availableMinesNumber = maxMinesNumber;
        timeSinceLastMine = 0.0f;

        dashCooldown.timeSinceLastAction = dashCooldown.cooldownTime;
        coneAttackCooldown.timeSinceLastAction = coneAttackCooldown.cooldownTime;
        strongAttackCooldown.timeSinceLastAction = strongAttackCooldown.cooldownTime;
        meteoriteAttackCooldown.timeSinceLastAction = meteoriteAttackCooldown.cooldownTime;
        mineAttackCooldown.timeSinceLastAction = mineAttackCooldown.cooldownTime;

        mineCounterUI.SetCurrentCount(0);
        mineCounterUI.SetTotalCount(maxMinesNumber);

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

        if (!GameManager.instance.gameIsPaused && lastTransitionTime != Time.time)
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
            SoundManager.instance.PlaySfxClip(audioSource, knockbackSFX, true);
            ParticlesManager.instance.LaunchParticleSystem(knockbackVFX, this.transform.position + Vector3.up * 1.55f, knockbackVFX.transform.rotation);
            knockbackCurrentForce = knockbackForce;
            this.knockbackDirection = knockbackDirection.normalized;
        }
    }

    public void ExpelFromZone(AIZoneController sourceZone, TeleportTarget teleportTarget)
    {
        if (currentZoneController == sourceZone)
        {
            currentTelepotTarget = teleportTarget;
            currentZoneController = teleportTarget.zoneController;
            TransitionToState(teleportExpelState);
        }
    }

    public void InstantiateMine()
    {
        availableMinesNumber--;
        for (int i = 0; i < maxMinesNumber; i++)
        {
            if (mines[i] == null)
            {
                mines[i] = ParticlesManager.instance.LaunchParticleSystem(minePrefab, this.transform.position, minePrefab.transform.rotation).GetComponent<ActivateMineExplosion>();
                ++minesCount;
                mineCounterUI.SetCurrentCount(minesCount);
                return;
            }
        }
        mines[0].DestroyMine();
        mines[0] = null;
        SortMines();
        mines[maxMinesNumber - 1] = ParticlesManager.instance.LaunchParticleSystem(minePrefab, this.transform.position, minePrefab.transform.rotation).GetComponent<ActivateMineExplosion>();
    }

    public void RemoveMine(ActivateMineExplosion mineToRemove)
    {
        for(int i = 0; i < maxMinesNumber; i++)
        {
            if(mines[i] == mineToRemove)
            {
                mines[i] = null;
                --minesCount;
                mineCounterUI.SetCurrentCount(minesCount);
                break;
            }
        }
        SortMines();
    }

    public int getMinesCount()
    {
        return minesCount;
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
    public void TakeDamage(float damage, AttackType attacktype)
    {
        if (isGrounded)
            return;

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
    #endregion

    #region Private Methods
    private void SortMines()
    {
        for(int i = 0; i < maxMinesNumber - 1; i++)
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
