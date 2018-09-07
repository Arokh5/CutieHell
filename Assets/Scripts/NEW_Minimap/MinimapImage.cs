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
    private void Awake()
    {
        image = GetComponent<Image>();
        UnityEngine.Assertions.Assert.IsNotNull(image, "ERROR: An Image Component could not be found by MinimapImage in GameObject " + gameObject.name);
    }
    #endregion

    #region Public Methods
    public void SetupMinimapImage(MinimapElement mmElement)
    {
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
        image.enabled = true;
    }

    public void Hide()
    {
        image.enabled = false;
    }
    #endregion
}
