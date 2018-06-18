using UnityEngine;

public class MenuButton : MonoBehaviour {

    public GameObject selectedButton;

    public void SelectButton()
    {
        selectedButton.SetActive(true);
    }

    public void UnselectButton()
    {
        selectedButton.SetActive(false);
    }
}
