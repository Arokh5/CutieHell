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
    private Image fixedImage;
    private RectTransform fixedImageRectTransform;
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

        if (!fixedImage)
            fixedImage = GetComponentsInChildren<Image>()[1];

        if (!fixedImageRectTransform)
            fixedImageRectTransform = GetComponentsInChildren<RectTransform>()[1];

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
    public void SetImage(Sprite sprite)
    {
        blurImage.sprite = sprite;
        fixedImage.sprite = sprite;
    }

    public void SetAlert()
    {
        alertTimeLeft = alertDuration;
    }

    public void TurnOn()
    {
        blurImage.enabled = true;
        fixedImage.enabled = true;
    }

    public void TurnOff()
    {
        blurImage.enabled = false;
        fixedImage.enabled = false;
    }
    #endregion

    #region Private Methods
    private void Blink()
    {
        float sinValue = Mathf.Abs(Mathf.Sin(Time.time * (alertFrequency / 2.0f) * (2.0f * Mathf.PI)));
        rectTransform.sizeDelta = new Vector2(referenceSize.x * (1 + blurGrowthPercent * sinValue), referenceSize.y * (1 + blurGrowthPercent * sinValue));
        fixedImageRectTransform.sizeDelta = new Vector2(referenceSize.x * (1 - imageShrinkPercent * sinValue), referenceSize.y * (1 - imageShrinkPercent * sinValue));
    }

    private void ResetAlert()
    {
        alertTimeLeft = 0;
        blurImage.enabled = false;
        rectTransform.sizeDelta = referenceSize;
        fixedImageRectTransform.sizeDelta = referenceSize;
    }
    #endregion
}
