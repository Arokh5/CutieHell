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

    [Header("NavTargets")]
    [SerializeField]
    [Tooltip("One randomly selected Nav Target out of the 'Selection size' closest Nav Targets will be picked when GetNavTarget is called")]
    private int selectionSize = 3;
    [SerializeField]
    private List<Transform> navTargets;
    
    #endregion

    #region MonoBehaviour Methods
    // Use this for initialization
    protected void Awake()
    {
        buildingEffects = GetComponent<BuildingEffects>();
        if (!buildingEffects)
            Debug.LogWarning("WARNING: A BuildingEffects Component could not be found by Building in GameObject " + gameObject.name + ". No effects will be used.");

        currentHealth = baseHealth;
    }

    protected void Start()
    {
        // Intentionally left blank to allow child classes to call base.Start() and lately add code here.
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

    // IDamageable
    public float GetMaxHealth()
    {
        return baseHealth;
    }

    // IDamageable
    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetNormalizedHealth()
    {
        return currentHealth / baseHealth;
    }

    public bool HasFullHealth()
    {
        return currentHealth == baseHealth;
    }

    public Transform GetNavTarget(Transform reference)
    {
        int navTargetsCount = navTargets.Count;
        if (navTargetsCount > 0)
        {
            navTargets.Sort((a, b) =>
            {
                float distanceA = (reference.position - a.position).sqrMagnitude;
                float distanceB = (reference.position - b.position).sqrMagnitude;
                return distanceA > distanceB ? 1 : (distanceA < distanceB ? -1 : 0);
            });

            int max = Mathf.Min(selectionSize, navTargets.Count);
            int randomIndex = Random.Range(0, max);
            return navTargets[randomIndex];
        }
        else
        {
            return transform;
        }
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
            buildingEffects.SetBuildingConquerProgress((baseHealth - currentHealth) / (float)baseHealth);
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

    // IDamageable
    public bool IsTargetable()
    {
        return true;
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
