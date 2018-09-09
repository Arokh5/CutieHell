using UnityEngine;

public class MinimapElement : MonoBehaviour
{
    public Sprite sprite;
    public Color color = Color.white;
    [Tooltip("The size in pixels of the sprite shown in the minimap.")]
    public int size = 20;
    [Tooltip("Elements with a higher priority number are drawn on top.")]
    public int priority = 0;
    [Tooltip("Determines whether having this element outside of the bounds of the minimap triggers the alert corresponding to the minimap border")]
    public bool triggersAlert = false;
    public MinimapController.MinimapImageType minimapImageType;
    [Tooltip("Determines whether the minimap image rotates with the GameObject's rotation")]
    public bool updatesRotation = false;

    private bool hasStarted = false;
    private bool effectRequested = false;

    #region MonoBehaviour Methods
    private void Start()
    {
        hasStarted = true;
        MinimapController.instance.AddMinimapElement(this);
    }

    private void OnEnable()
    {
        if (hasStarted)
            MinimapController.instance.AddMinimapElement(this);
    }

    private void OnDisable()
    {
        MinimapController.instance.RemoveMinimapElement(this);
    }
    #endregion

    #region Public Methods
    public void RequestEffect()
    {
        effectRequested = true;
    }

    public bool ExtractEffectRequestState()
    {
        bool response = effectRequested;
        effectRequested = false;
        return response;
    }
    #endregion
}
