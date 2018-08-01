using UnityEngine;
using UnityEngine.AI;

public class RainbowBridge : MonoBehaviour, IZoneTakenListener
{
    #region Fields
    [SerializeField]
    [Tooltip("The AIZoneController to listen to")]
    private AIZoneController referenceZone;
    [SerializeField]
    [Tooltip("The Rainbow Bridge that must be turned on when the Reference Zone is taken")]
    private GameObject bridge;
    [SerializeField]
    [Tooltip("The Obstacle that must be turned off when the Reference Zone is taken")]
    private NavMeshObstacle navObstacle;
    [SerializeField]
    [Tooltip("The Obstacle that must be turned off when the Reference Zone is taken")]
    private Collider[] playerBlockages;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        UnityEngine.Assertions.Assert.IsNotNull(referenceZone, "ERROR: Reference Zone (AIZoneController) not assigned for RainbowBridge script in GameObject " + gameObject.name);
        UnityEngine.Assertions.Assert.IsNotNull(bridge, "ERROR: Bridge (GameObject) not assigned for RainbowBridge script in GameObject " + gameObject.name);
        UnityEngine.Assertions.Assert.IsNotNull(navObstacle, "ERROR: Nav Obstacle (NavMeshObstacle) not assigned for RainbowBridge script in GameObject " + gameObject.name);
        Collider bridgeCollider = bridge.GetComponent<Collider>();
        bridgeCollider.enabled = false;
    }

    private void Start()
    {
        Close();
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
        Open();
    }

    public void Open()
    {
        bridge.SetActive(true);
        navObstacle.gameObject.SetActive(false);
        foreach (Collider blockage in playerBlockages)
            blockage.gameObject.SetActive(true);
    }

    public void Close()
    {
        bridge.SetActive(false);
        navObstacle.gameObject.SetActive(true);
        foreach (Collider blockage in playerBlockages)
            blockage.gameObject.SetActive(false);
    }
    #endregion
}
