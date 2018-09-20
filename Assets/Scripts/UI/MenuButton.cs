using UnityEngine;

public class MenuButton : MonoBehaviour
{
    #region Fields
    [SerializeField]
    private GameObject selectedButton;
    [SerializeField]
    private GameObject unselectedButton;
    #endregion

    #region Public Methods
    public void SelectButton()
    {
        unselectedButton.SetActive(false);
        selectedButton.SetActive(true);
    }

    public void UnselectButton()
    {
        unselectedButton.SetActive(true);
        selectedButton.SetActive(false);
    }
    #endregion
}
