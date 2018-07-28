using UnityEngine;
using UnityEngine.UI;

public class CooldownUI : MonoBehaviour {

    #region Fields
    [Header("Cooldown configuration")]
    [SerializeField]
    private int fontSize;
    [SerializeField]
    private Sprite sprite;

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

        CooldownOver();
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
            CooldownOver();
        }
        else
        {
            if (!gameObject.activeSelf)
                gameObject.SetActive(true);
            foreground.fillAmount = time / maxCooldown;

            // Avoid construction of String when not necessary
            int newNumber = Mathf.CeilToInt(time);
            if (newNumber != currentNumber)
            {
                currentNumber = newNumber;
                numberText.text = currentNumber.ToString();
            }
        }
    }
    #endregion

    #region Private Methods
    private void CooldownOver()
    {
        numberText.text = "";
        foreground.fillAmount = 1;
    }
    #endregion
}
