using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InitGame : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private MenuButton[] buttons;

    private int index = 0;

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


    private void ToggleBetweenButtons()
    {
        if (InputManager.instance.GetPadDownDown() || InputManager.instance.GetLeftStickDownDown())
        {
            buttons[index].UnselectButton();

            if (index == buttons.Length - 1)
            {
                index = 0;
            }
            else
            {
                index++;
            }

            buttons[index].SelectButton();
        }
        else if (InputManager.instance.GetPadUpDown() || InputManager.instance.GetLeftStickUpDown())
        {
            buttons[index].UnselectButton();

            if (index == 0)
            {
                index = buttons.Length - 1;
            }
            else
            {
                index--;
            }

            buttons[index].SelectButton();
        }
    }

    private void PressButton()
    {
        if (InputManager.instance.GetXButtonDown())
        {
            switch (index)
            {
                case 0:
                    Destroy(BackgroundMusic.instance.gameObject);
                    SceneManager.LoadScene("Garden Scene", LoadSceneMode.Single);
                    break;

                case 1:
                    SceneManager.LoadScene("OptionsScreen", LoadSceneMode.Single);
                    break;

                case 2:
                    SceneManager.LoadScene("CreditsScreen", LoadSceneMode.Single);
                    break;

                case 3:
                    Application.Quit();
                    break;
            }
        }

    }

	#endregion
}