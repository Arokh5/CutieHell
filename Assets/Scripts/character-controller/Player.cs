using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour {

    #region Fields
    [Header("Tutorial")]
    public TutorialController tutorialController;

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

    [Header("Evilness")]
    [SerializeField]
    private int maxEvilLevel = 50;
    public int evilLevel;

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
    [HideInInspector]
    public float timeSinceLastStrongAttack;
    [HideInInspector]
    public List<AIEnemy> currentStrongAttackTargets = new List<AIEnemy>();
    [HideInInspector]
    public Transform initialPositionOnStrongAttack;
    [HideInInspector]
    public bool comeBackFromStrongAttack;

    [Header("Fog Attack")]
    public SphereCollider fogCollider;
    public ParticleSystem fogVFX;
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
    public AudioSource footstepsSource;
    public AudioSource audioSource;

    #endregion

    public enum CameraState { STILL, MOVE, WOLF, FOG, BATTURRET, CANONTURRET, TRANSITION, ZOOMOUT, ZOOMIN}
    public enum TeleportStates { OUT, TRAVEL, IN}
    

    #region MonoBehaviour Methods
    private void Awake() 
    {
        cameraState = CameraState.MOVE;
        mainCameraController = mainCamera.GetComponent<CameraController>();
        teleportState = TeleportStates.OUT;
        initialBulletSpawnPointPos = new Vector3(0.8972f, 1.3626f, 0.1209f);

        renderers = this.GetComponentsInChildren<Renderer>();
        colliders = this.GetComponentsInChildren<Collider>();

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

        footstepsSource = GetComponent<AudioSource>();
        footstepsSource.clip = footstepsClip;
        footstepsSource.loop = true;
        canMove = true;
        comeBackFromStrongAttack = false;

        fogStateLastTime = float.MinValue;
        evilLevel = maxEvilLevel;
    }

    private void Start () 
    {
        UIManager.instance.SetEvilBarValue(evilLevel);

        footSteps.SetActive(false);

        timeSinceLastTrapUse = trapUseCooldown;
        timeSinceLastAttack = 1000.0f;
        timeSinceLastStrongAttack = 1000.0f;
        timeSinceLastTeleport = 0.0f;
        teleported = false;

        currentState.EnterState(this);

        // Quick fix. More info above ReEnable method.
        enabled = false;
        Invoke("ReEnable", 0);
    }

    private void Update()
    {
        if (GameManager.instance.gameIsPaused)
        {
            footstepsSource.Stop();
            return;
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
        
    }
    #endregion

    #region Public Methods
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

    public int GetMaxEvilLevel()
    {
        return maxEvilLevel;
    }

    public int GetEvilLevel()
    {
        return evilLevel;
    }

    public void AddEvilPoints(int value)
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

        UIManager.instance.SetEvilBarValue(evilLevel);
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
    }

    public void InstantiateAttack(ParticleSystem attackPrefab, Transform enemy, Vector3 hitOffset)
    {
        Vector3 spawningPos = bulletSpawnPoint.position;

        ParticleSystem attack = ParticlesManager.instance.LaunchParticleSystem(attackPrefab, spawningPos, transform.rotation);
        FollowTarget attackClone = attack.GetComponent<FollowTarget>();
        attackClone.SetEnemyTransform(enemy);
        attackClone.SetHitOffset(hitOffset);
    }
    #endregion

    #region Private Methods
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
    #endregion
}
