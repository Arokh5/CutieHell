﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIEnemy : MonoBehaviour, IDamageable
{
    #region Fields
    private AIZoneController zoneController;
    private SubZoneType currentSubZone;
    private Building currentTarget;
    private NavMeshAgent agent;

    [Header("Attack information")]
    public float attackRange = 5;
    public float dps = 0.5f;

    [Tooltip("The initial amount of hit points for the conquerable building.")]
    public float baseHealth;

    protected float currentHealth;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        UnityEngine.Assertions.Assert.IsNotNull(agent, "Error: agent is null for AIEnemy in GameObject '" + gameObject.name + "'!");
    }

    private void Start()
    {
        UnityEngine.Assertions.Assert.IsNotNull(zoneController, "Error: zoneController is null for AIEnemy in GameObject '" + gameObject.name + "'!");
        UpdateTarget();
    }

    private void Update()
    {
        if (currentTarget)
        {
            agent.SetDestination(currentTarget.transform.position);

            if (Vector3.Distance(transform.position, currentTarget.transform.position) < attackRange)
            {
                Attack();
            }
        }
    }
    #endregion

    #region Public Methods
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

    // Called by the ZoneController in case the Monument gets repaired (this will cause all AIEnemy to return to the ZoneController's area)
    // or when a Trap gets deactivated or when the area-type Trap explodes
    public void SetCurrentTarget(Building target)
    {
        currentTarget = target;
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
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
        }
    }

    // Called by the Area-type Trap to retarget the AIEnemy after exploding
    public void UpdateTarget()
    {
        currentTarget = zoneController.GetTargetBuilding();
    }

    #endregion

    #region Private Methods
    private void Attack()
    {
        currentTarget.TakeDamage(dps * Time.deltaTime, AttackType.ENEMY);
    }
    #endregion
}
