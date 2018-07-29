using UnityEngine;

public class PathsChanger : MonoBehaviour, IZoneTakenListener
{
    #region Fields
    [SerializeField]
    [Tooltip("The AIZoneController to listen to")]
    private AIZoneController referenceZone;
    [SerializeField]
    [Tooltip("The AIZoneController whose AIPaths will be replaced when the Reference Zone is taken")]
    private AIZoneController targetZone;
    [SerializeField]
    [Tooltip("The new AIPaths that will be assigned to the Target Zone when the Reference Zone is taken")]
    private PathsController paths;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        UnityEngine.Assertions.Assert.IsNotNull(referenceZone, "ERROR: Reference Zone (AIZoneController) not assigned for PathsChanger script in GameObject " + gameObject.name);
        UnityEngine.Assertions.Assert.IsNotNull(targetZone, "ERROR: Target Zone (AIZoneController) not assigned for PathsChanger script in GameObject " + gameObject.name);
        UnityEngine.Assertions.Assert.IsNotNull(paths, "ERROR: Paths (PathsController) not assigned for PathsChanger script in GameObject " + gameObject.name);

    }
    #endregion

    #region Public Methods
    // IZoneTakenListener
    public void OnZoneTaken()
    {
        ApplyPaths();
    }
    #endregion

    #region Private Methods
    private void ApplyPaths()
    {
        targetZone.SetPathsController(paths);
    }
    #endregion
}
