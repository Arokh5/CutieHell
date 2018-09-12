using UnityEngine;
using UnityEngine.UI;

public class FollowUpButtonPrompt : MonoBehaviour
{
    [System.Serializable]
    private class GraphicColorInfo
    {
        public Graphic graphic;
        public Color initialColor;

        public GraphicColorInfo(Graphic g, Color c)
        {
            graphic = g;
            initialColor = c;
        }
    }

    private enum AnimationState
    {
        OFF,
        HIDDEN,
        PRE_ALERT,
        MASHABLE,
        FAIL
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
    private float coverStartScale = 2.0f;
    [SerializeField]
    private float maskingEndScale = 2.0f;
    [SerializeField]
    private float failDuration = 0.5f;
    [SerializeField]
    private Color failTargetColor = Color.red;

    private FollowUpPromptInfo currentInfo;
    private float maskMaxOffset;
    private float elapsedTime;
    private float failElapsedTime;
    private AnimationState animState;
    private RectTransform rectTransform;
    private GraphicColorInfo[] graphicInfos;
    #endregion

    #region MonoBehaviour
    private void Awake()
    {
        UnityEngine.Assertions.Assert.IsNotNull(baseButton, "ERROR: Base Button (Image) not assigned for FollowUpButtonPrompt script in GameObject " + gameObject.name);
        UnityEngine.Assertions.Assert.IsNotNull(maskTransform, "ERROR: Mask Transform (RectTransform) not assigned for FollowUpButtonPrompt script in GameObject " + gameObject.name);
        UnityEngine.Assertions.Assert.IsNotNull(maskImage, "ERROR: Mask Image (Image) not assigned for FollowUpButtonPrompt script in GameObject " + gameObject.name);
        UnityEngine.Assertions.Assert.IsNotNull(preAlertCover, "ERROR: Pre Alert Cover (RectTransform) not assigned for FollowUpButtonPrompt script in GameObject " + gameObject.name);
        rectTransform = GetComponent<RectTransform>();
        SetupGraphicInfos();
        ResetElements();
        maskMaxOffset = 0.5f * maskTransform.rect.width;
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
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
                        animState = AnimationState.FAIL;
                    }
                }
                break;
            case AnimationState.FAIL:
                {
                    failElapsedTime += Time.deltaTime;
                    float normalizedPhaseProgress = failElapsedTime / failDuration;
                    FailAnimation(normalizedPhaseProgress);
                    if (failElapsedTime > failDuration)
                    {
                        Deactivate();
                    }
                }
                break;
        }
    }
    #endregion

    #region Public Methods
    public void RequestShow(FollowUpPromptInfo fupInfo)
    {
        gameObject.SetActive(true);
        if (!IsCurrentInfo(fupInfo) || animState == AnimationState.FAIL)
        {
            SetupElements(fupInfo);
            elapsedTime += Time.deltaTime; // Done to be in sync with the AttackChainsManager
            animState = AnimationState.HIDDEN;
        }
    }

    public void RequestDeactivate()
    {
        switch (animState)
        {
            case AnimationState.OFF:
            case AnimationState.FAIL:
                // Do nothing
                break;
            case AnimationState.HIDDEN:
                Deactivate();
                break;
            case AnimationState.MASHABLE:
                if (elapsedTime + Time.deltaTime < currentInfo.timingInfo.end)
                    Deactivate();
                break;
            case AnimationState.PRE_ALERT:
                currentInfo = new FollowUpPromptInfo();
                animState = AnimationState.FAIL;
                break;
        }
    }
    #endregion

    #region Private Methods
    private void Deactivate()
    {
        if (!rectTransform)
            rectTransform = GetComponent<RectTransform>();

        currentInfo = new FollowUpPromptInfo();
        ResetElements();
        gameObject.SetActive(false);
    }

    private void SetupGraphicInfos()
    {
        Graphic[] graphics = GetComponentsInChildren<Graphic>();
        graphicInfos = new GraphicColorInfo[graphics.Length];
        for (int i = 0; i < graphicInfos.Length; ++i)
        {
            graphicInfos[i] = new GraphicColorInfo(graphics[i], graphics[i].color);
        }
    }

    private void SetColorFactor(Color colorFactor)
    {
        foreach (GraphicColorInfo info in graphicInfos)
        {
            Color c = info.initialColor;
            c *= colorFactor;
            info.graphic.color = c;
        }
    }

    private bool IsCurrentInfo(FollowUpPromptInfo newFupInfo)
    {
        return newFupInfo.sprite == currentInfo.sprite
            && newFupInfo.timingInfo.alert == currentInfo.timingInfo.alert
            && newFupInfo.timingInfo.start == currentInfo.timingInfo.start
            && newFupInfo.timingInfo.end == currentInfo.timingInfo.end;
    }

    private void SetupElements(FollowUpPromptInfo newFupInfo)
    {
        ResetElements();
        currentInfo = newFupInfo;
        baseButton.sprite = currentInfo.sprite;
        maskImage.sprite = currentInfo.sprite;
    }

    private void ResetElements()
    {
        elapsedTime = 0.0f;
        failElapsedTime = 0.0f;
        animState = AnimationState.OFF;
        baseButton.gameObject.SetActive(false);
        maskTransform.gameObject.SetActive(false);
        preAlertCover.gameObject.SetActive(false);
        rectTransform.localScale = Vector2.one;
        SetMaskTransformProgress(0.0f);
        SetPreAlertProgress(0.0f);
        SetColorFactor(Color.white);
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
        rectTransform.localScale = (1.0f + (maskingEndScale - 1.0f) * Mathf.Sin(0.5f * Mathf.PI * normalizedTime)) * Vector2.one;
        SetMaskTransformProgress(normalizedTime);
    }

    private void FailAnimation(float normalizedTime)
    {
        float u = Mathf.Sin(0.5f * Mathf.PI * normalizedTime);
        Color targetColorFactor = (1.0f - u) * Color.white + u * failTargetColor;
        SetColorFactor(targetColorFactor);
    }
    #endregion
}
