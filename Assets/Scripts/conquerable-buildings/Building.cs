using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building : MonoBehaviour, IDamageable
{

    #region Fields
    [Header ("General Building fields")]
    [SerializeField]
    protected AIZoneController zoneController;
    [Tooltip("The initial amount of hit points for the conquerable building.")]
    public float baseHealth;

    [SerializeField]
    [ShowOnly]
    protected float currentHealth;
    
    [ShowOnly]
    public AIEnemy attachedConqueror;

    private BuildingEffects buildingEffects;
    [HideInInspector]
    public bool animating = false;

    [Header("Damage testing")]
    public bool immortal = false;
    public bool takeDamage = false; // TEST
    public float lifeLossPerSecond = 0; // TEST

    #endregion

    #region MonoBehaviour Methods
    // Use this for initialization
    private void Awake()
    {
        buildingEffects = GetComponent<BuildingEffects>();
        if (!buildingEffects)
            Debug.LogWarning("WARNING: A BuildingEffects Component could not be found by Building in GameObject " + gameObject.name + ". No effects will be used.");

        currentHealth = baseHealth;
    }

    // Update is called once per frame
    protected void Update()
    {
        Test();
    }
    #endregion

    #region Public Methods
    public abstract void BuildingConverted();
    public abstract void BuildingKilled();

    public float GetMaxHealth()
    {
        return baseHealth;
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public bool HasFullHealth()
    {
        return currentHealth == baseHealth;
    }

    // IDamageable
    public bool IsDead()
    {
        return currentHealth == 0;
    }

    // IDamageable
    public virtual void TakeDamage(float damage, AttackType attacktype)
    {
        if (immortal || currentHealth == 0)
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
            BuildingKilled();
            if (buildingEffects)
                buildingEffects.StartConquerEffect();
            else
                BuildingConverted();
        }
    }
    #endregion

    #region Private Methods
    private void Test()
    {
        if (takeDamage)
        {
            TakeDamage(lifeLossPerSecond * Time.deltaTime, AttackType.ENEMY);
            if (currentHealth == 0)
                takeDamage = false;
        }
    }
    #endregion
}
