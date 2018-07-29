using UnityEngine;

public class ZoneTargetUpdater : MonoBehaviour, IZoneTakenListener
{
    #region Fields
    [SerializeField]
    [Tooltip("The AIZoneController to listen to")]
    private AIZoneController referenceZone;
    [SerializeField]
    [Tooltip("The AIZoneController whose zoneTarget will be updated when the Reference Zone is taken")]
    private AIZoneController targetZone;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        UnityEngine.Assertions.Assert.IsNotNull(referenceZone, "ERROR: Reference Zone (AIZoneController) not assigned for PathsChanger script in GameObject " + gameObject.name);
        UnityEngine.Assertions.Assert.IsNotNull(targetZone, "ERROR: Target Zone (AIZoneController) not assigned for PathsChanger script in GameObject " + gameObject.name);
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
        UpdateZoneTarget();
    }
    #endregion

    #region Private Methods
    private void UpdateZoneTarget()
    {
        targetZone.UpdateZoneTarget();
    }
    #endregion
}
