using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour
{
    [System.Serializable]
    private class GraphicInfo
    {
        public Graphic graphic;
        public float initialAlpha;

        public GraphicInfo(Graphic g, float a)
        {
            graphic = g;
            initialAlpha = a;
        }
    }

    #region Fields
    [SerializeField]
    private bool startActive;

    private GraphicInfo[] graphicInfos;
    
    [SerializeField]
    private bool showFlag = false;
    [SerializeField]
    private bool hideFlag = false;
    [SerializeField]
    private bool showHideFlag = false;
    [SerializeField]
    private float elapsedTime;
    [SerializeField]
    private float blendDuration;
    [SerializeField]
    private float displayDuration;

    [Header("Testing")]
    [Range(0.0f, 1.0f)]
    public float alpha;
    public bool update;
    public float blend = 1.0f;
    public float display = 2.0f;
    public bool show;
    public bool hide;
    public bool showHide;
    #endregion

    #region Properties
    private bool animating
    {
        get
        {
            return showFlag || hideFlag || showHideFlag;
        }
    }
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        ResetAnimationFlags();
        SetupGraphicInfos();
        if (startActive)
            Show();
        else
            Hide();

    }

    private void Update()
    {
        if (animating)
        {
            if (showFlag)
            {
                ShowAnimation();
            }

            else if (hideFlag)
            {
                HideAnimation();
            }

            else if (showHideFlag)
            {
                ShowHideAnimation();
            }
        }
    }

    private void OnValidate()
    {
        if (graphicInfos == null || update)
        {
            update = false;
            SetupGraphicInfos();
        }
        SetAlphaFactor(alpha);
        if (show)
        {
            show = false;
            ShowAnimated(blend, display);
        }
        if (hide)
        {
            hide = false;
            HideAnimated(blend, display);
        }
        if (showHide)
        {
            showHide = false;
            ShowHideAnimated(blend, display);
        }
    }
    #endregion

    #region Public Methods
    public void Show()
    {
        ResetAnimationFlags();
        SetAlphaFactor(1.0f);
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        SetAlphaFactor(0.0f);
        ResetAnimationFlags();
    }

    public bool ShowAnimated(float _blendDuration, float _displayDuration)
    {
        gameObject.SetActive(true);
        return SetupAnimation(_blendDuration, _displayDuration, true, false);
    }

    public bool HideAnimated(float _blendDuration, float _displayDuration)
    {
        gameObject.SetActive(true);
        return SetupAnimation(_blendDuration, _displayDuration, false, true);
    }

    public bool ShowHideAnimated(float _blendDuration, float _displayDuration)
    {
        gameObject.SetActive(true);
        return SetupAnimation(_blendDuration, _displayDuration, true, true);
    }
    #endregion

    #region Private Methods
    private void SetupGraphicInfos()
    {
        Graphic[] graphics = GetComponentsInChildren<Graphic>();
        graphicInfos = new GraphicInfo[graphics.Length];
        for (int i = 0; i < graphicInfos.Length; ++i)
        {
            graphicInfos[i] = new GraphicInfo(graphics[i], graphics[i].color.a);
        }
    }

    private bool SetupAnimation(float _blendDuration, float _displayDuration, bool show, bool hide)
    {
        if (animating || (!show && !hide))
        {
            Debug.Log("WILL NOT!");
            return false;
        }
        else
        {
            Debug.Log("WILL DO!");
            if (show && hide)
                showHideFlag = true;
            else if (show)
                showFlag = true;
            else if (hide)
                hideFlag = true;
            elapsedTime = 0.0f;
            blendDuration = _blendDuration;
            displayDuration = _displayDuration;
            return true;
        }
    }

    private void ResetAnimationFlags()
    {
        showFlag = false;
        hideFlag = false;
        showHideFlag = false;
    }

    private void SetAlphaFactor(float alphaFactor)
    {
        foreach (GraphicInfo info in graphicInfos)
        {
            Color c = info.graphic.color;
            c.a = alphaFactor * info.initialAlpha;
            info.graphic.color = c;
        }
    }

    private void ShowAnimation()
    {
        if (elapsedTime < blendDuration)
        {
            elapsedTime += Time.deltaTime;
            float alphaFactor = elapsedTime / blendDuration;
            if (alphaFactor > 1.0f)
                alphaFactor = 1.0f;
            SetAlphaFactor(alphaFactor);
        }
        else
        {
            showFlag = false;
        }
    }

    private void HideAnimation()
    {
        if (elapsedTime < blendDuration)
        {
            elapsedTime += Time.deltaTime;
            float alphaFactor = elapsedTime / blendDuration;
            alphaFactor = 1.0f - alphaFactor;
            if (alphaFactor < 0.0f)
                alphaFactor = 0.0f;
            SetAlphaFactor(alphaFactor);
        }
        else
        {
            hideFlag = false;
            gameObject.SetActive(false);
        }
    }

    private void ShowHideAnimation()
    {
        float alphaFactor;
        elapsedTime += Time.deltaTime;

        if (elapsedTime < blendDuration)
        {
            alphaFactor = elapsedTime / blendDuration;
            if (alphaFactor > 1.0f)
                alphaFactor = 1.0f;
        }
        else if (elapsedTime <= blendDuration + displayDuration)
        {
            // We wait...
            alphaFactor = 1.0f;
        }
        else if (elapsedTime > blendDuration + displayDuration && elapsedTime <= 2 * blendDuration + displayDuration)
        {
            float u = (elapsedTime - (blendDuration + displayDuration)) / blendDuration;
            alphaFactor = 1 - u;
            if (alphaFactor < 0.0f)
                alphaFactor = 0.0f;
        }
        else
        {
            alphaFactor = 0.0f;
            showHideFlag = false;
            gameObject.SetActive(false);
        }
        SetAlphaFactor(alphaFactor);
    }
    #endregion
}
