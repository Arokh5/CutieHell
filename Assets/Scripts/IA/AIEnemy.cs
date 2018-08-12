﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIEnemy : MonoBehaviour, IDamageable
{
    #region Fields
    public EnemyType enemyType;

    [HideInInspector]
    public AISpawnController spawnController;
    [ShowOnly]
    [SerializeField]
    private AIZoneController zoneController;

    private Player playerTarget;
    private IDamageable currentTarget;
    private Building currentTargetBuilding;

    [Header("Motion")]
    public bool ignorePath = false;
    public bool useSoftenedNavigation = true;
    [SerializeField]
    private List<PathNode> currentPath;
    [SerializeField]
    private PathNode currentNode;
    private int currentNodeIndex = -1;
    public float highSpeedMultiplier = 2.0f;
    [ShowOnly]
    public bool inHighSpeedZone;

    private NavMeshAgent agent;
    [HideInInspector]
    public float initialSpeed;
    public float speedOnSlow;
    private float originalStoppingDistance;

    [SerializeField]
    [ShowOnly]
    private Vector3 navMotionTarget = Vector3.positiveInfinity;
    [SerializeField]
    [ShowOnly]
    private bool inNavNode = false;
    [SerializeField]
    [ShowOnly]
    private Transform navAttackTarget = null;
    private const float softenedNavigationOvershootDistance = 0.5f;


    private Renderer mRenderer;
    private Animator animator;

    [Header("Player Detection")]
    [SerializeField]
    private bool canAttackPlayer = false;
    [SerializeField]
    [Tooltip("The radius within which the player gets detected and becomes the target of the enemy")]
    private float detectionRadius = 4.0f;
    [SerializeField]
    [Tooltip("The radius outside of which a targeted player gets ignored by the enemy")]
    private float escapeRadius = 8.0f;
    [SerializeField]
    [Tooltip("The distance at which the enemy stops approaching the player")]
    private float minDistance = 1.0f;
    private Player player;
    private bool hasPlayerAsTarget;

    [Header("Outline")]
    [SerializeField]
    private float outlineThickness = 0.05f;

    [Header("Attack information")]
    [SerializeField]
    private AIAttackLogic attackLogic;
    [HideInInspector]
    public Vector3 lastAttackRecivedDirection;
    [HideInInspector]
    public float timeSinceLastAttackRecived;
    private float knockbackForceMultiplier;
    [SerializeField]
    private float knockbackForce;
    private float knockbackCurrentForce;

    [Header("Health information")]
    [Tooltip("The initial amount of hit points for the conquerable building.")]
    public float baseHealth;
    [SerializeField]
    private ParticleSystem getHitVFX;
    [SerializeField]
    private ParticleSystem deathVFX;
    [SerializeField]
    private AudioClip deathSFX;
    [HideInInspector]
    public float heightOffset;
    [HideInInspector]
    public float timeOnSlow;
    [HideInInspector]
    public float timeOnStun;
    public GameObject stunVFX;

    protected float currentHealth;
    private EnemyCanvasController canvasController;

    [Header("Damage Testing")]
    public float healthToReduce = 100;
    public bool hit;
    private bool isTargetable = true;
    private bool isTarget = false;

    private Collider enemyCollider;
    private AttackType killingHit = AttackType.NONE;

    private bool active;
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
        active = false;
        initialSpeed = agent.speed;
        timeSinceLastAttackRecived = 0.0f;
        heightOffset = enemyCollider.bounds.size.y / 2.0f;
        player = GameManager.instance.GetPlayer1();
        timeOnStun = 0.0f;
        timeOnSlow = 0.0f;
        stunVFX.SetActive(false);
        canvasController = GetComponent<EnemyCanvasController>();
    }

    private void Update()
    {
        UpdateCurrentTarget();
        Knockback();
        UpdateSlowSpeed();
        // Motion through NavMeshAgent
        bool bearShouldMove = false;
        if (currentTarget != null && agent.enabled)
        {
            if (!IsStunned())
            {
                /* First case is when going for the Monument, second is for a player target */
                if (currentNode == null || hasPlayerAsTarget)
                {
                    agent.autoBraking = true;
                    if (hasPlayerAsTarget)
                    {
                        // Player target
                        agent.stoppingDistance = minDistance;
                        agent.SetDestination(currentTarget.transform.position);
                    }
                    else
                    {
                        // Monument target
                        agent.stoppingDistance = originalStoppingDistance;
                        agent.SetDestination(navAttackTarget.position);
                    }
                    attackLogic.AttemptAttack(currentTarget, agent.destination);

                    if (enemyType == EnemyType.RANGE)
                    {
                        if (!attackLogic.IsInAttackRange(agent.destination))
                            bearShouldMove = true;
                    }
                }
                else
                {
                    // PathNode target
                    agent.autoBraking = false;
                    agent.stoppingDistance = 0.0f;
                    agent.SetDestination(navMotionTarget);
                    AdvanceInNodePath();
                    if (enemyType == EnemyType.RANGE)
                        bearShouldMove = true;

                }
            }
        }

        if (enemyType == EnemyType.RANGE)
        {
            agent.isStopped = !bearShouldMove;
            animator.SetBool("Move", bearShouldMove);
        }

        if (isTarget)
        {
            canvasController.EnableHealthBar(false);
        }

        // Testing
        if (hit)
        {
            hit = false;
            TakeDamage(healthToReduce, AttackType.ENEMY);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (!canAttackPlayer)
            return;

        Gizmos.color = new Color(0, 1, 0, 1);
        Gizmos.DrawWireSphere(transform.position, escapeRadius);
        Gizmos.color = new Color(1, 0, 0, 1);
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        if (hasPlayerAsTarget)
        {
            Vector3 offset = Vector3.up;
            Gizmos.color = new Color(1, 0, 0, 1);
            GizmosHelper.DrawArrow(transform.position + offset, player.transform.position + offset);
        }
    }
    #endregion

    #region Public Methods
    public void SetAgentEnable(bool enabled)
    {
        agent.enabled = enabled;
    }

    public void ResetSpeed()
    {
        SetSpeed(initialSpeed);
    }

    public void SetSpeed(float newSpeed)
    {
        if (newSpeed <= 0)
            agent.speed = 0;
        else
            agent.speed = newSpeed * (inHighSpeedZone ? highSpeedMultiplier : 1);
    }

    public void SetInHighSpeedZone(bool inHighSpeedZone)
    {
        this.inHighSpeedZone = inHighSpeedZone;
        SetSpeed(agent.speed);
    }

    public void SetKnockback(Vector3 originForce, float forceMultiplier = 1.0f)
    {
        knockbackForceMultiplier = forceMultiplier;
        knockbackCurrentForce = knockbackForce;
        timeSinceLastAttackRecived = Time.time;
        lastAttackRecivedDirection = (this.transform.position - originForce).normalized;
        lastAttackRecivedDirection.y = 0;
    }

    public void SetStun(float timeToStun)
    {
        timeOnStun = timeToStun;
    }

    public void SetSlow(float timeToSloDown)
    {
        timeOnSlow = timeToSloDown;
    }

    public float GetMaxHealth()
    {
        return baseHealth;
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public void Restart()
    {
        navAttackTarget = null;
        timeOnStun = 0.0f;
        timeOnSlow = 0.0f;
        currentHealth = baseHealth;
        enemyCollider.enabled = true;
        SetAgentEnable(true);
        isTargetable = true;
        isTarget = false;
        canvasController.SetHealthBar();
        active = true;
        AdjustMaterials();
    }

    public AIZoneController GetZoneController()
    {
        return zoneController;
    }

    // Called by AISpawner when instantiating an AIEnemy. This method should inform the ZoneController about this AIEnemy's creation
    public void SetZoneController(AIZoneController newZoneController)
    {
        if (!newZoneController || newZoneController == zoneController)
        {
            return;
        }

        /*
         * Enemy MUST be added to the new ZoneController before removing it from the original one to avoid 
         * potentially reporting all zones as empty when only 1 enemy is left!
         */
        newZoneController.AddEnemy(this);
        if (zoneController)
        {
            zoneController.RemoveEnemy(this);
        }
        zoneController = newZoneController;
        navAttackTarget = null;

        if (!ignorePath)
            UpdateNodePath();
        UpdateTarget();
    }

    // Called by the ZoneController when a Trap gets deactivated
    public void SetCurrentTarget(Building target)
    {
        if (currentTargetBuilding != target)
        {
            navAttackTarget = null;
            currentTargetBuilding = target;
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
        if (IsDead() || !isTargetable || !gameObject.activeSelf)
            return;

        currentHealth -= damage;
        if (getHitVFX != null)
            ParticlesManager.instance.LaunchParticleSystem(getHitVFX, this.transform.position + Vector3.up * heightOffset, this.transform.rotation);

        Achievements.instance.IncreaseCurrentCountHitType(1, attacktype);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            SetAgentEnable(false);
            SetIsTargetable(false);
            killingHit = attacktype;
            Achievements.instance.IncreaseCurrentCountKillingType(1, killingHit);
            animator.SetBool("DieStandard", true);
            SoundManager.instance.PlaySfxClip(deathSFX);
            //Die();
        }
        else
        {
            animator.SetTrigger("GetHit");
        }
        canvasController.EnableHealthBar(true);
        canvasController.SetHealthBar();
        AdjustMaterials();
    }

    // Called by the Area-type Trap to retarget the AIEnemy after exploding
    public void UpdateTarget()
    {
        UnityEngine.Assertions.Assert.IsNotNull(zoneController, "Error: zoneController is null for AIEnemy in GameObject '" + gameObject.name + "'!");
        currentTargetBuilding = zoneController.GetTargetBuilding();
    }

    public bool MarkAsTarget(bool isTarget)
    {
        if (isTargetable && this.isTarget != isTarget)
        {
            this.isTarget = isTarget;
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
                AdjustMaterials();
            }
        }
    }

    // Called on Animator
    public void Die()
    {
        StatsManager.instance.GetMaxCombo().EnableCombo();
        StatsManager.instance.RegisterKill(enemyType);
        zoneController.RemoveEnemy(this);
        killingHit = AttackType.NONE;

        if (deathVFX != null)
            ParticlesManager.instance.LaunchParticleSystem(deathVFX, this.transform.position + Vector3.up * heightOffset, this.transform.rotation);

        DestroySelf();
    }

    public void DieAfterMatch()
    {
        if (deathVFX != null)
            ParticlesManager.instance.LaunchParticleSystem(deathVFX, this.transform.position + Vector3.up * heightOffset, this.transform.rotation);

        DestroySelf();
    }

    public void DestroySelf()
    {
        zoneController = null;
        animator.Rebind();
        spawnController.ReturnEnemy(this);
        UIManager.instance.roundInfoController.AddToEnemiesCount(-1);
    }
    #endregion

    #region Private Methods
    private void AdjustMaterials()
    {
        if (isTarget)
            mRenderer.material.SetFloat("_Outline", outlineThickness);
        else
            mRenderer.material.SetFloat("_Outline", 0.0f);
    }

    private void UpdateSlowSpeed()
    {
        if (timeOnSlow > 0.0f)
        {
            timeOnSlow -= Time.deltaTime;
            SetSpeed(speedOnSlow);
        }
        else
        {
            ResetSpeed();
        }
        if(timeOnStun > 0.0f)
        {
            timeOnStun -= Time.deltaTime;
            SetSpeed(0.0f);
            animator.SetBool("Stunned", true);
        }
        else
        {
            animator.SetBool("Stunned", false);
        }
    }

    private bool IsStunned()
    {
        if (timeOnStun > 0.0f)
        {
            return true;
        }
        return false;
    }

    private void Knockback()
    {
        if (knockbackCurrentForce > 0.1f)
        {
            knockbackCurrentForce = Mathf.Lerp(knockbackCurrentForce, 0.0f, 0.2f);
            this.transform.Translate(lastAttackRecivedDirection * knockbackCurrentForce * knockbackForceMultiplier * Time.deltaTime,Space.World);
        }
    }

    private void UpdateNodePath()
    {
        UnityEngine.Assertions.Assert.IsNotNull(zoneController, "Error: zoneController is null for AIEnemy in GameObject '" + gameObject.name + "'!");
        /* Update path and nextNode */
        currentPath = zoneController.GetPath(transform.position);
        if (currentPath != null && currentPath.Count > 0)
        {
            currentNodeIndex = 0;
            currentNode = currentPath[0];
            navMotionTarget = currentNode.transform.position;
        }
        else
        {
            currentNodeIndex = -1;
            currentNode = null;
        }
    }

    private void UpdateCurrentTarget()
    {
        if (currentTarget == null)
            currentTarget = currentTargetBuilding;

        if (canAttackPlayer)
        {
            Vector3 enemyToPlayer = player.transform.position - transform.position;
            if (hasPlayerAsTarget)
            {
                // So we check if the player has exited the escapeRadius
                if (!player.isTargetable || player.IsDead() || enemyToPlayer.sqrMagnitude > escapeRadius * escapeRadius)
                {
                    hasPlayerAsTarget = false;
                    currentTarget = currentTargetBuilding;
                }
            }
            else
            {
                // So we check if the player has entered the detectionRadius
                if (player.isTargetable && !player.IsDead() && enemyToPlayer.sqrMagnitude < detectionRadius * detectionRadius)
                {
                    hasPlayerAsTarget = true;
                    currentTarget = player;
                    navAttackTarget = null;
                }
                else
                {
                    currentTarget = currentTargetBuilding;
                }
            }
        }
        else
        {
            currentTarget = currentTargetBuilding;
        }

        if (navAttackTarget == null && currentNode == null && currentTarget == currentTargetBuilding)
        {
            navAttackTarget = currentTargetBuilding.GetNavTarget(transform);
        }
    }

    private void AdvanceInNodePath()
    {
        if (!inNavNode)
        {
            // Normal case where the AIEnemy is going towards the PathNode
            if ((transform.position - currentNode.transform.position).sqrMagnitude < currentNode.radius * currentNode.radius)
            {
                // Arrived at PathNode
                if (currentNodeIndex < currentPath.Count - 1)
                {
                    // There's a further PathNode

                    if (useSoftenedNavigation)
                    {
                        /*  We soften the rotation by assigning an extra NavDestination at the intersection
                         *  between the line from the currentNode to the nextNode and the currentNode's radius
                         */
                        inNavNode = true;
                        PathNode nextNode = currentPath[currentNodeIndex + 1];

                        Vector3 currentToNext = nextNode.transform.position - currentNode.transform.position;
                        currentToNext.Normalize();
                        // To ensure the targetPos falls outside of the PathNode, we overshoot the targetPos by a small amount
                        currentToNext *= currentNode.radius + softenedNavigationOvershootDistance;
                        Vector3 targetPos = currentNode.transform.position + currentToNext;
                        navMotionTarget = targetPos;
                    }
                    else
                    {
                        ++currentNodeIndex;
                        currentNode = currentPath[currentNodeIndex];
                        navMotionTarget = currentNode.transform.position;
                    }

                }
                else
                {
                    // This was the last PathNode
                    currentNodeIndex = -1;
                    currentNode = null;
                }
            }
        }
        else
        {
            // Moving through the softened path. This is never reached if useSoftenedNavigation == false
            if ((transform.position - currentNode.transform.position).sqrMagnitude > currentNode.radius * currentNode.radius)
            {
                // So we exited the PathNode
                inNavNode = false;
                ++currentNodeIndex;
                currentNode = currentPath[currentNodeIndex];
                navMotionTarget = currentNode.transform.position;
            }
        }
        
    }
    #endregion
}