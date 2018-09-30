using UnityEngine;

public class EnemiesClearer : MonoBehaviour, IZoneTakenListener
{
    #region Fields
    [SerializeField]
    [Tooltip("The AIZoneController to listen to")]
    private AIZoneController referenceZone;
    [SerializeField]
    private ScenarioController scenarioController;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        UnityEngine.Assertions.Assert.IsNotNull(referenceZone, "ERROR: Reference Zone (AIZoneController) not assigned for EnemiesClearer script in GameObject " + gameObject.name);
        UnityEngine.Assertions.Assert.IsNotNull(scenarioController, "ERROR: Scenario Controller (ScenarioController) not assigned for EnemiesClearer script in GameObject " + gameObject.name);
    }

    private void Start()
    {
        referenceZone.AddIZoneTakenListener(this);
    }

    private void OnDestroy()
    {
        referenceZone.RemoveIZoneTakenListener(this);
    }
    #endregion

    #region Public Methods
    // IZoneTakenListener
    public void OnZoneTaken()
    {
        scenarioController.ClearCurrentActiveEnemies();
    }
    #endregion
}
