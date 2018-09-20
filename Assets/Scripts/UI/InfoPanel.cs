using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour
{
    public delegate void VoidCallback();

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
    [SerializeField]
    [ShowOnly]
    private bool inShownState;

    private GraphicInfo[] graphicInfos;
    
    private bool showFlag = false;
    private bool hideFlag = false;
    private bool showHideFlag = false;

    private float elapsedTime;
    private float blendDuration;
    private float displayDuration;

    private VoidCallback endCallback = null;
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
    #endregion

    #region Public Methods
    public void Show()
    {
        ResetAnimationFlags();
        SetAlphaFactor(1.0f);
        inShownState = true;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        SetAlphaFactor(0.0f);
        inShownState = false;
        ResetAnimationFlags();
    }

    public bool ShowAnimated(float _blendDuration, VoidCallback _endCallback = null)
    {
        return SetupAnimation(_blendDuration, 0.0f, true, false, _endCallback);
    }

    public bool HideAnimated(float _blendDuration, VoidCallback _endCallback = null)
    {
        return SetupAnimation(_blendDuration, 0.0f, false, true, _endCallback);
    }

    public bool ShowHideAnimated(float _blendDuration, float _displayDuration, VoidCallback _endCallback = null)
    {
        return SetupAnimation(_blendDuration, _displayDuration, true, true, _endCallback);
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

    private bool SetupAnimation(float _blendDuration, float _displayDuration, bool show, bool hide, VoidCallback _endCallback)
    {
        bool success = false;

        if (!animating && !(!show && !hide))
        {
            endCallback = _endCallback;
            if ((show && hide) && !inShownState)
                showHideFlag = true;
            else if (show && !inShownState)
                showFlag = true;
            else if (hide && inShownState)
                hideFlag = true;

            if (animating)
            {
                success = true;
                gameObject.SetActive(true);
                elapsedTime = 0.0f;
                blendDuration = _blendDuration;
                displayDuration = _displayDuration;
            }
        }

        return success;
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
            inShownState = true;
            elapsedTime += Time.deltaTime;
            float alphaFactor = elapsedTime / blendDuration;
            if (alphaFactor > 1.0f)
                alphaFactor = 1.0f;
            SetAlphaFactor(alphaFactor);
        }
        else
        {
            showFlag = false;
            LaunchCallback();
        }
    }

    private void HideAnimation()
    {
        if (elapsedTime < blendDuration)
        {
            inShownState = false;
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
            LaunchCallback();
        }
    }

    private void ShowHideAnimation()
    {
        float alphaFactor;
        elapsedTime += Time.deltaTime;

        if (elapsedTime < blendDuration)
        {
            inShownState = true;
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
            inShownState = false;
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
            LaunchCallback();
        }
        SetAlphaFactor(alphaFactor);
    }

    private void LaunchCallback()
    {
        if (endCallback != null)
        {
            VoidCallback callback = endCallback;
            endCallback = null;
            callback();
        }
    }
    #endregion
}
