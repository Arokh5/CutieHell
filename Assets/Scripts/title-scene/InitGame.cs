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
    private InfoPanel frontUI;
    [SerializeField]
    private float frontUIBlendDuration = 1.0f;

    [Header("Sounds")]
    [SerializeField]
    private AudioClip changeSelectionClip;
    [SerializeField]
    private AudioClip startClip;
    [SerializeField]
    private AudioClip clickClip;
    [SerializeField]
    private AudioClip backClip;

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
        UnityEngine.Assertions.Assert.IsNotNull(frontUI, "ERROR: Front UI (InfoPanel) not assigned for InitGame in GameObject '" + gameObject.name + "'!");
        UnityEngine.Assertions.Assert.IsNotNull(startClip, "ERROR: Start Clip (AudioClip) not assigned for InitGame in GameObject '" + gameObject.name + "'!");
        UnityEngine.Assertions.Assert.IsNotNull(changeSelectionClip, "ERROR: Change Selection Clip (AudioClip) not assigned for InitGame in GameObject '" + gameObject.name + "'!");
        UnityEngine.Assertions.Assert.IsNotNull(clickClip, "ERROR: Click Clip (AudioClip) not assigned for InitGame in GameObject '" + gameObject.name + "'!");
        UnityEngine.Assertions.Assert.IsNotNull(backClip, "ERROR: Back Clip (AudioClip) not assigned for InitGame in GameObject '" + gameObject.name + "'!");
        audioSource = GetComponent<AudioSource>();
        UnityEngine.Assertions.Assert.IsNotNull(audioSource, "ERROR: An AudioSource component could not be found by the InitGame script in GameObject '" + gameObject.name + "'!");

        for (int i = 0; i < buttons.Length; ++i)
        {
            if (i == 0)
            {
                buttons[i].SelectButton();
            }
            else
            {
                buttons[i].UnselectButton();
            }
        }
    }

    private void Start()
    {
        helpPanel.SetActive(false);
        loadingText.enabled = false;
        blackFader.TurnOpaque();
        frontUI.Hide();
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
        frontUI.ShowAnimated(frontUIBlendDuration, OnMenuShown);
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

            PlayChangeSelectionClip();
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

            PlayChangeSelectionClip();
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
                    PlayStartClip();
                    menuActive = false;
                    blackFader.FadeToOpaque(0.5f, LoadGameScene);
                    break;

                case 1:
                    PlayClickClip();
                    menuActive = false;
                    helpPanel.SetActive(true);
                    break;

                case 2:
                    PlayClickClip();
                    blackFader.FadeToOpaque(0.5f, LoadCreditsScene);
                    break;

                case 3:
                    PlayClickClip();
                    Application.Quit();
                    break;
            }
        }

    }

    private void ProcessBackButton()
    {
        if (InputManager.instance.GetOButtonDown())
        {
            PlayBackClip();
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

    private void PlayChangeSelectionClip()
    {
        if (changeSelectionClip)
            audioSource.PlayOneShot(changeSelectionClip);
    }

    private void PlayStartClip()
    {
        if (startClip)
            audioSource.PlayOneShot(startClip);
    }

    private void PlayClickClip()
    {
        if (clickClip)
            audioSource.PlayOneShot(clickClip);
    }
    private void PlayBackClip()
    {
        if (backClip)
            audioSource.PlayOneShot(backClip);
    }
    #endregion
}
