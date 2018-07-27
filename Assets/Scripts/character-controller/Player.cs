using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour, IDamageable {

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

    [Header("Zone change")]
    public AIZoneController startingZone;
    [ShowOnly]
    [SerializeField]
    private AIZoneController currentZoneController;
    [SerializeField]
    private float knockbackForce = 25.0f;
    [SerializeField]
    private ParticleSystem knockbackVFX;
    private float knockbackCurrentForce;
    private Vector3 knockbackDirection;
    private bool knockbackActive = false;

    [Header("Health")]
    [SerializeField]
    private float baseHealth = 200.0f;
    public float autoHealDelay = 2.0f;
    public float fullAutoHealDuration = 2.0f;
    public float recoveryDuration = 5.0f;
    public float timeSavedPerClick = 0.5f;
    [ShowOnly]
    public bool isTargetable = true;

    [HideInInspector]
    public float elapsedRecoveryTime = 0.0f;
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

    [Header("Evilness")]
    [SerializeField]
    EvilManaController evilManaController;
    [SerializeField]
    private float maxEvilLevel;
    public float evilLevel;
    [SerializeField][Range(0,1)]
    private float autoEvilRecoveringTime;
    [SerializeField]
    [Range(0, 0.5f)]
    private float autoEvilRecoveringValue;
    
    private bool isAutoRecoveringEvil = false;
    private float lastAutoEvilRecovering = 0;
    

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
    public float strongAttackStateCooldown;
    public SphereCollider strongAttackCollider;
    [HideInInspector]
    public float timeSinceLastStrongAttack;
    [HideInInspector]
    public List<AIEnemy> currentStrongAttackTargets = new List<AIEnemy>();
    [HideInInspector]
    public Transform initialPositionOnStrongAttack;
    [HideInInspector]
    public bool comeBackFromStrongAttack;

    [Header("Cone Attack")]
    public ParticleSystem coneAttackVFX;
    public float coneAttackEvilCost;
    [HideInInspector]
    public bool comeBackFromConeAttack;
    [HideInInspector]
    public float timeSinceLastConeAttack = 0;

    [Header("Mine Attack")]
    public ParticleSystem minePrefab;
    public int maxMinesNumber;
    public int availableMinesNumber;
    public float timeToGetAnotherMine;
    [HideInInspector]
    public float timeSinceLastMine;
    public ActivateMineExplosion[] mines;

    [Header("Meteorite Attack")]
    public GameObject meteoriteDestinationMarker;
    public Vector3 initialPos;
    public float meteoriteCooldown;
    [HideInInspector]
    public float timeSinceLastMeteoriteAttack;
    [HideInInspector]
    public bool comeBackFromMeteoriteAttack;
    [HideInInspector]
    public Vector3 lastMeteoriteAttackDestination;
    private bool isMeteoritesOn = false;
    public Transform[] meteoritesPlayerPosition;
    public Transform[] meteoritesReturnPlayerPosition;
    public AIZoneController[] meteoriteZones;
    public int currentZonePlaying = 0;


    [Header("Footsteps")]
    public AudioClip footstepsClip;
    public AudioSource loopAudioSource;
    public AudioSource oneShotAudioSource;

    #endregion

    public enum CameraState { STILL, MOVE, STRONG_ATTACK, TRANSITION, ZOOMOUT, ZOOMIN, METEORITEAIM}
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

        loopAudioSource.loop = true;
        oneShotAudioSource.loop = false;

        canMove = true;
        comeBackFromStrongAttack = false;
        comeBackFromConeAttack = false;

        evilLevel = maxEvilLevel;
        currentState = defaultState;
    }

    private void Start () 
    {
        isAutoRecoveringEvil = true;

        footSteps.SetActive(false);

        timeSinceLastAttack = 1000.0f;
        timeSinceLastStrongAttack = 1000.0f;
        timeSinceLastTeleport = 0.0f;
        teleported = false;
        currentHealth = baseHealth;
        timeSinceLastHit = autoHealDelay;
        UIManager.instance.SetPlayerHealth(1.0f);
        availableMinesNumber = maxMinesNumber;
        timeSinceLastMine = 0.0f;

        currentState.EnterState(this);

        // Quick fix. More info above ReEnable method.
        enabled = false;
        Invoke("ReEnable", 0);
    }

    private void Update()
    {
        if (GameManager.instance.gameIsPaused)
        {
            loopAudioSource.Stop();
            return;
        }
        else
        {
            EvilAutoRecovering();
        }

        if ( timeSinceLastMonumentChecking >= checkingMonumentRepetitionTime)
        {
            UpdateNearestMonument();
            timeSinceLastMonumentChecking -= checkingMonumentRepetitionTime;
        }

        timeSinceLastAttack += Time.deltaTime;
        timeSinceLastStrongAttack += Time.deltaTime;
        timeSinceLastMonumentChecking += Time.deltaTime;
        timeSinceLastMeteoriteAttack += Time.deltaTime;

        if (knockbackActive)
        {
            Knockback();
        }

        if (lastTransitionTime != Time.time && !knockbackActive)
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
            ParticlesManager.instance.LaunchParticleSystem(knockbackVFX, this.transform.position + Vector3.up * 1.55f, knockbackVFX.transform.rotation);
            knockbackCurrentForce = knockbackForce;
            this.knockbackDirection = knockbackDirection;
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
            }
        }
        SortMines();
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

    public virtual void TransitionToState(State targetState)
    {
        currentState.ExitState(this);
        targetState.EnterState(this);
        currentState = targetState;
        lastTransitionTime = Time.time;
    }

    public float GetMaxEvilLevel()
    {
        return maxEvilLevel;
    }

    public float GetEvilLevel()
    {
        return evilLevel;
    }

    public void AddEvilPoints(float value)
    {
        if (value < 0 || (isAutoRecoveringEvil && evilLevel < maxEvilLevel))
        {
            evilLevel += value;

            if (evilLevel < 0)
            {
                evilLevel = 0;
            }
            else if (evilLevel > maxEvilLevel)
            {
                evilLevel = maxEvilLevel;
            }
            evilManaController.ModifyEvil(evilLevel);
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
        strongAttackCollider.enabled = true;
    }

    public void InstantiateAttack(ParticleSystem attackPrefab, Transform enemy, Vector3 hitOffset)
    {
        Vector3 spawningPos = bulletSpawnPoint.position;

        ParticleSystem attack = ParticlesManager.instance.LaunchParticleSystem(attackPrefab, spawningPos, transform.rotation);
        FollowTarget attackClone = attack.GetComponent<FollowTarget>();
        attackClone.Fire(enemy, hitOffset);
    }

    public bool GetIsAutoRecoveringEvil()
    {
        return isAutoRecoveringEvil;
    }

    public void SetIsAutoRecoveringEvil(bool isRecovering)
    {
        isAutoRecoveringEvil = isRecovering;
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

    private void Knockback()
    {
        if (knockbackActive)
        {
            knockbackCurrentForce = Mathf.Lerp(knockbackCurrentForce, 0.0f, 0.5f);
            rb.position += knockbackDirection * knockbackForce * Time.deltaTime;
            if (knockbackCurrentForce < 0.2f)
            {
                knockbackActive = false;
            }
        }
    }

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

    private void EvilAutoRecovering()
    {
        lastAutoEvilRecovering += Time.deltaTime;

        if(lastAutoEvilRecovering >= autoEvilRecoveringTime)
        {
            AddEvilPoints(autoEvilRecoveringValue);
            lastAutoEvilRecovering = 0;
        }
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
