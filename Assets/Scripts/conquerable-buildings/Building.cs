using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building : MonoBehaviour, IDamageable, IRepairable
{

    #region Fields
    [Header ("General Building fields")]
    public SubZoneType zoneType;
    [SerializeField]
    protected AIZoneController zoneController;
    [Tooltip("The initial amount of hit points for the conquerable building.")]
    public float baseHealth;
    public int fullRepairCost;
    [Tooltip("The radius within which enemies are attracted to this building if it is set as target. Negative numbers stand for infinite radius")]
    public float attractionRadius = -1;

    [SerializeField]
    [ShowOnly]
    protected float currentHealth;
    
    [ShowOnly]
    public AIEnemy attachedConqueror;

    private BuildingEffects buildingEffects;
   
    [Header("Damage testing")]
    public bool takeDamage = false; // TEST
    public float lifeLossPerSecond = 0; // TEST
    public bool fullRepair = false;

    #endregion

    #region MonoBehaviour Methods
    // Use this for initialization
    private void Awake()
    {
        buildingEffects = GetComponent<BuildingEffects>();
        if (!buildingEffects)
            Debug.LogWarning("WARNING: A BuildingEffects Component could not be found by Building in GameObject " + gameObject.name + ". No effects will be used.");
    }

    private void Start()
    {
        currentHealth = baseHealth;
    }

    // Update is called once per frame
    protected void Update()
    {
        Test();
    }
    #endregion

    #region Public Methods
    public float GetMaxHealth()
    {
        return baseHealth;
    }

    // IDamageable
    public bool IsDead()
    {
        return currentHealth == 0;
    }

    // IDamageable
    public virtual void TakeDamage(float damage, AttackType attacktype)
    {
        if (currentHealth == 0)
            return;

        currentHealth -= damage;

        if (currentHealth <= 0)
            currentHealth = 0;

        // Reset the underAttackElapsedTime timer
        if (buildingEffects)
        {
            buildingEffects.SetUnderAttack(true);
            buildingEffects.AdjustMaterials((baseHealth - currentHealth) / (float)baseHealth);
        }

        if (currentHealth == 0)
        {
            Conquer();
            if (buildingEffects)
                buildingEffects.StartConquerEffect();
        }
    }

    // IRepairable
    public virtual void FullRepair()
    {
        Debug.LogWarning("REMINDER: To prevent: Repairing a building while it's losing health due to a conqueror " +
            "will cause the conqueror to finish its attack without fully conquering the trap!");

        if (currentHealth == 0 && attachedConqueror)
        {
            zoneController.RemoveEnemy(attachedConqueror);
            Destroy(attachedConqueror.gameObject);
            attachedConqueror = null;
        }

        if (currentHealth == 0)
        {
            if (buildingEffects)
                buildingEffects.StartUnconquerEffect();
            else
                Unconquer();
        }

        currentHealth = baseHealth;

        if (buildingEffects)
            buildingEffects.AdjustMaterials(0);

    }

    // IRepairable
    public bool HasFullHealth()
    {
        return currentHealth == baseHealth;
    }

    // IRepairable
    public int GetRepairCost()
    {
        float normalizedDamageTaken = ( (baseHealth - currentHealth ) / baseHealth );

        if (normalizedDamageTaken < 0.25f)
        {
            return fullRepairCost * 1/4;
        }
        else if (normalizedDamageTaken < 0.50f)
        {
            return fullRepairCost * 2/4;
        }
        else if (normalizedDamageTaken < 0.75f)
        {
            return fullRepairCost * 3/4;
        }
        else
        {
            return fullRepairCost;
        }
        
    }

    /* Called by the BuildingEffects script if available */
    public void Conquer()
    {
        BuildingKilled();
    }

    /* Called by the BuildingEffects script if available */
    public void Unconquer()
    {
        BuildingRecovered();
    }
    #endregion

    #region Protected Methods
    protected abstract void BuildingKilled();
    protected abstract void BuildingRecovered();
    #endregion

    #region Private Methods
    private void Test()
    {
        if (fullRepair)
        {
            fullRepair = false;
            FullRepair();
        }

        if (takeDamage)
        {
            TakeDamage(lifeLossPerSecond * Time.deltaTime, AttackType.ENEMY);
            if (currentHealth == 0)
                takeDamage = false;
        }
    }
    #endregion
}
