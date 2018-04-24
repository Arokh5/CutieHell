using System.Collections;
using System.Collections.Generic;
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
