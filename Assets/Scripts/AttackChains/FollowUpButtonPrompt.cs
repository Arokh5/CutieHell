using UnityEngine;
using UnityEngine.UI;

public class FollowUpButtonPrompt : MonoBehaviour
{
    private enum AnimationState
    {
        HIDDEN,
        PRE_ALERT,
        MASHABLE
    }

    #region Fields
    [Header("Elements setup")]
    [SerializeField]
    private Image baseButton;
    [SerializeField]
    private RectTransform maskTransform;
    [SerializeField]
    private Image maskImage;
    [SerializeField]
    private RectTransform preAlertCover;

    [Header("Effect configuration")]
    [SerializeField]
    public float coverStartScale = 2.0f;

    private FollowUpPromptInfo currentInfo;
    private float maskMaxOffset;
    private float elapsedTime;
    private AnimationState animState;
    private RectTransform rectTransform;

    [Range(0.0f, 1.0f)]
    public float maskTest = 0;
    [Range(0.0f, 1.0f)]
    public float preAlertTest = 0;
    #endregion

    #region MonoBehaviour
    private void Awake()
    {
        UnityEngine.Assertions.Assert.IsNotNull(baseButton, "ERROR: Base Button (Image) not assigned for FollowUpButtonPrompt script in GameObject " + gameObject.name);
        UnityEngine.Assertions.Assert.IsNotNull(maskTransform, "ERROR: Mask Transform (RectTransform) not assigned for FollowUpButtonPrompt script in GameObject " + gameObject.name);
        UnityEngine.Assertions.Assert.IsNotNull(maskImage, "ERROR: Mask Image (Image) not assigned for FollowUpButtonPrompt script in GameObject " + gameObject.name);
        UnityEngine.Assertions.Assert.IsNotNull(preAlertCover, "ERROR: Pre Alert Cover (RectTransform) not assigned for FollowUpButtonPrompt script in GameObject " + gameObject.name);
        rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        maskMaxOffset = 0.5f * maskTransform.rect.width;
        ResetElements();
    }

    private void Update()
    {
        switch (animState)
        {
            case AnimationState.HIDDEN:
                if (elapsedTime >= currentInfo.timingInfo.alert)
                {
                    animState = AnimationState.PRE_ALERT;
                    maskTransform.gameObject.SetActive(true);
                    preAlertCover.gameObject.SetActive(true);
                }
                break;
            case AnimationState.PRE_ALERT:
                {
                    float normalizedPhaseProgress = (elapsedTime - currentInfo.timingInfo.alert) / (currentInfo.timingInfo.start - currentInfo.timingInfo.alert);
                    PreAlertAnimation(normalizedPhaseProgress);
                    if (elapsedTime >= currentInfo.timingInfo.start)
                    {
                        animState = AnimationState.MASHABLE;
                        baseButton.gameObject.SetActive(true);
                        preAlertCover.gameObject.SetActive(false);
                    }
                }
                break;
            case AnimationState.MASHABLE:
                {
                    float normalizedPhaseProgress = (elapsedTime - currentInfo.timingInfo.start) / (currentInfo.timingInfo.end - currentInfo.timingInfo.start);
                    MashingAnimation(normalizedPhaseProgress);
                    if (elapsedTime >= currentInfo.timingInfo.end)
                    {
                        Deactivate();
                    }
                }
                break;
        }

        elapsedTime += Time.deltaTime;
    }

    private void OnValidate()
    {
        if (maskMaxOffset == 0)
            maskMaxOffset = 0.5f * maskTransform.rect.width;
        SetMaskTransformProgress(maskTest);
        SetPreAlertProgress(preAlertTest);
    }
    #endregion

    #region Public Methods
    public void RequestShow(FollowUpPromptInfo fupInfo)
    {
        gameObject.SetActive(true);
        if (!IsCurrentInfo(fupInfo))
        {
            SetupElements(fupInfo);
        }
    }

    public void Deactivate()
    {
        if (!rectTransform)
            rectTransform = GetComponent<RectTransform>();

        currentInfo = new FollowUpPromptInfo();
        ResetElements();
        gameObject.SetActive(false);
    }
    #endregion

    #region Private Methods
    private bool IsCurrentInfo(FollowUpPromptInfo newFupInfo)
    {
        return newFupInfo.sprite == currentInfo.sprite
            && newFupInfo.timingInfo.alert == currentInfo.timingInfo.alert
            && newFupInfo.timingInfo.start == currentInfo.timingInfo.start
            && newFupInfo.timingInfo.end == currentInfo.timingInfo.end;
    }

    private void SetupElements(FollowUpPromptInfo newFupInfo)
    {
        currentInfo = newFupInfo;
        ResetElements();
        baseButton.sprite = currentInfo.sprite;
        maskImage.sprite = currentInfo.sprite;
    }

    private void ResetElements()
    {
        elapsedTime = 0;
        animState = AnimationState.HIDDEN;
        baseButton.gameObject.SetActive(false);
        maskTransform.gameObject.SetActive(false);
        preAlertCover.gameObject.SetActive(false);
        rectTransform.localScale = Vector2.one;
        SetMaskTransformProgress(0.0f);

        SetPreAlertProgress(0.0f);
    }

    private void SetMaskTransformProgress(float normalizedProgress)
    {
        float offset = maskMaxOffset * normalizedProgress;
        maskTransform.offsetMin = new Vector2(offset, offset);
        maskTransform.offsetMax = new Vector2(-offset, -offset);
    }

    private void SetPreAlertProgress(float normalizedProgress)
    {
        float scaleFactor = (1.0f - normalizedProgress) * coverStartScale + normalizedProgress * 1.0f;
        preAlertCover.localScale = scaleFactor * Vector2.one;
    }

    private void PreAlertAnimation(float normalizedTime)
    {
        SetPreAlertProgress(normalizedTime);
    }

    private void MashingAnimation(float normalizedTime)
    {
        rectTransform.localScale = (1.0f + Mathf.Sin(0.5f * Mathf.PI * normalizedTime)) * Vector2.one;
        SetMaskTransformProgress(normalizedTime);
    }
    #endregion
}
