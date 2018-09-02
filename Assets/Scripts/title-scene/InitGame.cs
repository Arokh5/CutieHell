using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InitGame : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private MenuButton[] buttons;
    [SerializeField]
    private Text loadingText;

    [Header("Alternate screens")]
    [SerializeField]
    private Image helpScreen;

    [Header("Faders")]
    [SerializeField]
    private ScreenFadeController blackFader;
    [SerializeField]
    private ScreenFadeController foregroundFader;

    private bool menuActive = false;
    private int index = 0;

    #endregion

    #region Properties

    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        UnityEngine.Assertions.Assert.IsNotNull(loadingText, "ERROR: Text (loadingText) not assigned for InitGame in GameObject '" + gameObject.name + "'!");
        UnityEngine.Assertions.Assert.IsNotNull(helpScreen, "ERROR: Image (helpScreen) not assigned for InitGame in GameObject '" + gameObject.name + "'!");
        UnityEngine.Assertions.Assert.IsNotNull(blackFader, "ERROR: ScreenFadeController (blackFader) not assigned for InitGame in GameObject '" + gameObject.name + "'!");
        UnityEngine.Assertions.Assert.IsNotNull(foregroundFader, "ERROR: ScreenFadeController (foregroundFader) not assigned for InitGame in GameObject '" + gameObject.name + "'!");
    }

    private void Start()
    {
        helpScreen.enabled = false;
        loadingText.enabled = false;
        blackFader.TurnOpaque();
        foregroundFader.TurnOpaque();
        blackFader.FadeToTransparent(OnFadedIn);
    }

    private void Update()
    {
        if (menuActive)
        {
            ProcessToggleBetweenButtons();
            ProcessButtonClick();
        }
        else
        {
            ProcessBackButton();
        }
    }

    #endregion

    #region Public Methods
    public void OnFadedIn()
    {
        foregroundFader.FadeToTransparent(OnMenuShown);
    }

    public void OnMenuShown()
    {
        menuActive = true;
    }
    #endregion

    #region Private Methods


    private void ProcessToggleBetweenButtons()
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

    private void ProcessButtonClick()
    {
        if (InputManager.instance.GetXButtonDown())
        {
            switch (index)
            {
                case 0:
                    menuActive = false;
                    blackFader.FadeToOpaque(0.5f, LoadGameScene);
                    break;

                case 1:
                    menuActive = false;
                    helpScreen.enabled = true;
                    break;

                case 2:
                    blackFader.FadeToOpaque(0.5f, LoadCreditsScene);
                    break;

                case 3:
                    Application.Quit();
                    break;
            }
        }

    }

    private void ProcessBackButton()
    {
        if (InputManager.instance.GetOButtonDown())
        {
            menuActive = true;
            helpScreen.enabled = false;
        }
    }

    private void LoadGameScene()
    {
        loadingText.enabled = true;
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    private void LoadCreditsScene()
    {
        SceneManager.LoadScene("Credits",LoadSceneMode.Single);
    }
    #endregion
}