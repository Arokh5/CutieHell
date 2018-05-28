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

    private float timeSinceLastMonumentChecking = 0f;
    private float checkingMonumentRepetitionTime = 1f;

    private Monument[] allMonuments;
    [HideInInspector]
    public Monument monument;

    [Header("Actual Trap")]
    [HideInInspector]
    public Trap[] allTraps;
    public Trap nearbyTrap;
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
    public GameObject strongAttackObject;
    public ProjectorColorChange projector;
    public MeshCollider strongAttackMeshCollider;
    public ParticleSystem strongAttackExplosion;
    [HideInInspector]
    public float timeSinceLastStrongAttack;
    [HideInInspector]
    public List<AIEnemy> currentStrongAttackTargets = new List<AIEnemy>();

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
    [HideInInspector]
    public List<AIEnemy> currentFogAttackTargets = new List<AIEnemy>();

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

        fogStateLastTime = float.MinValue;
    }

    private void Start () 
    {
        footSteps.SetActive(false);
        evilLevel = maxEvilLevel;

        timeSinceLastTrapUse = trapUseCooldown;
        timeSinceLastAttack = 1000.0f;
        timeSinceLastStrongAttack = 1000.0f;
        timeSinceLastTeleport = 0.0f;
        teleported = false;

        currentState.EnterState(this);

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

    public void InstantiateStrongAttack(int evilCost)
    {
        AddEvilPoints(evilCost);
        ParticleSystem strongAttackReference = ParticlesManager.instance.LaunchParticleSystem(strongAttackExplosion, transform.position, transform.rotation);
        Transform particlesParent = strongAttackReference.transform.parent;
        strongAttackReference.transform.SetParent(this.transform);
        strongAttackReference.transform.localPosition = new Vector3(0.0f, 1.5f, 0.0f);
        strongAttackReference.transform.localRotation = Quaternion.Euler(new Vector3(-90, 180, 0));
        strongAttackReference.transform.SetParent(particlesParent);
    }
    #endregion

    #region Private Methods
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
