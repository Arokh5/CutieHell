using UnityEngine;

public class ZoneConnection : MonoBehaviour, IZoneTakenListener
{
    [System.Serializable]
    private class OutletInfo
    {
        public AIZoneController zoneController;
        public bool ignoreEnemies;
        public bool ignorePlayer;
    }

    #region Fields
    [SerializeField]
    private OutletInfo backwardOutlet;
    [SerializeField]
    private OutletInfo forwardOutlet;
    [SerializeField]
    [Tooltip("Optional: The AIZoneController to listen to")]
    private AIZoneController referenceZone;
    [SerializeField]
    [Tooltip("Optional: The Cute Wall that Vlad can't walk through. This requires a referenceZone.")]
    private ParticleSystem cuteWall;
    
    private const float knockbackTargetDistance = 4.0f;
    #endregion

    #region MonoBehavior Methods
    private void Awake()
    {
        if (referenceZone)
            UnityEngine.Assertions.Assert.IsNotNull(cuteWall, "ERROR: A Reference Zone (AIZoneController) has been assigned for ZoneConnection in gameObject '" + gameObject.name + "', but a Cute Wall (ParticleSystem) is missing. You should assign both Objects or none of them.");
        if (cuteWall)
            UnityEngine.Assertions.Assert.IsNotNull(referenceZone, "ERROR: A Cute Wall (ParticleSystem) has been assigned for ZoneConnection in gameObject '" + gameObject.name + "', but a Reference Zone (AIZoneController) is missing. You should assign both Objects or none of them.");
    }

    private void Start()
    {
        if (referenceZone)
        {
            referenceZone.AddIZoneTakenListener(this);
            if (referenceZone.isConquered)
                Close();
            else
                Open();
        }
    }

    private void OnDestroy()
    {
        if (referenceZone)
            referenceZone.RemoveIZoneTakenListener(this);
    }

    private void OnTriggerExit(Collider other)
    {
        OutletInfo outletInfo = GetOutletInfo(other.transform.position);

        AIEnemy enemy = null;
        if (!outletInfo.ignoreEnemies)
            enemy = other.GetComponent<AIEnemy>();

        if (enemy)
        {
            enemy.SetZoneController(outletInfo.zoneController);
        }
        else
        {
            Player player = null;
            if (!outletInfo.ignorePlayer)
                player = other.GetComponentInParent<Player>();

            if (player)
            {
                bool isForward = outletInfo == forwardOutlet;
                Vector3 targetPos = transform.position + (isForward ? -1 : 1) * knockbackTargetDistance * transform.forward;
                Vector3 knockbackDirection = targetPos - player.transform.position;
                knockbackDirection.y = 0.0f;
                player.SetZoneController(outletInfo.zoneController, knockbackDirection);
            }
        }
    }
    #endregion

    #region Public Methods
    // IZoneTakenListener
    public void OnZoneTaken()
    {
        Close();
    }
    #endregion

    #region Private Methods
    private OutletInfo GetOutletInfo(Vector3 pos)
    {
        if (Vector3.Dot(transform.forward, pos - transform.position) > 0)
            return forwardOutlet;
        else
            return backwardOutlet;
    }

    private void Open()
    {
        if (cuteWall)
        {
            cuteWall.Stop();
            cuteWall.gameObject.SetActive(false);
        }
    }

    private void Close()
    {
        if (cuteWall)
        {
            cuteWall.gameObject.SetActive(true);
            cuteWall.Play();
        }
    }
    #endregion
}
