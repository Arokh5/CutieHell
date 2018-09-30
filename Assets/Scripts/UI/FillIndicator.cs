using UnityEngine;
using UnityEngine.UI;

public class FillIndicator : MonoBehaviour
{
    #region Fields
    private Image image;
    private Material material;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        image = GetComponent<Image>();
        UnityEngine.Assertions.Assert.IsNotNull(image, "ERROR: An Image Component could not be found by FillIndicator in GameObject " + gameObject.name);
    }
    #endregion

    #region Public Methods
    public void SetFill(float normalizedFill)
    {
        if (image)
            image.fillAmount = normalizedFill;
    }
    #endregion
}
