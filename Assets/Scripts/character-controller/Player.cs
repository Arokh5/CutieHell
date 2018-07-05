using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour, IDamageable {

    #region Fields
    [Header("Movement Variabes")]
    public Transform[] teleportTargets;
    public GameObject footSteps;

    [Header("Movement")]
    [HideInInspector]
    public bool canMove;
    public float floorClearance;
    [HideInInspector]
    public Vector3 currentSpeed;
    [HideInInspector]
    public Vector3 lastValidPosition;

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
    [HideInInspector]
    public float timeSinceLastTrapUse;
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

    [Header("Actual Trap")]
    [HideInInspector]
    public Trap[] allTraps;
    public Trap nearbyTrap;
    [HideInInspector]
    public bool currentlyUsingTrap = false;
    public Trap currentTrap;
    public int trapUseCooldown;
    public float trapMaxUseDistance;
    [HideInInspector]
    public bool shouldExitTrap = false;

    public ZoneTrap zoneTrap = null;

    [Header("Player States")]
    [SerializeField]
    private State defaultState;
    [ShowOnly]
    [SerializeField]
    private State currentState;
    public CameraState cameraState;
    public Camera mainCamera;
    [HideInInspector]
    public CameraController mainCameraController;

    [Header("Basic Attacks")]
    [HideInInspector]
    public float timeSinceLastAttack;
    [HideInInspector]
    public AIEnemy currentBasicAttackTarget = null;
    [HideInInspector]
    public bool animatingAttack = false;
    [HideInInspector]
    public Vector3 weakAttackTargetHitOffset;
    [HideInInspector]
    public Transform weakAttackTargetTransform;

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
    public float strongAttackEvilCost;

    [Header("Cone Attack")]
    public ParticleSystem coneAttackVFX;
    public float coneAttackEvilCost;
    [HideInInspector]
    public bool comeBackFromConeAttack;
    [HideInInspector]
    public float timeSinceLastConeAttack = 0;

    [Header("Fog Attack")]
    public SphereCollider fogCollider;
    public GameObject fogVFX;
    public float fogStateCooldown;
    [HideInInspector]
    public float fogStateLastTime;
    [HideInInspector]
    public float accumulatedFogEvilCost = 0;
    [HideInInspector]
    public float timeSinceLastFogHit = 0;
    //[HideInInspector]
    public List<AIEnemy> currentFogAttackTargets = new List<AIEnemy>();
    [HideInInspector]
    public List<AIEnemy> toRemoveFogAttackTargets = new List<AIEnemy>();


    [Header("Footsteps")]
    public AudioClip footstepsClip;
    public AudioSource loopAudioSource;
    public AudioSource oneShotAudioSource;

    #endregion

    public enum CameraState { STILL, MOVE, WOLF, FOG, BATTURRET, CANONTURRET, TRANSITION, ZOOMOUT, ZOOMIN}
    public enum TeleportStates { OUT, TRAVEL, IN, DELAY}
    

    #region MonoBehaviour Methods
    private void Awake() 
    {
        cameraState = CameraState.MOVE;
        mainCameraController = mainCamera.GetComponent<CameraController>();
        teleportState = TeleportStates.OUT;
        initialBulletSpawnPointPos = new Vector3(0.8972f, 1.3626f, 0.1209f);

        renderers = this.GetComponentsInChildren<Renderer>();

        List<Collider> colls = new List<Collider>();
        this.GetComponentsInChildren<Collider>(colls);
        colls.RemoveAll((Collider coll) => coll.isTrigger);
        colliders = colls.ToArray();

        rb = this.GetComponent<Rigidbody>();
        animator = this.GetComponent<Animator>();
        GameObject[] allTrapsGameObjects = GameObject.FindGameObjectsWithTag("Traps");
        allTraps = new Trap[allTrapsGameObjects.Length];
        for (int i = 0; i < allTrapsGameObjects.Length; ++i)
        {
            allTraps[i] = allTrapsGameObjects[i].GetComponent<Trap>();
        }

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

        fogStateLastTime = float.MinValue;
        evilLevel = maxEvilLevel;
        currentState = defaultState;
    }

    private void Start () 
    {
        isAutoRecoveringEvil = true;

        footSteps.SetActive(false);

        timeSinceLastTrapUse = trapUseCooldown;
        timeSinceLastAttack = 1000.0f;
        timeSinceLastStrongAttack = 1000.0f;
        timeSinceLastTeleport = 0.0f;
        teleported = false;
        currentHealth = baseHealth;
        timeSinceLastHit = autoHealDelay;
        UIManager.instance.SetPlayerHealth(1.0f);

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
        }else
        {
            EvilAutoRecovering();
        }

        if ( timeSinceLastMonumentChecking >= checkingMonumentRepetitionTime)
        {
            UpdateNearestMonument();
            timeSinceLastMonumentChecking -= checkingMonumentRepetitionTime;
        }

        timeSinceLastTrapUse += Time.deltaTime;
        timeSinceLastAttack += Time.deltaTime;
        timeSinceLastStrongAttack += Time.deltaTime;
        timeSinceLastMonumentChecking += Time.deltaTime;
        currentState.UpdateState(this);

        // Testing
        if (hit)
        {
            hit = false;
            TakeDamage(healthToReduce, AttackType.NONE);
        }
    }
    #endregion

    #region Public Methods
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
            PlayerKilled();
        }
        UIManager.instance.SetPlayerHealth(currentHealth / baseHealth);
    }

    public virtual void TransitionToState(State targetState)
    {
        currentState.ExitState(this);
        targetState.EnterState(this);
        currentState = targetState;
    }

    public void StopTrapUse()
    {
        shouldExitTrap = true;
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
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = visible;
        }
        strongAttackCollider.enabled = true;
    }

    public void InstantiateAttack(ParticleSystem attackPrefab, Transform enemy, Vector3 hitOffset)
    {
        Vector3 spawningPos = bulletSpawnPoint.position;

        ParticleSystem attack = ParticlesManager.instance.LaunchParticleSystem(attackPrefab, spawningPos, transform.rotation);
        FollowTarget attackClone = attack.GetComponent<FollowTarget>();
        attackClone.SetEnemyTransform(enemy);
        attackClone.SetHitOffset(hitOffset);
    }

    public bool SetIsAutoRecoveringEvil()
    {
        return isAutoRecoveringEvil;
    }

    public void SetIsAutoRecoveringEvil(bool isRecovering)
    {
        isAutoRecoveringEvil = isRecovering;
    }

    public void OnRoundOver()
    {
        TransitionToState(defaultState);
    }
    #endregion

    #region Private Methods
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

    private void PlayerKilled()
    {
        isGrounded = true;
        Debug.Log("Player killed!");
    }
    #endregion
}
