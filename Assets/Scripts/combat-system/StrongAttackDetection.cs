using UnityEngine;

public class StrongAttackDetection : MonoBehaviour
{
    #region Fields
    public LayerMask layerMask;

    private Projector projector;
    private new Collider collider;
    private Player player;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        projector = GetComponent<Projector>();
        UnityEngine.Assertions.Assert.IsNotNull(projector, "ERROR: A Projector Component could not be found by StrongAttackDetection in GameObject " + gameObject.name);

        collider = GetComponent<Collider>();
        UnityEngine.Assertions.Assert.IsNotNull(collider, "ERROR: A Collider Component could not be found by StrongAttackDetection in GameObject " + gameObject.name);
        Deactivate();
    }

    private void Start()
    {
        player = GameManager.instance.GetPlayer1();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Helpers.GameObjectInLayerMask(other.gameObject, layerMask))
        {
            AIEnemy aIEnemy = other.GetComponent<AIEnemy>();
            if (aIEnemy)
            {
                aIEnemy.MarkAsTarget(true);
                player.currentStrongAttackTargets.Add(aIEnemy);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (Helpers.GameObjectInLayerMask(other.gameObject, layerMask))
        {
            AIEnemy aIEnemy = other.GetComponent<AIEnemy>();
            if (aIEnemy)
            {
                aIEnemy.MarkAsTarget(false);
                player.currentStrongAttackTargets.Remove(aIEnemy);
            }
        }
    }
    #endregion

    #region Public Methods
    public void Activate(bool useProjector = true)
    {
        collider.enabled = true;
        if (useProjector)
            projector.enabled = true;
    }

    public void Deactivate()
    {
        collider.enabled = false;
        projector.enabled = false;
    }
    #endregion
}
