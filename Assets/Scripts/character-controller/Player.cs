using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour {

    #region Fields
    [Header("Movement Variabes")]
    public float maxSpeed = 10;
    public float acceleration = 50;
    public Transform centerTeleportPoint;
    public Transform statueTeleportPoint;
    public GameObject footSteps;

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

    [Header("Player States")]
    [SerializeField]
    private State currentState;
    public CameraState cameraState;
    public Camera mainCamera;

    [Header("Basic Attacks")]
    [HideInInspector]
    public float timeSinceLastAttack;
    [HideInInspector]
    public AIEnemy currentBasicAttackTarget = null;
    [HideInInspector]
    public bool animatingAttack = false;
    [HideInInspector]
    public Vector3 weakAttackTargetHitPoint;
    [HideInInspector]
    public Transform weakAttackTargetTransform;

    [Header("Strong Attack")]
    public GameObject strongAttackObject;
    public ProjectorColorChange projector;
    public MeshCollider strongAttackMeshCollider;
    public GameObject strongAttackExplosion;
    [HideInInspector]
    public float timeSinceLastStrongAttack;
    [HideInInspector]
    public List<AIEnemy> currentStrongAttackTargets = new List<AIEnemy>();

    [HideInInspector]
    public AudioSource footstepsSource;
    public AudioClip footstepsClip;
    #endregion

    public enum CameraState { STILL, MOVE, WOLF, FOG, TURRET, TRANSITION, ZOOMOUT, ZOOMIN}
    public enum TeleportStates { OUT, TRAVEL, IN}
    

    #region MonoBehaviour Methods
    private void Awake() 
    {
        cameraState = CameraState.MOVE;
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

        if (!monument)
            monument = GameObject.FindGameObjectWithTag("Monument").GetComponent<Monument>();

        footstepsSource = GetComponent<AudioSource>();
        footstepsSource.clip = footstepsClip;
        footstepsSource.loop = true;
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
            return;
        }

        timeSinceLastTrapUse += Time.deltaTime;
        timeSinceLastAttack += Time.deltaTime;
        timeSinceLastStrongAttack += Time.deltaTime;
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

    public void SetEvilLevel(int value)
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
        rb.isKinematic = !visible;
    }

    public void InstantiateAttack(FollowTarget attackPrefab, Transform enemy, Vector3 hitPoint)
    {
        Vector3 spawningPos = bulletSpawnPoint.position;

        FollowTarget attackClone = Instantiate(attackPrefab, spawningPos, transform.rotation);
        attackClone.SetEnemy(enemy);
        attackClone.SetHitPoint(hitPoint);
    }

    public void InstantiateStrongAttack(int evilCost)
    {
        SetEvilLevel(evilCost);
        GameObject strongAttackReference = Instantiate(strongAttackExplosion, transform.position, transform.rotation);
        strongAttackReference.transform.SetParent(this.transform);
        strongAttackReference.transform.localPosition = new Vector3(0.0f, 1.5f, 0.0f);
        strongAttackReference.transform.localRotation = Quaternion.Euler(new Vector3(-90, 180, 0));
        strongAttackReference.transform.SetParent(null);
    }
    #endregion
}

    
