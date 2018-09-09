using UnityEngine;
using UnityEngine.UI;

public class UIFlasher : MonoBehaviour
{
    #region Fields
    public bool flashOnAwake = false;
    [SerializeField]
    [Tooltip("The number of cycles done per second.")]
    private float frequency = 1.0f;

    [Header("Scale")]
    [SerializeField]
    private bool shouldScale = true;
    [SerializeField]
    [Tooltip("The normalized (to initial scale) amplitude of a grow-shrink cycle")]
    [Range(0.0f, 1.0f)]
    private float amplitude = 0.1f;

    [Header("Color")]
    [SerializeField]
    private bool shouldColor = false;
    [SerializeField]
    private Color baseColor = Color.white;
    [SerializeField]
    private Color flashColor = Color.red;


    private RectTransform targetTransform;
    private Graphic uiGraphic;
    private Vector3 initialScale;
    private bool shouldStop;
    private float elapsedTime;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        targetTransform = GetComponent<RectTransform>();
        UnityEngine.Assertions.Assert.IsNotNull(targetTransform, "ERROR: A RectTransform Component could not be found by UIFlasher in GameObject " + gameObject.name);
        initialScale = targetTransform.localScale;

        if (shouldColor)
        {
            uiGraphic = GetComponent<Graphic>();
            UnityEngine.Assertions.Assert.IsNotNull(uiGraphic, "ERROR: A Graphic Component (Text, Image, etc.) could not be found by UIFlasher in GameObject " + gameObject.name + "! A Graphic component must be available if Should Color is ticked.");
        }

        if (flashOnAwake)
            enabled = true;
        else
            enabled = false;
    }

    private void Update()
    {
        float cycleTime = 1 / frequency;

        if (shouldScale)
            ScaleAnimation(cycleTime);

        if (shouldColor)
            ColorAnimation(cycleTime);

        elapsedTime += Time.deltaTime;
        if (elapsedTime > cycleTime)
        {
            elapsedTime -= cycleTime;
            if (shouldStop)
            {
                Stop();
            }
        }
    }
    #endregion

    #region Public Methods
    public void RequestStartFlash()
    {
        if (!enabled)
        {
            enabled = true;
            elapsedTime = 0;
        }
    }

    public void RequestStopFlash()
    {
        if (enabled)
            shouldStop = true;
    }
    #endregion

    #region Private Methods
    private void ScaleAnimation(float cycleTime)
    {
        float scaleFactor;
        if (elapsedTime < 0.25f * cycleTime)
        {
            // Grow
            scaleFactor = Mathf.Lerp(1.0f, 1.0f + amplitude, elapsedTime / (0.25f * cycleTime));
        }
        else if (elapsedTime < 0.75f * cycleTime)
        {
            // Shrink
            scaleFactor = Mathf.Lerp(1.0f + amplitude, 1.0f - amplitude, (elapsedTime - 0.25f * cycleTime) / (0.5f * cycleTime));
        }
        else
        {
            // Grow
            scaleFactor = Mathf.Lerp(1.0f - amplitude, 1.0f, (elapsedTime - 0.75f * cycleTime) / (0.25f * cycleTime));
        }
        targetTransform.localScale = initialScale * scaleFactor;
    }

    private void ColorAnimation(float cycleTime)
    {
        Color targetColor;
        if (elapsedTime < 0.25f * cycleTime)
        {
            // Go red
            targetColor = Color.Lerp(baseColor, flashColor, elapsedTime / (0.25f * cycleTime));
        }
        else if (elapsedTime < 0.5f * cycleTime)
        {
            // Go white
            targetColor = Color.Lerp(flashColor, baseColor, (elapsedTime - 0.25f * cycleTime) / (0.5f * cycleTime));
        }
        else if (elapsedTime < 0.75f * cycleTime)
        {
            // Go red
            targetColor = Color.Lerp(baseColor, flashColor, (elapsedTime - 0.5f * cycleTime) / (0.25f * cycleTime));
        }
        else
        {
            // Go white
            targetColor = Color.Lerp(flashColor, baseColor, (elapsedTime - 0.75f * cycleTime) / (0.25f * cycleTime));
        }
        uiGraphic.color = targetColor;
    }

    private void Stop()
    {
        shouldStop = false;
        if (shouldScale)
            targetTransform.localScale = initialScale;
        if (shouldColor)
            uiGraphic.color = baseColor;
        enabled = false;
    }

    private float Interpolate(float start, float end, float u)
    {
        return start * (1-u) + end * u;
    }
    #endregion
}
