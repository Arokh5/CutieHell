using UnityEngine;

public class MonumentDamager : MonoBehaviour
{
    #region Fields
    [SerializeField]
    private Monument targetMonument;
    [SerializeField]
    private float dps = 50.0f;
    [SerializeField]
    private ControllerButton damageButton = ControllerButton.L1;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        UnityEngine.Assertions.Assert.IsNotNull(targetMonument, "ERROR: Target Monument (Monument) not assigned for MonumentDamager script in GameObject " + gameObject.name);
    }

    private void Update()
    {
        if (targetMonument && InputManager.instance.GetButton(damageButton))
        {
            targetMonument.TakeDamage(dps * Time.deltaTime, AttackType.NONE);
        }
    }
    #endregion

    #region Public Methods
    public void SetTargetMonument(Monument newTarget)
    {
        targetMonument = newTarget;
    }
    #endregion
}
