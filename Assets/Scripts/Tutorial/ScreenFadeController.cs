using UnityEngine;
using UnityEngine.UI;

public delegate void FadeCallback();

public class ScreenFadeController : MonoBehaviour
{
    #region Fields
    public float defaultFadeDuration;
    public Color opaqueColor;

    private float currentFadeDuration;
    private Image image;
    private FadeCallback endCallback;
    private bool fading;
    private float elapsedTime;

    private float startAlpha;
    private float endAlpha;
    #endregion

    #region MonoBehaviourMethods
    private void Awake()
    {
        image = GetComponent<Image>();
        UnityEngine.Assertions.Assert.IsNotNull(image, "ERROR: An Image Component could not be found by FadeToBlack in GameObject " + gameObject.name);
    }

    private void Update()
    {
        if (fading)
            Fade();
    }
    #endregion

    #region Public Methods
    public void FadeToOpaque(FadeCallback endCallback = null)
    {
        FadeToOpaque(defaultFadeDuration, endCallback);
    }

    public void FadeToOpaque(float fadeDuration, FadeCallback endCallback = null)
    {
        if (!fading)
        {
            currentFadeDuration = fadeDuration;
            this.endCallback = endCallback;
            fading = true;
            elapsedTime = 0;

            startAlpha = 0;
            endAlpha = opaqueColor.a;
        }
    }

    public void FadeToTransparent(FadeCallback endCallback = null)
    {
        FadeToTransparent(defaultFadeDuration, endCallback);
    }

    public void FadeToTransparent(float fadeDuration, FadeCallback endCallback = null)
    {
        if (!fading)
        {
            currentFadeDuration = fadeDuration;
            this.endCallback = endCallback;
            fading = true;
            elapsedTime = 0;

            startAlpha = opaqueColor.a;
            endAlpha = 0;
        }
    }

    public void FadeToAlpha(float alpha, FadeCallback fadeCallback = null)
    {
        FadeToAlpha(alpha, defaultFadeDuration, endCallback);
    }

    public void FadeToAlpha(float alpha, float fadeDuration, FadeCallback endCallback = null)
    {
        if (!fading)
        {
            currentFadeDuration = fadeDuration;
            this.endCallback = endCallback;
            fading = true;
            elapsedTime = 0;

            startAlpha = image.color.a;
            endAlpha = alpha;
        }
    }

    public void TurnOpaque()
    {
        image.color = opaqueColor;
    }

    public void TurnTransparent()
    {
        Color targetColor = opaqueColor;
        targetColor.a = 0;
        image.color = targetColor;
    }
    #endregion

    #region Private Methods
    private void Fade()
    {
        elapsedTime += Time.deltaTime;
        float u = elapsedTime / currentFadeDuration;
        
        if (u < 1)
        {
            float targetAlpha = startAlpha * (1 - u) + endAlpha * u;
            Color targetColor = opaqueColor;
            targetColor.a = targetAlpha;
            image.color = targetColor;
        }
        else
        {
            Color targetColor = opaqueColor;
            targetColor.a = endAlpha;
            image.color = targetColor;

            fading = false;
            if (endCallback != null)
            {
                endCallback();
            }
        }
    }
    #endregion
}
