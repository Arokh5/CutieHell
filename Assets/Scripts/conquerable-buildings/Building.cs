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

    protected float currentHealth;

    [Header("Elements setup")]
    [SerializeField]
    private MeshRenderer buildingRenderer;
    [SerializeField]
    private MeshRenderer alternateBuildingRenderer;
    [SerializeField]
    private MeshRenderer areaRenderer;

    [Header("Area of Effect")]
    [SerializeField]
    private float effectRadius = 5;
    [SerializeField]
    private List<Convertible> convertibles;

    [Header("Life and stuff")]
    [Tooltip("The duration (in seconds) for which the conquerable object is considered to be \"under attack\" after the last actual attack happened.")]
    [SerializeField]
    private float underAttackStateDuration = 1;
    [Tooltip("The duration (in seconds) that the dark to cute conversion takes.")]
    [SerializeField]
    private float conquerEffectDuration = 1;

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
    public int lifeLossPerSecond = 0; // TEST
    public bool restoreLife = false;
    private float accumulatedLifeLoss = 0;

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
        areaRenderer.material.SetFloat("_AlternateStartRadius", 0);
        areaRenderer.material.SetFloat("_ConquerFactor", 0);
        areaRenderer.material.SetFloat("_Conquered", 0);

        conquered = false;
    }

    private void Awake()
    {
        UnityEngine.Assertions.Assert.IsNotNull(buildingRenderer, "Building Renderer not assigned for ConquerableBuilding script in GameObject " + gameObject.name);
        UnityEngine.Assertions.Assert.IsNotNull(alternateBuildingRenderer, "Alternate Building Renderer not assigned for ConquerableBuilding script in GameObject " + gameObject.name);
        UnityEngine.Assertions.Assert.IsNotNull(areaRenderer, "Area Renderer not assigned for ConquerableBuilding script in GameObject " + gameObject.name);
        buildingRenderer.gameObject.SetActive(true);
        alternateBuildingRenderer.gameObject.SetActive(false);
        areaRenderer.gameObject.SetActive(true);
        areaRenderer.transform.localScale = new Vector3(effectRadius * 2 / 10, effectRadius * 2 / 10, effectRadius * 2 / 10);
    }

    private void Start()
    {
        Reset();
    }

    // Update is called once per frame
    private void Update()
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
        SetUnderAttack(true);
        underAttackElapsedTime = 0;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
        }

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
        return fullRepairCost;
    }
    #endregion

    #region Protected Methods
    protected abstract void BuildingKilled();
    #endregion

    #region Private Methods
    private void Test()
    {
        if (reset)
        {
            reset = false;
            loseHitPoints = false;
            Unconquer();
        }

        if (restoreLife)
        {
            restoreLife = false;
            Unconquer();
            FullRepair();
        }

        if (loseHitPoints)
        {
            accumulatedLifeLoss += lifeLossPerSecond * Time.deltaTime;
            if (accumulatedLifeLoss > 1)
            {
                accumulatedLifeLoss -= 1;
                TakeDamage(1, AttackType.ENEMY);
                if (conquered)
                    loseHitPoints = false;
            }
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
        areaRenderer.material.SetFloat("_AlternateStartRadius", progress);

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

        float limitRadius = effectRadius * progress;
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
        areaRenderer.material.SetFloat("_Conquered", 1);
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
        areaRenderer.material.SetFloat("_ConquerFactor", conqueredFactor);
    }
    #endregion
}
