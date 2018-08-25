using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFlasher : MonoBehaviour
{
    #region Fields
    [Tooltip("The number of grow-shrink cycles done per second.")]
    public float frequency = 1.0f;
    [Tooltip("The normalized (to initial scale) amplitude of a grow-shrink cycle")]
    [Range(0.0f, 1.0f)]
    public float amplitude = 0.1f;
    public bool flashOnAwake = false;

    private RectTransform targetTransform;
    Vector3 initialScale;
    private bool shouldStop;
    float elapsedTime;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        targetTransform = GetComponent<RectTransform>();
        UnityEngine.Assertions.Assert.IsNotNull(targetTransform, "ERROR: A RectTransform Component could not be found by UIFlasher in GameObject " + gameObject.name);
        initialScale = targetTransform.localScale;
        if (flashOnAwake)
            enabled = true;
        else
            enabled = false;
    }

    private void Update()
    {
        FlashAnimation();
    }
    #endregion

    #region Public Methods
    public void StartFlash()
    {
        enabled = true;
        elapsedTime = 0;
    }

    public void CancelFlash()
    {
        if (enabled)
            shouldStop = true;
    }
    #endregion

    #region Private Methods
    private void FlashAnimation()
    {
        float cycleTime = 1 / frequency;
        elapsedTime += Time.deltaTime;
        if (elapsedTime > cycleTime)
        {
            if (shouldStop)
            {
                Stop();
            }
            elapsedTime -= cycleTime;
        }

        float scaleFactor;
        if (elapsedTime < 0.25f * cycleTime)
        {
            // Grow
            scaleFactor = Interpolate(1.0f, 1.0f + amplitude, elapsedTime / (0.25f * cycleTime));
        }
        else if (elapsedTime < 0.75f * cycleTime)
        {
            scaleFactor = Interpolate(1.0f + amplitude, 1.0f - amplitude, (elapsedTime - 0.25f * cycleTime) / (0.5f * cycleTime));
        }
        else
        {
            // Grow
            scaleFactor = Interpolate(1.0f - amplitude, 1.0f, (elapsedTime - 0.75f * cycleTime) / (0.25f * cycleTime));
        }
        targetTransform.localScale = initialScale * scaleFactor;
    }

    private void Stop()
    {
        shouldStop = false;
        targetTransform.localScale = initialScale;
        enabled = false;
    }

    private float Interpolate(float start, float end, float u)
    {
        return start * (1-u) + end * u;
    }
    #endregion
}
