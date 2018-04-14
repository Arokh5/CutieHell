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
    [HideInInspector]
    public MeshRenderer meshRenderer;

    [Header("Attacks")]
    [SerializeField]
    public Transform bulletSpawnPoint;

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
    public PlayerStates state;

    [Header("Basic Attacks")]
    [HideInInspector]
    public float timeSinceLastAttack;
    [HideInInspector]
    public AIEnemy currentBasicAttackTarget = null;

    [Header("Strong Attack")]
    public MeshCollider strongAttackMeshCollider;
    public Renderer strongAttackRenderer;
    [HideInInspector]
    public float timeSinceLastStrongAttack;
    [HideInInspector]
    public List<AIEnemy> currentStrongAttackTargets = new List<AIEnemy>();
    #endregion

    public enum PlayerStates { STILL, MOVE, WOLF, FOG, TURRET}

    #region MonoBehaviour Methods
    private void Awake() 
    {
        state = PlayerStates.MOVE;
        initialBulletSpawnPointPos = new Vector3(0.8972f, 1.3626f, 0.1209f);
        meshRenderer = this.GetComponentInChildren<MeshRenderer>();
        rb = this.GetComponent<Rigidbody>();
        GameObject[] allTrapsGameObjects = GameObject.FindGameObjectsWithTag("Traps");
        allTraps = new Trap[allTrapsGameObjects.Length];
        for (int i = 0; i < allTrapsGameObjects.Length; ++i)
        {
            allTraps[i] = allTrapsGameObjects[i].GetComponent<Trap>();
        }
    }

    private void Start () 
    {
        footSteps.SetActive(false);
        evilLevel = maxEvilLevel;

        timeSinceLastTrapUse = trapUseCooldown;
        timeSinceLastAttack = 1000.0f;
        timeSinceLastStrongAttack = 1000.0f;

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
    #endregion
}