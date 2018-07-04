using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    #region Fields
    [SerializeField]
    private Image healthImage;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        UnityEngine.Assertions.Assert.IsNotNull(healthImage, "ERROR: healthImage not assigned for HealthBar in gameObject '" + gameObject.name + "'");
    }
    #endregion

    #region Public Methods
    public void SetHealthBarFill(float normalizedFill)
    {
        healthImage.fillAmount = normalizedFill;
    }
    #endregion
}
