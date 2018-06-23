using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicStripes : MonoBehaviour
{
    #region Fields
    [Tooltip("The time (in seconds) that it takes by default to show or hide the stripes.")]
    public float defaultTime = 1.0f;
    [Tooltip("The height that the stripes should occupy when fully shown.")]
    public float targetHeight = 150.0f;
    [SerializeField]
    private RectTransform topStripe;
    [SerializeField]
    private RectTransform bottomStripe;

    private bool showing = false;
    private bool hiding = false;
    private float animationTime = 0.0f;
    private float elapsedTime = 0.0f;
    private float currentHeight = 0.0f;
    #endregion

    #region Properties
    private bool animating
    {
        get
        {
            return showing || hiding;
        }
    }
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        UnityEngine.Assertions.Assert.IsNotNull(topStripe, "ERROR: The CinematicStripes in gameObject '" + gameObject.name + "' doesn't have a RectTransform (topStripe) assigned!");
        UnityEngine.Assertions.Assert.IsNotNull(bottomStripe, "ERROR: The CinematicStripes in gameObject '" + gameObject.name + "' doesn't have a RectTransform (bottomStripe) assigned!");
    }

    private void Start()
    {
        currentHeight = 0.0f;
        SetHeights(currentHeight);
    }

    private void Update()
    {
        if (animating)
        {
            elapsedTime += Time.deltaTime;

            float u = elapsedTime / animationTime;
            if (u > 1.0f)
            {
                u = 1.0f;
            }

            float newHeight;
            if (showing)
            {
                newHeight = u * targetHeight;
                if (currentHeight < newHeight)
                    SetHeights(newHeight);
            }

            if (hiding)
            {
                newHeight = (1 - u) * targetHeight;
                if (currentHeight > newHeight)
                    SetHeights(newHeight);
            }

            if (elapsedTime >= animationTime)
            {
                showing = false;
                hiding = false;
            }
        }
    }
    #endregion

    #region Public Methods
    public bool ShowAnimated(float duration = -1.0f)
    {
        if (showing)
            return false;

        if (duration == 0)
        {
            Show();
            return true;
        }

        animationTime = duration < 0.0f ? defaultTime : duration;
        elapsedTime = 0.0f;
        hiding = false;
        showing = true;

        return true;
    }

    public bool HideAnimated(float duration = -1.0f)
    {
        if (hiding)
            return false;

        if (duration == 0)
        {
            Hide();
            return true;
        }

        animationTime = duration < 0.0f ? defaultTime : duration;
        elapsedTime = 0.0f;
        showing = false;
        hiding = true;

        return true;
    }

    public void Show()
    {
        showing = false;
        hiding = false;
        SetHeights(targetHeight);
    }

    public void Hide()
    {
        showing = false;
        hiding = false;
        SetHeights(0.0f);
    }
    #endregion

    #region Private Methods
    private void SetHeights(float height)
    {
        currentHeight = height;
        SetHeight(topStripe, currentHeight);
        SetHeight(bottomStripe, currentHeight);
    }

    private void SetHeight(RectTransform rectTransform, float height)
    {
        Vector2 size = rectTransform.sizeDelta;
        size.y = height;
        rectTransform.sizeDelta = size;
    }
    #endregion

}
