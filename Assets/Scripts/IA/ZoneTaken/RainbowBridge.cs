using UnityEngine;
using UnityEngine.AI;

public class RainbowBridge : MonoBehaviour, IZoneTakenListener
{
    #region Fields
    [SerializeField]
    private GameObject bridge;
    [SerializeField]
    private NavMeshObstacle navObstacle;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        UnityEngine.Assertions.Assert.IsNotNull(bridge, "ERROR: Bridge (GameObject) not assigned for RainbowBridge script in GameObject " + gameObject.name);
        UnityEngine.Assertions.Assert.IsNotNull(bridge, "ERROR: Nav Obstacle (NavMeshObstacle) not assigned for RainbowBridge script in GameObject " + gameObject.name);
    }

    private void Start()
    {
        Close();
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
    }

    public void Close()
    {
        bridge.SetActive(false);
        navObstacle.gameObject.SetActive(true);
    }
    #endregion
}
