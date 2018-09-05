using UnityEngine;

public class StrongAttackDetection : MonoBehaviour
{
    #region Fields
    public LayerMask layerMask;
    public float startingSize;

    private Projector projector;
    private new SphereCollider collider;
    private Player player;
    [SerializeField]
    private Color decalOriginalColor;
    [SerializeField]
    private Color decalFinalColor;
    private Color colorDiffernce;
    private Material decalMat;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        projector = GetComponent<Projector>();
        UnityEngine.Assertions.Assert.IsNotNull(projector, "ERROR: A Projector Component could not be found by StrongAttackDetection in GameObject " + gameObject.name);
        projector.orthographicSize = startingSize;
        projector.material = new Material(projector.material);
        decalMat = projector.material;

        decalMat.SetColor("[HDR]_TintColor", decalOriginalColor);
        colorDiffernce = decalFinalColor - decalOriginalColor;

        collider = GetComponent<SphereCollider>();
        UnityEngine.Assertions.Assert.IsNotNull(collider, "ERROR: A Collider Component could not be found by StrongAttackDetection in GameObject " + gameObject.name);
        collider.radius = startingSize;
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

    public void IncreaseSize(float time)
    {
        collider.radius = startingSize + time;
        projector.orthographicSize = startingSize + time;
    }

    public void ResetSize()
    {
        projector.orthographicSize = startingSize;
        collider.radius = startingSize;
    }

    public void ChangeDecalColor(float time)
    {
        decalMat.SetColor("Tint Color", decalOriginalColor + colorDiffernce * time);
    }
    #endregion
}
