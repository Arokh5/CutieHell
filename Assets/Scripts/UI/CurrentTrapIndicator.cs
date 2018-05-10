using UnityEngine;
using UnityEngine.UI;

public class CurrentTrapIndicator : MonoBehaviour {

    #region Fields
    [SerializeField]
    private Image fillImage;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        UnityEngine.Assertions.Assert.IsNotNull(fillImage, "ERROR: TrapIndicator in gameObject '" + gameObject.name + "' doesn't have fillImage assigned!");
    }
    #endregion

    #region Public Methods
    public void SetFill(float normalizedFill)
    {
        fillImage.fillAmount = normalizedFill;
    }
    #endregion
}
