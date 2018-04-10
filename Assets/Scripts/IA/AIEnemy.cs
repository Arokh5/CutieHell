﻿using System.Collections;
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
    private Renderer mRenderer;

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

    protected float currentHealth;

    [Header("Color changing Testing")]
    public Color initialColor;
    public Color halfColor;
    public Color deadColor;
    public float healthToReduce = 1;
    public bool hit;
    private bool isTargetable = true;
    private bool isTarget = false;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        UnityEngine.Assertions.Assert.IsNotNull(agent, "Error: No NavMeshAgent found for AIEnemy in GameObject '" + gameObject.name + "'!");
        mRenderer = GetComponentInChildren<MeshRenderer>();
        UnityEngine.Assertions.Assert.IsNotNull(mRenderer, "Error: No MeshRenderer found for children of AIEnemy in GameObject '" + gameObject.name + "'!");
        mRenderer.material.color = initialColor;
        originalStoppingDistance = agent.stoppingDistance;
    }

    private void Start()
    {
        UnityEngine.Assertions.Assert.IsNotNull(zoneController, "Error: zoneController is null for AIEnemy in GameObject '" + gameObject.name + "'!");
        UpdateTarget();
        currentHealth = baseHealth;
    }

    private void Update()
    {
        // Motion through NavMeshAgent
        if (currentTarget && agent.enabled)
        {
            
            if (currentNode == null || currentTarget.GetType() != typeof(Monument))
            {
                agent.stoppingDistance = originalStoppingDistance;
                agent.SetDestination(currentTarget.transform.position);
            }
            else
            {
                agent.stoppingDistance = 0.0f;
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
        if (getHitVFX != null) Destroy(Instantiate(getHitVFX, this.transform.position + Vector3.up, this.transform.rotation), 0.9f);
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
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

    private void Die()
    {
        zoneController.RemoveEnemy(this);
        Player player = GameManager.instance.GetPlayer1();
        if (player != null)
        {
            player.SetEvilLevel(evilKillReward);
        }

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