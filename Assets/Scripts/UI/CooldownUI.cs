using UnityEngine;
using UnityEngine.UI;

public class CooldownUI : MonoBehaviour {

    #region Fields
    [Header("Cooldown configuration")]
    [SerializeField]
    private int fontSize;
    [SerializeField]
    private Sprite sprite;
    [SerializeField]
    private float defaultFlashDuration = 0.2f;
    [SerializeField]
    private float flashScale = 2.0f;

    private RectTransform rectTransform;
    private float initialScale;
    private float currentScale;
    private float flashDuration;
    private float flashElapsedTime;
    private bool flashing = false;

    [Header("Prefab setup")]
    [SerializeField]
    private Image background;
    [SerializeField]
    private Image foreground;
    [SerializeField]
    private Text numberText;

    private int currentNumber;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        UnityEngine.Assertions.Assert.IsNotNull(background, "ERROR: Background (Image) not assigned for CooldownUI script in GameObject " + gameObject.name);
        UnityEngine.Assertions.Assert.IsNotNull(foreground, "ERROR: Foreground (Image) not assigned for CooldownUI script in GameObject " + gameObject.name);
        UnityEngine.Assertions.Assert.IsNotNull(numberText, "ERROR: Number Text (Text) not assigned for CooldownUI script in GameObject " + gameObject.name);
        UnityEngine.Assertions.Assert.IsNotNull(sprite, "ERROR: Sprite (Sprite) not assigned for CooldownUI script in GameObject " + gameObject.name);

        rectTransform = GetComponent<RectTransform>();
        UnityEngine.Assertions.Assert.IsNotNull(rectTransform, "ERROR: A RectTransform could not be found by CooldownUI script in GameObject " + gameObject.name);
        if (rectTransform.localScale.x != rectTransform.localScale.y
            || rectTransform.localScale.x != rectTransform.localScale.z
            || rectTransform.localScale.y != rectTransform.localScale.z)
        {
            Debug.Log("INFO: The RectTransform's local scale in GameObject " + gameObject.name + " is not uniform. Scale will be readjusted based on the x component.");
            rectTransform.localScale = rectTransform.localScale.x * Vector3.one;
        }
        initialScale = rectTransform.localScale.x;

        CooldownOver();
    }

    private void Update()
    {
        if (flashing)
        {
            FlashAnimation();
        }
    }

    private void OnValidate()
    {
        if (fontSize < 0)
            fontSize = 0;
        if (numberText)
            numberText.fontSize = fontSize;
        if (background)
            background.sprite = sprite;
        if (foreground)
            foreground.sprite = sprite;
    }
    #endregion

    #region Public Methods
    public void SetCountdownLeft(float time, float maxCooldown)
    {
        if (time <= 0)
        {
            currentNumber = 0;
            CooldownOver();
        }
        else
        {
            foreground.fillAmount = (maxCooldown - time) / maxCooldown;

            // Avoid construction of String when not necessary
            int newNumber = Mathf.CeilToInt(time);
            if (newNumber != currentNumber)
            {
                currentNumber = newNumber;
                numberText.text = currentNumber.ToString();
            }
        }
    }

    public void Flash()
    {
        Flash(defaultFlashDuration);
    }

    public void Flash(float flashDuration)
    {
        this.flashDuration = flashDuration;
        flashing = true;
        flashElapsedTime = 0;
    }
    #endregion

    #region Private Methods
    private void CooldownOver()
    {
        numberText.text = "";
        foreground.fillAmount = 1;
    }

    private void FlashAnimation()
    {
        flashElapsedTime += Time.deltaTime;
        float u = flashElapsedTime / flashDuration;
        if (u >= 1)
        {
            u = 1;
            flashing = false;
        }

        currentScale = (1 - u) * flashScale + u * 1.0f;
        rectTransform.localScale = currentScale * Vector3.one;
    }
    #endregion
}
