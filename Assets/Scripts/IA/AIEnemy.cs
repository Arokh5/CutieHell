using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIEnemy : MonoBehaviour, IDamageable
{
    #region Fields
    [HideInInspector]
    public AISpawnController spawnController;
    private AIZoneController zoneController;
    private SubZoneType currentSubZone;

    private Building currentTarget;
    [SerializeField]
    private List<PathNode> currentPath;
    [SerializeField]
    private PathNode currentNode;
    private int currentNodeIndex = -1;

    private NavMeshAgent agent;
    private float originalStoppingDistance;

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
    private ParticleSystem getHitVFX;
    [SerializeField]
    private ParticleSystem deathVFX;
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
    private Collider enemyCollider;
    private AttackType killingHit = AttackType.NONE;

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
        enemyCollider = GetComponent<Collider>();
        originalStoppingDistance = agent.stoppingDistance;
    }

    private void Start()
    {
        heightOffset = enemyCollider.bounds.size.y / 2.0f;
        Restart();
    }

    private void Update()
    {
        // Motion through NavMeshAgent
        if (currentTarget && agent.enabled)
        {
            agent.stoppingDistance = 0.0f;
            /* First case is when going for the Monument, second case is when going for a Trap */
            if (currentNode == null || currentTarget.GetType() != typeof(Monument))
            {
                agent.stoppingDistance = originalStoppingDistance;
                agent.SetDestination(currentTarget.transform.position);
                attackLogic.AttemptAttack(currentTarget);
                if (enemyType == EnemyType.RANGE && attackLogic.IsInAttackRange(currentTarget))
                {
                    animator.SetBool("Move", false);
                }
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

            if (enemyType == EnemyType.RANGE && !attackLogic.IsInAttackRange(currentTarget))
            {
                animator.SetBool("Move", true);
            }
        }

        // Testing
        if (hit)
        {
            hit = false;
            TakeDamage(healthToReduce, AttackType.ENEMY);
        }

        if (isTarget)
        {
            GetComponent<EnemyCanvasController>().EnableHealthBar(false);
        }
    }
    #endregion

    #region Public Methods

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public void HitByZoneTrap()
    {
        agent.enabled = false;
        enemyCollider.enabled = false;
    }

    public void Restart()
    {
        UnityEngine.Assertions.Assert.IsNotNull(zoneController, "Error: zoneController is null for AIEnemy in GameObject '" + gameObject.name + "'!");
        UpdateTarget();
        currentHealth = baseHealth;
        mRenderer.material = basicMat;
        mRenderer.material.color = initialColor;
        enemyCollider.enabled = true;
        agent.enabled = true;
        isTargetable = true;
        isTarget = false;
        GetComponent<EnemyCanvasController>().SetHealthBar();
    }

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
        UpdateNodePath();
        UpdateTarget();
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
        if (getHitVFX != null)
            ParticlesManager.instance.LaunchParticleSystem(getHitVFX, this.transform.position + Vector3.up * heightOffset, this.transform.rotation);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            agent.enabled = false;
            SetIsTargetable(false);
            killingHit = attacktype;
            animator.SetBool("DieStandard", true);
            //Die();
        }
        else
        {
            animator.SetBool("GetHit", true);
        }
        GetComponent<EnemyCanvasController>().EnableHealthBar(true);
        GetComponent<EnemyCanvasController>().SetHealthBar();
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

    public bool GetIsTargetable()
    {
        return isTargetable;
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
        if (player != null && killingHit == AttackType.WEAK || killingHit == AttackType.STRONG)
        {
            player.AddEvilPoints(evilKillReward);
        }
        killingHit = AttackType.NONE;

        if(deathVFX != null)
            ParticlesManager.instance.LaunchParticleSystem(deathVFX, this.transform.position + Vector3.up * heightOffset, this.transform.rotation);

        DestroySelf();
    }

    public void DieAfterMatch()
    {
        if(deathVFX != null)
            ParticlesManager.instance.LaunchParticleSystem(deathVFX, this.transform.position + Vector3.up * heightOffset, this.transform.rotation);

        DestroySelf();
    }

    public void DestroySelf()
    {
        zoneController = null;
        animator.Rebind();
        spawnController.ReturnEnemy(this);
    }

    private void UpdateNodePath()
    {
        /* Update path and nextNode */
        currentPath = zoneController.GetPath(transform.position);
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