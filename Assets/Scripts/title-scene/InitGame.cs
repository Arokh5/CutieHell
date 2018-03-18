using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InitGame : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private GameObject[] buttons;
    [SerializeField]
    private GameObject controlsList;

    private int index = 0;
    private bool showControls = false;

	#endregion
	
	#region Properties
	
    #endregion
	
	#region MonoBehaviour Methods
	
    private void Update()
    {
        ToggleBetweenButtons();
        PressButton();
    }

    #endregion

    #region Public Methods

    #endregion

    #region Private Methods

    private void Start() {
        controlsList.SetActive(showControls);
    }

    private void ToggleBetweenButtons()
    {
        if (InputManager.instance.GetPadDownDown() || InputManager.instance.GetLeftStickDownDown())
        {
            buttons[index].GetComponent<Outline>().enabled = false;

            if (index == buttons.Length - 1)
            {
                index = 0;
            }
            else
            {
                index++;
            }

            buttons[index].GetComponent<Outline>().enabled = true;
        }
        else if (InputManager.instance.GetPadUpDown() || InputManager.instance.GetLeftStickUpDown())
        {
            buttons[index].GetComponent<Outline>().enabled = false;

            if (index == 0)
            {
                index = buttons.Length - 1;
            }
            else
            {
                index--;
            }

            buttons[index].GetComponent<Outline>().enabled = true;
        }
    }

    private void PressButton()
    {
        if (InputManager.instance.GetXButtonDown())
        {
            switch (index)
            {
                case 0:
                    SceneManager.LoadScene("Test Scene", LoadSceneMode.Single);
                    break;

                case 1:
                    showControls = !showControls;
                    break;

                case 2:
                    Application.Quit();
                    break;
            }
        }
        controlsList.SetActive(showControls);

    }

	#endregion
}