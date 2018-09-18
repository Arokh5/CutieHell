using UnityEngine;
using UnityEngine.UI;

public class MinimapImage : MonoBehaviour
{
    #region Fields
    private Image image;
    #endregion

    #region Properties
    public Vector2 localPosition
    {
        get
        {
            return image.rectTransform.localPosition;
        }
        set
        {
            image.rectTransform.localPosition = value;
        }
    }

    public float rotationCW
    {
        get
        {
            return 360 - image.rectTransform.localRotation.eulerAngles.z;
        }
        set
        {
            Vector3 eulerAngles = image.rectTransform.localRotation.eulerAngles;
            eulerAngles.z = 360 - value;
            image.rectTransform.localRotation = Quaternion.Euler(eulerAngles);
        }
    }

    public float rotationCCW
    {
        get
        {
            return image.rectTransform.localRotation.eulerAngles.z;
        }
        set
        {
            Vector3 eulerAngles = image.rectTransform.localRotation.eulerAngles;
            eulerAngles.z = value;
            image.rectTransform.localRotation = Quaternion.Euler(eulerAngles);
        }
    }

    public float width
    {
        get
        {
            return image.rectTransform.sizeDelta.x;
        }
    }

    public float height
    {
        get
        {
            return image.rectTransform.sizeDelta.y;
        }
    }
    #endregion

    #region MonoBehaviour
    protected void Awake()
    {
        image = GetComponent<Image>();
        UnityEngine.Assertions.Assert.IsNotNull(image, "ERROR: An Image Component could not be found by MinimapImage in GameObject " + gameObject.name);
    }
    #endregion

    #region Public Methods
    public void SetupMinimapImage(MinimapElement mmElement)
    {
        if (!image)
        {
            image = GetComponent<Image>();
        }
        image.sprite = mmElement.sprite;
        image.color = mmElement.color;
        image.rectTransform.sizeDelta = new Vector2(mmElement.size, mmElement.size);
        gameObject.name = "MM_" + mmElement.gameObject.name;
    }

    public void CleanUp()
    {
        image.sprite = null;
        image.color = Color.white;
        image.rectTransform.sizeDelta = new Vector2(0.0f, 0.0f);
        gameObject.name = "MinimapImagePrefab";
    }

    public void Show()
    {
        if (!image.enabled)
        {
            OnImageShown();
        }
        image.enabled = true;
    }

    public void Hide()
    {
        if (image.enabled)
        {
            OnImageHidden();
        }
        image.enabled = false;
    }

    public virtual void RequestEffect() { }
    #endregion

    #region Protected Methods
    protected virtual void OnImageShown() { }
    protected virtual void OnImageHidden() { }
    #endregion
}
