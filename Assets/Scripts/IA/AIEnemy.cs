using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIEnemy : MonoBehaviour, IDamageable
{
    #region Fields
    private AIZoneController zoneController;
    private PathsController pathsController;
    private SubZoneType currentSubZone;

    private Building currentTarget;
    [SerializeField]
    private List<PathNode> currentPath;
    [SerializeField]
    private PathNode currentNode;
    private int currentNodeIndex = -1;

    private NavMeshAgent agent;
    private float originalStoppingDistance;

    private EnemyProjection currentVirtualTarget;

    private Renderer mRenderer;
    private Animator animator;

    [Header("Materials")]
    [SerializeField]
    private Material basicMat;
    [SerializeField]
    private Material outlinedMat;
    public float effectOnMapRadius = 1.0f;

    [Header("Attack information")]
    [SerializeField]
    private AIAttackLogic attackLogic;

    [Header("Health information")]
    [Tooltip("The initial amount of hit points for the conquerable building.")]
    public float baseHealth;
    public int evilKillReward;
    [SerializeField]
    private GameObject getHitVFX;
    [SerializeField]
    private GameObject deathVFX;
    [HideInInspector]
    public float heightOffset;

    protected float currentHealth;

    [Header("Color changing Testing")]
    public Color initialColor;
    public Color halfColor;
    public Color deadColor;
    public float healthToReduce = 1;
    public bool hit;
    private bool isTargetable = true;
    private bool isTarget = false;

    public EnemyType enemyType;

    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        UnityEngine.Assertions.Assert.IsNotNull(agent, "Error: No NavMeshAgent found for AIEnemy in GameObject '" + gameObject.name + "'!");
        mRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        if (mRenderer == null)
            mRenderer = GetComponentInChildren<MeshRenderer>();
        UnityEngine.Assertions.Assert.IsNotNull(mRenderer, "Error: No MeshRenderer found for children of AIEnemy in GameObject '" + gameObject.name + "'!");
        animator = this.GetComponent<Animator>();
        UnityEngine.Assertions.Assert.IsNotNull(animator, "Error: No Animator found in GameObject '" + gameObject.name + "'!");
        mRenderer.material.color = initialColor;
        originalStoppingDistance = agent.stoppingDistance;
    }

    private void Start()
    {
        UnityEngine.Assertions.Assert.IsNotNull(zoneController, "Error: zoneController is null for AIEnemy in GameObject '" + gameObject.name + "'!");
        UpdateTarget();
        currentHealth = baseHealth;
        heightOffset = this.GetComponent<Collider>().bounds.size.y / 2.0f;
    }

    private void Update()
    {
        // Motion through NavMeshAgent
        if (currentTarget && agent.enabled)
        {
            agent.stoppingDistance = 0.0f;
            if (this.currentVirtualTarget != null)
            {
                agent.SetDestination(currentVirtualTarget.transform.position);
            }
            else if (currentNode == null || currentTarget.GetType() != typeof(Monument))
            {
                    agent.stoppingDistance = originalStoppingDistance;
                    agent.SetDestination(currentTarget.transform.position);               
            }
            else
            {    
                agent.SetDestination(currentNode.transform.position);
                if ((transform.position - currentNode.transform.position).sqrMagnitude < currentNode.radius * currentNode.radius)
                {
                    if (currentNodeIndex < currentPath.Count - 1)
                    {
                        ++currentNodeIndex;
                        currentNode = currentPath[currentNodeIndex];
                    }
                    else
                    {
                        currentNodeIndex = -1;
                        currentNode = null;
                    }
                }
            }

            attackLogic.AttemptAttack(currentTarget);
        }

        // Testing
        if (hit)
        {
            hit = false;
            TakeDamage(healthToReduce, AttackType.ENEMY);
        }
    }
    #endregion

    #region Public Methods
    public AIZoneController GetZoneController()
    {
        return zoneController;
    }

    // Called by AISpawner when instantiating an AIEnemy. This method should inform the ZoneController about this AIEnemy's creation
    public void SetZoneController(AIZoneController newZoneController)
    {
        if (!newZoneController)
        {
            return;
        }

        if (zoneController)
        {
            zoneController.RemoveEnemy(this);
        }
        newZoneController.AddEnemy(this);
        zoneController = newZoneController;
    }

    // Called by AISpawner when instantiating an AIEnemy.
    public void SetPathsController(PathsController newPathsController)
    {
        pathsController = newPathsController;

        UpdateNodePath();
    }

    // Called by the ZoneController in case the Monument gets repaired (this will cause all AIEnemy to return to the ZoneController's area)
    // or when a Trap gets deactivated or when the area-type Trap explodes
    public void SetCurrentTarget(Building target)
    {
        if (currentTarget != target)
        {
            currentTarget = target;
        }
    }

    // Called by the ZoneController in case the AIEnemy is affected by the attracion power of the EnemyProjection
    public void SetCurrentVirtualTarget(EnemyProjection virtualTarget)
    {
        if (virtualTarget != null) //AIEnemies will ignore stopping distance if they are chasing projections
        {
            currentVirtualTarget = virtualTarget;
        }     
    }

    // Called by the ZoneController to know if an specific enemy is currently attracted by a certain enemyprojection
    public EnemyProjection GetCurrentVirtualTarget()
    {
        return currentVirtualTarget;
    }

    // Called by the ZoneController to know if an specific enemy is currently attracted by a seductive projection
    public bool isCurrentlyAttractedByAProjection()
    {
        bool currentlyAttracted = false;
        if(currentVirtualTarget != null)
        {
            currentlyAttracted = true;
        }
        return currentlyAttracted;
    }

    // IDamageable
    // Called by the AIPlayer or an Attack to determine if this AIEnemy should be targetted
    public bool IsDead()
    {
        return currentHealth <= 0;
    }

    // Called by the AIPlayer or an Attack to damage the AIEnemy
    public void TakeDamage(float damage, AttackType attacktype)
    {
        if (IsDead() || !isTargetable)
            return;

        currentHealth -= damage;
        if (getHitVFX != null) Destroy(Instantiate(getHitVFX, this.transform.position + Vector3.up * heightOffset, this.transform.rotation), 0.9f);
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            agent.enabled = false;
            animator.SetBool("DieStandard", true);
            //Die();
        }
        else
        {
            animator.SetBool("GetHit", true);
        }
        AdjustMaterials();
    }

    // Called by the Area-type Trap to retarget the AIEnemy after exploding
    public void UpdateTarget()
    {
        currentTarget = zoneController.GetTargetBuilding(transform);
    }

    public bool MarkAsTarget(bool isTarget)
    {
        if (isTargetable && this.isTarget != isTarget)
        {
            this.isTarget = isTarget;
            mRenderer.material = isTarget ? outlinedMat : basicMat;
            AdjustMaterials();
            return true;
        }

        return false;
    }

    public void SetIsTargetable(bool isTargetable)
    {
        if (this.isTargetable != isTargetable)
        {
            this.isTargetable = isTargetable;
            if (!isTargetable && isTarget)
            {
                isTarget = false;
                mRenderer.material = basicMat;
                AdjustMaterials();
            }
        }
    }

    #endregion

    #region Private Methods
    private void AdjustMaterials()
    {
        Color finalColor;
        float normalizedHealth = currentHealth / baseHealth;

        if (normalizedHealth < 0.5f)
        {
            normalizedHealth *= 2;
            finalColor = deadColor * (1 - normalizedHealth) + halfColor * normalizedHealth;
        }
        else
        {
            normalizedHealth = (normalizedHealth - 0.5f) * 2;
            finalColor = halfColor * (1 - normalizedHealth) + initialColor * normalizedHealth;
        }
        mRenderer.material.color = finalColor;
    }

    // Called on Animator
    public void Die()
    {
        StatsManager.instance.RegisterKill(enemyType);
        zoneController.RemoveEnemy(this);
        Player player = GameManager.instance.GetPlayer1();
        if (player != null)
        {
            player.SetEvilLevel(evilKillReward);
        }
        if(deathVFX != null) Destroy(Instantiate(deathVFX, this.transform.position + Vector3.up * heightOffset, this.transform.rotation),0.9f);

        if (currentVirtualTarget)
            currentVirtualTarget.RemoveEnemyAttracted(this);

        Destroy(gameObject);
    }

    private void UpdateNodePath()
    {
        /* Update path and nextNode */
        currentPath = pathsController.GetPath(transform.position);
        if (currentPath != null && currentPath.Count > 0)
        {
            currentNodeIndex = 0;
            currentNode = currentPath[0];
        }
        else
        {
            currentNodeIndex = -1;
            currentNode = null;
        }
    }
    #endregion
}