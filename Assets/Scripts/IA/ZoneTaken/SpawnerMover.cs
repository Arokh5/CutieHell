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
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        UnityEngine.Assertions.Assert.IsNotNull(referenceZone, "ERROR: Reference Zone (AIZoneController) not assigned for SpawnerMover script in GameObject " + gameObject.name);
        UnityEngine.Assertions.Assert.IsNotNull(spawnerToMove, "ERROR: Spawner To Move (AISpawner) not assigned for SpawnerMover script in GameObject " + gameObject.name);
        UnityEngine.Assertions.Assert.IsNotNull(targetTransform, "ERROR: Target Transform (Transform) not assigned for SpawnerMover script in GameObject " + gameObject.name);
    }
    #endregion

    #region Public Methods
    // IZoneTakenListener
    public void OnZoneTaken()
    {
        throw new System.NotImplementedException();
    }
    #endregion

    #region Private Methods
    private void MoveSpawner()
    {
        spawnerToMove.transform.position = targetTransform.position;
    }
    #endregion
}
