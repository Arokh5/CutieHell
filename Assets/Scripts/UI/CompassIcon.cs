using UnityEngine;
using UnityEngine.UI;

public class CompassIcon : MonoBehaviour
{
    #region Fields
    public float alertDuration = 2.0f;
    [Range(1.0f, 5.0f)]
    public float alertFrequency = 2.0f;
    [Range(0.0f, 0.5f)]
    public float blurGrowthPercent = 0.2f;
    [Range(0.0f, 0.5f)]
    public float imageShrinkPercent = 0.2f;

    [HideInInspector]
    public float alertTimeLeft = 0;

    private RectTransform rectTransform;
    private Image blurImage;
    [SerializeField]
    private Image backgroundImage;
    [SerializeField]
    private Image fillImage;
    [SerializeField]
    private Image logoImage;
    [SerializeField]
    private RectTransform frontRectTransform;
    private Vector2 referenceSize;
    #endregion

    #region Properties
    public Vector3 localPosition
    {
        get
        {
            return rectTransform.localPosition;
        }
        set
        {
            rectTransform.localPosition = value;
        }
    }
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        if (!rectTransform)
            rectTransform = GetComponent<RectTransform>();

        if (!blurImage)
            blurImage = GetComponent<Image>();

        UnityEngine.Assertions.Assert.IsNotNull(backgroundImage, "ERROR: CompassIcon in GameObject '" + gameObject.name + "' doesn't have a backgroundImage assigned!");
        UnityEngine.Assertions.Assert.IsNotNull(fillImage, "ERROR: CompassIcon in GameObject '" + gameObject.name + "' doesn't have a fillImage assigned!");
        UnityEngine.Assertions.Assert.IsNotNull(logoImage, "ERROR: CompassIcon in GameObject '" + gameObject.name + "' doesn't have a logoImage assigned!");
        UnityEngine.Assertions.Assert.IsNotNull(frontRectTransform, "ERROR: CompassIcon in GameObject '" + gameObject.name + "' doesn't have a frontRectTransform assigned!");

        referenceSize = rectTransform.sizeDelta;
        TurnOff();
    }

    private void Update()
    {
        if (alertTimeLeft > 0)
        {
            Blink();
            alertTimeLeft -= Time.deltaTime;

            if (alertTimeLeft <= 0)
                ResetAlert();
        }
    }
    #endregion

    #region Public Methods
    public void SetBackground(Sprite sprite)
    {
        blurImage.sprite = sprite;
        backgroundImage.sprite = sprite;
    }

    public void SetLogo(Sprite sprite)
    {
        logoImage.sprite = sprite;
    }

    public void SetAlert()
    {
        alertTimeLeft = alertDuration;
    }

    public void SetFill(float normalizedFill)
    {
        fillImage.fillAmount = normalizedFill;
    }

    public void TurnOn()
    {
        if (alertTimeLeft > 0)
            blurImage.enabled = true;

        frontRectTransform.gameObject.SetActive(true);
    }

    public void TurnOff()
    {
        blurImage.enabled = false;
        frontRectTransform.gameObject.SetActive(false);
    }
    #endregion

    #region Private Methods
    private void Blink()
    {
        float sinValue = Mathf.Abs(Mathf.Sin(Time.time * (alertFrequency / 2.0f) * (2.0f * Mathf.PI)));
        rectTransform.sizeDelta = new Vector2(referenceSize.x * (1 + blurGrowthPercent * sinValue), referenceSize.y * (1 + blurGrowthPercent * sinValue));
        frontRectTransform.sizeDelta = new Vector2(referenceSize.x * (1 - imageShrinkPercent * sinValue), referenceSize.y * (1 - imageShrinkPercent * sinValue));
    }

    private void ResetAlert()
    {
        alertTimeLeft = 0;
        blurImage.enabled = false;
        rectTransform.sizeDelta = referenceSize;
        frontRectTransform.sizeDelta = referenceSize;
    }
    #endregion
}
