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

    [Header("Elements setup")]
    [SerializeField]
    private MeshRenderer buildingRenderer;
    [SerializeField]
    private MeshRenderer alternateBuildingRenderer;

    [Header("Area of Effect")]
    public float effectOnMapRadius = 0.0f;
    [SerializeField]
    private float maxEffectRadius = 5.0f;
    [SerializeField]
    private List<Convertible> convertibles;

    [Header("Life and stuff")]
    [Tooltip("The duration (in seconds) for which the conquerable object is considered to be \"under attack\" after the last actual attack happened.")]
    [SerializeField]
    private float underAttackStateDuration = 1;
    [Tooltip("The duration (in seconds) that the dark to cute conversion takes.")]
    [SerializeField]
    private float conquerEffectDuration = 1;
    [ShowOnly]
    public GameObject attachedConqueror;

    private CompassIconOwner compassIconOwner;
    private float underAttackElapsedTime = 0;
    private float conquerEffectElapsedTime = 0;
    private bool underAttack = false;
    private bool conquering = false;
    private bool conquered = false;

    [Header("Model shaking")]
    [Range(0, 1)]
    [SerializeField]
    private float shakeAmplitude;
    [SerializeField]
    private float shakeSpeed;

    [Header("Damage testing")]
    public bool reset = false; // TEST
    public bool loseHitPoints = false; // TEST
    public float lifeLossPerSecond = 0; // TEST
    public bool restoreLife = false;

    #endregion

    #region MonoBehaviour Methods
    // Use this for initialization
    private void OnApplicationQuit()
    {
        Reset();
    }

    private void Reset()
    {
        currentHealth = baseHealth;
        buildingRenderer.material.SetFloat("_ConquerFactor", 0);
        buildingRenderer.material.SetFloat("_ShakeAmplitude", shakeAmplitude);
        buildingRenderer.material.SetFloat("_ShakeSpeed", shakeSpeed);
        alternateBuildingRenderer.material.SetFloat("_SizeFactor", 0);
        effectOnMapRadius = 0.0f;
        conquered = false;
    }

    private void Awake()
    {
        UnityEngine.Assertions.Assert.IsNotNull(buildingRenderer, "ERROR: Building Renderer not assigned for ConquerableBuilding script in GameObject " + gameObject.name);
        UnityEngine.Assertions.Assert.IsNotNull(alternateBuildingRenderer, "ERROR: Alternate Building Renderer not assigned for ConquerableBuilding script in GameObject " + gameObject.name);
        buildingRenderer.gameObject.SetActive(true);
        alternateBuildingRenderer.gameObject.SetActive(false);
        effectOnMapRadius = 0.0f;

        if (!compassIconOwner)
            compassIconOwner = GetComponent<CompassIconOwner>();

        UnityEngine.Assertions.Assert.IsNotNull(compassIconOwner, "ERROR: Building could not find a CompassIconOwner in gameObject '" + gameObject.name + "'!");
    }

    private void Start()
    {
        Reset();
    }

    // Update is called once per frame
    protected void Update()
    {
        Test();

        if (underAttack)
        {
            underAttackElapsedTime += Time.deltaTime;
            if (underAttackElapsedTime >= underAttackStateDuration)
            {
                SetUnderAttack(false);
                underAttackElapsedTime = 0;
            }
        }

        if (conquering)
        {
            conquerEffectElapsedTime += Time.deltaTime;
            if (conquerEffectElapsedTime < conquerEffectDuration)
            {
                ConquerEffect();
            }
            else
            {
                Conquer();
            }
        }
    }
    #endregion

    #region Public Methods
    public float GetBlendRadius()
    {
        if (conquered)
            return maxEffectRadius / attractionRadius;
        else if (conquering)
            return (conquerEffectElapsedTime / conquerEffectDuration) * (maxEffectRadius / attractionRadius);
        else
            return 0.0f;
    }

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
        if (conquering || conquered)
            return;

        // Reset the underAttackElapsedTime timer
        if (compassIconOwner)
            UIManager.instance.compass.SetAlertForIcon(compassIconOwner);
        SetUnderAttack(true);
        underAttackElapsedTime = 0;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
        }
        InformUIManager();

        AdjustMaterials();

        if (currentHealth == 0)
        {
            ConquerEffect();
            BuildingKilled();
        }
    }

    // IRepairable
    public virtual void FullRepair()
    {
        Debug.LogWarning("REMINDER: To prevent: Repairing a building while it's losing health due to a conqueror " +
            "will cause the conqueror to finish its attack without fully conquering the trap!");

        if (currentHealth == 0 && attachedConqueror)
        {
            Destroy(attachedConqueror);
        }

        currentHealth = baseHealth;       

        AdjustMaterials();

        if (conquered)
        {
            Unconquer();
        }
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
    #endregion

    #region Protected Methods
    protected abstract void BuildingKilled();
    protected abstract void InformUIManager();
    #endregion

    #region Private Methods
    private void Test()
    {
        if (reset)
        {
            reset = false;
            loseHitPoints = false;
            FullRepair();
        }

        if (restoreLife)
        {
            restoreLife = false;
            FullRepair();
        }

        if (loseHitPoints)
        {
            TakeDamage(lifeLossPerSecond * Time.deltaTime, AttackType.ENEMY);
            if (conquered)
                loseHitPoints = false;
        }
    }

    private void SetUnderAttack(bool underAttackState)
    {
        if (underAttack != underAttackState)
        {
            if (underAttackState)
            {
                buildingRenderer.material.SetFloat("_UnderAttack", 1);
            }
            else
            {
                buildingRenderer.material.SetFloat("_UnderAttack", 0);
            }
        }
        underAttack = underAttackState;
    }

    private void ConquerEffect()
    {
        if (!conquering)
        {
            conquering = true;
            conquerEffectElapsedTime = 0;
            buildingRenderer.material.SetFloat("_ShakeAmplitude", 0);
            alternateBuildingRenderer.material.SetFloat("_SizeFactor", 0);
            alternateBuildingRenderer.gameObject.SetActive(true);
        }

        float progress = conquerEffectElapsedTime / conquerEffectDuration;
        effectOnMapRadius = maxEffectRadius + progress * (attractionRadius - maxEffectRadius);

        if (progress < 0.5f)
        {
            buildingRenderer.material.SetFloat("_ShakeAmplitude", progress * 2);
        }
        else if (progress < 0.6f)
        {
            alternateBuildingRenderer.material.SetFloat("_SizeFactor", (progress - 0.5f) * 4);
        }
        else
        {
            // Rescale te progress to fall in the range [0,1]
            progress = (progress - 0.6f) / (1 - 0.6f);
            alternateBuildingRenderer.material.SetFloat("_SizeFactor", 1 + (0.2f / (1 + progress)) * Mathf.Sin(2 * Mathf.PI * 2 * progress));
        }

        // Now we attempt to convert convertible props

        float limitRadius = maxEffectRadius * progress;
        foreach (Convertible convertible in convertibles)
        {
            if (!convertible.IsConverting() && Vector3.Distance(transform.position, convertible.transform.position) < limitRadius)
            {
                convertible.Convert();
            }
        }
    }

    private void Conquer()
    {
        conquering = false;
        conquerEffectElapsedTime = 0;
        alternateBuildingRenderer.material.SetFloat("_SizeFactor", 1);
        conquered = true;
    }

    private void Unconquer()
    {
        foreach (Convertible convertible in convertibles)
        {
            convertible.Unconvert();
        }

        Reset();
    }

    private void AdjustMaterials()
    {
        float conqueredFactor = (baseHealth - currentHealth) / (float)baseHealth;
        buildingRenderer.material.SetFloat("_ConquerFactor", conqueredFactor);
        effectOnMapRadius = conqueredFactor * maxEffectRadius;
    }
    #endregion
}
