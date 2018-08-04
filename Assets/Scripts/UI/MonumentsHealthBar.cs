using UnityEngine;
using UnityEngine.UI;

public class MonumentsHealthBar : MonoBehaviour
{
    #region Fields
    [Header("Element setup")]
    [SerializeField]
    private Image healthImage;
    [SerializeField]
    private Text barText;

    [Header("Refill animation setup")]
    [SerializeField]
    private float refillAnimationDuration = 1.0f;

    private float currentFill = 1.0f;

    private bool animateRefill = true;
    private float refillElapsedTime;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        UnityEngine.Assertions.Assert.IsNotNull(healthImage, "ERROR: Health Image (Image) not assigned for MonumentsHealthBar in gameObject '" + gameObject.name + "'");
        UnityEngine.Assertions.Assert.IsNotNull(barText, "ERROR: Bar Text (Text) not assigned for MonumentsHealthBar in gameObject '" + gameObject.name + "'");
    }

    private void Update()
    {
        if (animateRefill)
            RefillAnimation();

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

        if (!animateRefill)
            healthImage.fillAmount = normalizedFill;
    }

    public void RefillHealthBar(bool animate = true)
    {
        currentFill = 1.0f;
        if (animate)
        {
            animateRefill = true;
            refillElapsedTime = 0.0f;
        }
        else
            healthImage.fillAmount = currentFill;
    }
    #endregion

    #region Private Methods
    private void RefillAnimation()
    {
        refillElapsedTime += Time.deltaTime;

        float progress = refillElapsedTime / refillAnimationDuration;
        progress = Mathf.Clamp01(progress);
        healthImage.fillAmount = progress * currentFill;

        if (refillElapsedTime >= refillAnimationDuration)
            animateRefill = false;
    }
    #endregion
}
