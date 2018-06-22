using System.Collections.Generic;
using UnityEngine;

public class CuteEffect : MonoBehaviour, ITextureChanger
{
    #region Fields
    [SerializeField]
    [Tooltip("The absolute radius at which the blending of textures ends")]
    private float effectMaxBlendedRadius = 5.0f;
    [SerializeField]
    [Tooltip("Normalized value that represent the percentage of the effectRadius from which a blend towards 0 starts")]
    [Range(0.0f, 1.0f)]
    private float effectStartBlendRadius = 0.9f;
    [SerializeField]
    private float effectTime = 1.0f;
    [SerializeField]
    [Tooltip("Normalized value that represent the amount of damage that the Zone's Monument must have received for this CuteEffects to trigger")]
    [Range(0.0f, 1.0f)]
    private float monumentDamage;
    [SerializeField]
    [Tooltip("The time (in seconds) that will be waited before triggering the effect AFTER the monument has reached monumentDamage")]
    private float delay;
    [SerializeField]
    AIZoneController zoneController;
    [SerializeField]
    List<PropModelChanger> modelChangers;
    [ShowOnly]
    [SerializeField]
    private bool runningEffect = false;
    private bool finished = false;

    private float delayElapsedTime = 0.0f;
    private float elapsedTime = 0.0f;
    private float currentMaxEffectRadius;
    List<PropModelChanger> toRemove = new List<PropModelChanger>();
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        if (!zoneController)
            zoneController = GetComponentInParent<AIZoneController>();
        UnityEngine.Assertions.Assert.IsNotNull(zoneController, "ERROR: CuteEffects could not find an AIZoneController in its parent hierarchy in gameObject '" + gameObject.name + "'");
    }

    private void Start()
    {
        // Register to the Monument (through the ZoneController)
        zoneController.AddCuteEffects(this);

        if (monumentDamage <= 0)
            runningEffect = true;
    }

    private void Update()
    {
        if (!finished && runningEffect)
        {
            if (delayElapsedTime >= delay)
                UpdateEffect();
            else
                delayElapsedTime += Time.deltaTime;
        }
    }

    private void OnDestroy()
    {
        if (!finished)
            zoneController.RemoveCuteEffect(this);
    }

    private void OnValidate()
    {
        if (finished)
            currentMaxEffectRadius = effectMaxBlendedRadius;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, 0.75f);
        Gizmos.DrawWireSphere(transform.position, effectMaxBlendedRadius);
        Gizmos.color = new Color(0, 1, 0, 0.25f);
        Gizmos.DrawWireSphere(transform.position, effectMaxBlendedRadius * effectStartBlendRadius);
    }
    #endregion

    #region Public Methods
    public void InformMonumentDamage(float normalizedDamage)
    {
        if (runningEffect)
            return;

        if (normalizedDamage < 0 || normalizedDamage > 1)
        {
            Debug.LogError("ERROR: CuteEffect::InformMonumentDamage called with a value for normalizedDamage outside if the [0, 1] range!");
            return;
        }

        if (normalizedDamage >= monumentDamage)
            StartEffect();
    }

    // ITextureChanger
    public float GetNormalizedBlendStartRadius()
    {
        return effectStartBlendRadius;
    }

    // ITextureChanger
    public float GetEffectMaxRadius()
    {
        return currentMaxEffectRadius;
    }
    #endregion

    #region Private Methods
    private void StartEffect()
    {
        runningEffect = true;
    }

    private void UpdateEffect()
    {
        elapsedTime += Time.deltaTime;

        float u = elapsedTime / effectTime;
        if (u > 1)
            u = 1;

        currentMaxEffectRadius = u * effectMaxBlendedRadius;

        foreach (PropModelChanger modelChanger in modelChangers)
        {
            Vector3 modelChangerToThis = transform.position - modelChanger.transform.position;
            if (currentMaxEffectRadius * currentMaxEffectRadius > modelChangerToThis.sqrMagnitude)
            {
                modelChanger.Convert();
                toRemove.Add(modelChanger);
            }
        }

        foreach (PropModelChanger modelChanger in toRemove)
            modelChangers.Remove(modelChanger);

        if (u == 1)
            FinishEffect();
    }

    private void FinishEffect()
    {
        foreach (PropModelChanger modelChanger in modelChangers)
            modelChanger.Convert();
        modelChangers.Clear();

        runningEffect = false;
        finished = true;
        enabled = false;
    }

    #endregion
}
