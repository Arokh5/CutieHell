using UnityEngine;

public class SpawnerMover : MonoBehaviour, IZoneTakenListener
{
    #region Fields
    [SerializeField]
    [Tooltip("The AIZoneController to listen to")]
    private AIZoneController referenceZone;
    [SerializeField]
    [Tooltip("The Spawner that should be repositioned when the Reference Zone is taken")]
    private AISpawner spawnerToMove;
    [SerializeField]
    [Tooltip("The Transform that will be applied to the Spawner To Move when the Reference Zone is taken")]
    private Transform targetTransform;
    [SerializeField]
    [Tooltip("The AIZoneController to assign to the Spawner To Move after moving it. If left as null, no AIZoneController change takes place")]
    private AIZoneController newZoneController;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        UnityEngine.Assertions.Assert.IsNotNull(referenceZone, "ERROR: Reference Zone (AIZoneController) not assigned for SpawnerMover script in GameObject " + gameObject.name);
        UnityEngine.Assertions.Assert.IsNotNull(spawnerToMove, "ERROR: Spawner To Move (AISpawner) not assigned for SpawnerMover script in GameObject " + gameObject.name);
        UnityEngine.Assertions.Assert.IsNotNull(targetTransform, "ERROR: Target Transform (Transform) not assigned for SpawnerMover script in GameObject " + gameObject.name);
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
        MoveSpawner();
    }
    #endregion

    #region Private Methods
    private void MoveSpawner()
    {
        spawnerToMove.transform.position = targetTransform.position;
        if (newZoneController)
            spawnerToMove.SetZoneController(newZoneController);
    }
    #endregion
}
