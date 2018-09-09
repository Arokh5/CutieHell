using UnityEngine;

[RequireComponent(typeof(UIFlasher))]
public class MinimapFlashImage : MinimapImage
{

    #region MonoBehaviour Methods
    [SerializeField]
    [Tooltip("The duration (in seconds) of the flashing effect counted from the last effect request.")]
    private float flashDuration;

    private UIFlasher uiFlasher;
    private float lastRequestTime;
    private bool effectActive;
    #endregion

    #region MonoBehaviour Methods
    protected new void Awake()
    {
        base.Awake();
        uiFlasher = GetComponent<UIFlasher>();
        UnityEngine.Assertions.Assert.IsNotNull(uiFlasher, "ERROR: An UIFlasher could not be found by MinimapFlashImage in GameObject " + gameObject.name);
    }

    private void Update()
    {
        if (effectActive)
        {
            if (Time.time - lastRequestTime > flashDuration)
            {
                effectActive = false;
                uiFlasher.RequestStopFlash();
            }
        }
    }
    #endregion

    #region Public Methods
    public override void RequestEffect()
    {
        effectActive = true;
        lastRequestTime = Time.time;
        uiFlasher.RequestStartFlash();
    }
    #endregion
}
