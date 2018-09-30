using UnityEngine;

public class MonumentDamagerTargetChanger : MonoBehaviour, IZoneTakenListener
{
    #region Fields
    [SerializeField]
    [Tooltip("The AIZoneController to listen to")]
    private AIZoneController referenceZone;
    [SerializeField]
    [Tooltip("The new target that will be assigned to the MonumentDamager when the Reference Zone is conquered")]
    private Monument newTarget;

    private MonumentDamager monumentDamager;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        UnityEngine.Assertions.Assert.IsNotNull(referenceZone, "ERROR: Reference Zone (AIZoneController) not assigned for MonumentDamagerTargetChanger script in GameObject " + gameObject.name);
        UnityEngine.Assertions.Assert.IsNotNull(referenceZone, "ERROR: New Target (Monument) not assigned for MonumentDamagerTargetChanger script in GameObject " + gameObject.name);

        monumentDamager = GetComponentInParent<MonumentDamager>();
        UnityEngine.Assertions.Assert.IsNotNull(monumentDamager, "ERROR: A MonumentDamager Component could not be found in the parent hierarchy of MonumentDamagerTargetChanger script in GameObject " + gameObject.name);
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
        if (monumentDamager)
        {
            monumentDamager.SetTargetMonument(newTarget);
        }
    }
    #endregion
}
