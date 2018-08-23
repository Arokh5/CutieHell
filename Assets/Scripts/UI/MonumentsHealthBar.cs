using UnityEngine;
using UnityEngine.UI;

public class MonumentsHealthBar : MonoBehaviour
{
    [System.Serializable]
    private class FlashInfo
    {
        public float flashCycleDuration = 0.5f;
        [Range(1.0f, 2.0f)]
        public float scaleFactor = 1.1f;
        public Color flashTintColor = Color.red;
    }

    #region Fields
    [Header("Element setup")]
    [SerializeField]
    private Image backgroundImage;
    [SerializeField]
    private Image healthImage;
    [SerializeField]
    private Text barText;

    private float currentFill = 1.0f;

    [Header("Refill animation setup")]
    [SerializeField]
    [Tooltip("The time (in seconds) it takes to animate the health bar being refilled.")]
    private float refillAnimationDuration = 1.0f;

    private bool refilling;
    private float refillElapsedTime;

    [Header("Flashing setup")]
    [SerializeField]
    [Tooltip("The minimum value for the health bar fill below which the bar begins flashing")]
    public float flashThreshold = 0.2f;
    [SerializeField]
    [Tooltip("The time (in seconds) counted from the last health bar update in which the Strong Flash Info is used. Ohterwise, the Weak Flash Info is used.")]
    public float strongFlashDuration = 2.0f;
    [SerializeField]
    [Tooltip("The flash parameters to use for weak flashing.")]
    private FlashInfo weakFlashInfo;
    [SerializeField]
    [Tooltip("The flash parameters to use for strong flashing.")]
    private FlashInfo strongFlashInfo;

    private RectTransform rectTransform;
    private bool flashing = false;
    private bool shouldStopFlashing = false;
    private float flashElapsedTime;
    private FlashInfo currentFlashInfo;
    private float strongFlashTimeLeft;
    private Vector3 initialScale;
    private Color initialColor;

    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        UnityEngine.Assertions.Assert.IsNotNull(backgroundImage, "ERROR: Background Image (Image) not assigned for MonumentsHealthBar in gameObject '" + gameObject.name + "'");
        UnityEngine.Assertions.Assert.IsNotNull(healthImage, "ERROR: Health Image (Image) not assigned for MonumentsHealthBar in gameObject '" + gameObject.name + "'");
        UnityEngine.Assertions.Assert.IsNotNull(barText, "ERROR: Bar Text (Text) not assigned for MonumentsHealthBar in gameObject '" + gameObject.name + "'");
        rectTransform = GetComponent<RectTransform>();
        UnityEngine.Assertions.Assert.IsNotNull(rectTransform, "ERROR: A RectTransform Component could not be found by MonumentsHealthBar in gameObject '" + gameObject.name + "'");

        initialScale = rectTransform.localScale;
        initialColor = backgroundImage.color;
        refilling = false;
    }

    private void Update()
    {
        if (refilling)
            RefillAnimation();

        if (flashing)
            FlashAnimation();
    }
    #endregion

    #region Public Methods
    public void SetHealthBarTitle(string title)
    {
        barText.text = title;
    }

    public void SetHealthBarFill(float normalizedFill)
    {
        if (normalizedFill < 0 || normalizedFill > 1)
        {
            Debug.LogWarning("WARNING: MonumentsHealthBar:SetHealthBarFill called with a parameter outside of the range [0, 1]. Input value will be clamped to said range.");
            normalizedFill = Mathf.Clamp01(normalizedFill);
        }

        currentFill = normalizedFill;

        if (!refilling)
            healthImage.fillAmount = normalizedFill;

        if (normalizedFill < flashThreshold && normalizedFill > 0)
            StartFlashAnimation();
        else
            RequestFlashAnimationStop();
    }

    public void RefillHealthBar(bool animate = true)
    {
        RequestFlashAnimationStop();
        currentFill = 1.0f;
        if (animate)
        {
            StartRefillAnimation();
        }
        else
            healthImage.fillAmount = currentFill;
    }
    #endregion

    #region Private Methods
    private void StartRefillAnimation()
    {
        refilling = true;
        refillElapsedTime = 0.0f;
    }

    private void RefillAnimation()
    {
        refillElapsedTime += Time.deltaTime;

        float progress = refillElapsedTime / refillAnimationDuration;
        progress = Mathf.Clamp01(progress);
        healthImage.fillAmount = progress * currentFill;

        if (refillElapsedTime >= refillAnimationDuration)
            refilling = false;
    }

    private void StartFlashAnimation()
    {
        strongFlashTimeLeft = strongFlashDuration;
        if (currentFlashInfo == null)
            currentFlashInfo = strongFlashInfo;

        if (!flashing)
        {
            flashing = true;
            flashElapsedTime = 0.0f;
        }
        else
        {
            shouldStopFlashing = false;
        }
    }

    private void FlashAnimation()
    {
        if (strongFlashTimeLeft > 0)
            strongFlashTimeLeft -= Time.deltaTime;

        flashElapsedTime += Time.deltaTime;
        float progress = flashElapsedTime / currentFlashInfo.flashCycleDuration;

        bool stopFlashing = false;
        if (flashElapsedTime >= currentFlashInfo.flashCycleDuration)
        {
            // Cycle end
            flashElapsedTime -= currentFlashInfo.flashCycleDuration;
            currentFlashInfo = GetCurrentFlashInfo();
            progress = flashElapsedTime / currentFlashInfo.flashCycleDuration;

            if (shouldStopFlashing)
            {
                stopFlashing = true;
                progress = 0;
            }
        }

        UpdateScale(progress);
        UpdateTint(progress);

        if (stopFlashing)
            StopFlashAnimation();
    }

    private void StopFlashAnimation()
    {
        if (flashing)
        {
            shouldStopFlashing = false;
            flashing = false;
            currentFlashInfo = null;
        }
    }

    private void RequestFlashAnimationStop()
    {
        if (flashing)
            shouldStopFlashing = true;
    }

    private FlashInfo GetCurrentFlashInfo()
    {
        FlashInfo flashInfo;
        if (strongFlashTimeLeft > 0)
            flashInfo = strongFlashInfo;
        else
            flashInfo = weakFlashInfo;

        return flashInfo;
    }

    private void UpdateScale(float cycleProgress)
    {
        // The cycle is split in the 1st half (grow), the 2nd half (shrink)
        float scaleFactor;
        float localProgress;
        if (cycleProgress < 0.5f)
        {
            // Grow
            localProgress = cycleProgress / 0.5f;
            scaleFactor = Interpolate(1.0f, currentFlashInfo.scaleFactor, localProgress);
        }
        else
        {
            // Shrink
            localProgress = (cycleProgress - 0.5f) / 0.5f;
            scaleFactor = Interpolate(currentFlashInfo.scaleFactor, 1.0f, localProgress);
        }

        rectTransform.localScale = initialScale * scaleFactor;
    }

    private void UpdateTint(float cycleProgress)
    {
        // The cycle is split in 1st half (tint gain), the 2nd half (tint loss)
        float tintFactor;
        float localProgress;
        if (cycleProgress < 0.5f)
        {
            // Tint gain
            localProgress = cycleProgress / 0.5f;
            tintFactor = Interpolate(0.0f, 1.0f, localProgress);
        }
        else
        {
            // Tint loss
            localProgress = (cycleProgress - 0.5f) / 0.5f;
            tintFactor = Interpolate(1.0f, 0.0f, localProgress);
        }

        backgroundImage.color = initialColor + (tintFactor * currentFlashInfo.flashTintColor);
    }

    private float Interpolate(float start, float end, float u)
    {
        return start * (1 - u) + end * u;
    }
    #endregion
}
