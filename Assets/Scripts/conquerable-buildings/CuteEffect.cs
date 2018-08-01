using System.Collections.Generic;
using UnityEngine;

public class CuteEffect : MonoBehaviour
{
    #region Fields
    [SerializeField]
    [Tooltip("Normalized value that represent the amount of damage that the Zone's Monument must have received for this CuteEffects to trigger")]
    [Range(0.0f, 1.0f)]
    private float monumentDamage;
    [SerializeField]
    [Tooltip("The time (in seconds) that will be waited before triggering the effect AFTER the monument has reached monumentDamage")]
    private float delay;
    [SerializeField]
    private List<Convertible> convertibles;

    private AIZoneController zoneController;
    private bool runningEffect = false;
    private bool finished = false;
    private float delayElapsedTime = 0.0f;
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
                PerformEffect();
            else
                delayElapsedTime += Time.deltaTime;
        }
    }

    private void OnDestroy()
    {
        if (!finished)
            zoneController.RemoveCuteEffect(this);
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

    private void PerformEffect()
    {
        foreach (Convertible convertible in convertibles)
            convertible.Convert();

        FinishEffect();
    }

    private void FinishEffect()
    {
        zoneController.RemoveCuteEffect(this);

        runningEffect = false;
        finished = true;
        enabled = false;
    }
    #endregion
}
