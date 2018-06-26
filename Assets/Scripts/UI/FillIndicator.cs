using UnityEngine;
using UnityEngine.UI;

public class FillIndicator : MonoBehaviour
{
    #region Fields
    private RawImage rawImage;
    private Material material;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        rawImage = GetComponent<RawImage>();
        UnityEngine.Assertions.Assert.IsNotNull(rawImage, "ERROR: A RawImage Component could not be found by FillIndicator in GameObject " + gameObject.name);
        material = new Material(rawImage.material);
        rawImage.material = material;
    }
    #endregion

    #region Public Methods
    public void SetFill(float normalizedFill)
    {
        material.SetFloat("_Fill", normalizedFill);
    }
    #endregion
}
