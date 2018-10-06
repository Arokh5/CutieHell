using UnityEngine;

[RequireComponent(typeof(UIFlasher))]
public class MinimapEnableFlashImage : MinimapImage
{
    #region MonoBehaviour Methods
    [SerializeField]
    [Tooltip("(Optional) The sfx clip to play every time this minimap Image enters the minimap area.")]
    private AudioClip clip;

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
    #endregion

    #region Protected Methods
    protected override void OnImageShown()
    {
        // Flash once
        uiFlasher.RequestStartFlash();
        uiFlasher.RequestStopFlash();

        if (clip)
        {
            SoundManager.instance.PlaySfxClip(clip);
        }
    }
    #endregion
}
