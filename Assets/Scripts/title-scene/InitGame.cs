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

    [Header("Help (Controls)")]
    [SerializeField]
    private GameObject helpPanel;

    [Header("Faders")]
    [SerializeField]
    private ScreenFadeController blackFader;
    [SerializeField]
    private ScreenFadeController foregroundFader;

    [Header("Sounds")]
    [SerializeField]
    private AudioClip startClip;
    [SerializeField]
    private AudioClip selectionClip;

    private AudioSource audioSource;

    private bool menuActive = false;
    private int index = 0;

    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        UnityEngine.Assertions.Assert.IsNotNull(loadingText, "ERROR: Text (loadingText) not assigned for InitGame in GameObject '" + gameObject.name + "'!");
        UnityEngine.Assertions.Assert.IsNotNull(helpPanel, "ERROR: GameObject (helpPanel) not assigned for InitGame in GameObject '" + gameObject.name + "'!");
        UnityEngine.Assertions.Assert.IsNotNull(blackFader, "ERROR: ScreenFadeController (blackFader) not assigned for InitGame in GameObject '" + gameObject.name + "'!");
        UnityEngine.Assertions.Assert.IsNotNull(foregroundFader, "ERROR: ScreenFadeController (foregroundFader) not assigned for InitGame in GameObject '" + gameObject.name + "'!");
        UnityEngine.Assertions.Assert.IsNotNull(startClip, "ERROR: Start Clip (AudioClip) not assigned for InitGame in GameObject '" + gameObject.name + "'!");
        UnityEngine.Assertions.Assert.IsNotNull(selectionClip, "ERROR: Selection Clip (AudioClip) not assigned for InitGame in GameObject '" + gameObject.name + "'!");
        audioSource = GetComponent<AudioSource>();
        UnityEngine.Assertions.Assert.IsNotNull(audioSource, "ERROR: An AudioSource component could not be found by the InitGame script in GameObject '" + gameObject.name + "'!");
    }

    private void Start()
    {
        helpPanel.SetActive(false);
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
            audioSource.PlayOneShot(selectionClip);
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

            audioSource.PlayOneShot(selectionClip);
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
                    audioSource.PlayOneShot(startClip);
                    blackFader.FadeToOpaque(0.5f, LoadGameScene);
                    break;

                case 1:
                    menuActive = false;
                    helpPanel.SetActive(true);
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
            helpPanel.SetActive(false);
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