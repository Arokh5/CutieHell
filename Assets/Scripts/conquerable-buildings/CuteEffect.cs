using System.Collections.Generic;
using UnityEngine;

public class CuteEffect : MonoBehaviour
{
    #region Fields
    [SerializeField]
    [Tooltip("The maximum radius to which the model changing effect spreads over time (any PropModelChanger outside of the radius is converted at the end of the effect)")]
    private float effectRadius = 5.0f;
    [SerializeField]
    [Tooltip("The time (in seconds) that the model changing effect takes to go from the center to the effectRadius")]
    private float effectTime = 1.0f;
    [SerializeField]
    [Tooltip("Normalized value that represent the amount of damage that the Zone's Monument must have received for this CuteEffects to trigger")]
    [Range(0.0f, 1.0f)]
    private float monumentDamage;
    [SerializeField]
    [Tooltip("The time (in seconds) that will be waited before triggering the effect AFTER the monument has reached monumentDamage")]
    private float delay;
    [SerializeField]
    private List<PropModelChanger> modelChangers;

    private AIZoneController zoneController;
    private bool runningEffect = false;
    private bool finished = false;
    private float delayElapsedTime = 0.0f;
    private float elapsedTime = 0.0f;
    private float currentMaxEffectRadius;
    private List<PropModelChanger> toRemove = new List<PropModelChanger>();
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
            currentMaxEffectRadius = effectRadius;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, 0.75f);
        Gizmos.DrawWireSphere(transform.position, effectRadius);
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

        currentMaxEffectRadius = u * effectRadius;

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

        zoneController.RemoveCuteEffect(this);

        runningEffect = false;
        finished = true;
        enabled = false;
    }

    #endregion
}
