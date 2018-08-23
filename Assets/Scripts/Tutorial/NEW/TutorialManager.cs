using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    private delegate void VoidCallback();

    #region Fields
    [Header("Setup")]
    [SerializeField]
    private ScreenFadeController screenFadeController;
    [SerializeField]
    private CinematicStripes cinematicStripes;
    
    [Header("Scripted messages")]
    [SerializeField]
    private GameObject userWaitPrompts;
    [SerializeField]
    private GameObject firstMessage;

    // General
    private bool tutorialRunning;

    // Player
    Player player;

    // Skiping all further messages
    private bool skipAll = false;

    // WaitForUser related
    private VoidCallback waitEndedCallback = null;
    private bool waitingForUser = false;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        UnityEngine.Assertions.Assert.IsNotNull(screenFadeController, "ERROR: Screen Fade Controller (ScreenFadeController) not assigned for TutorialManager script in GameObject " + gameObject.name);
        UnityEngine.Assertions.Assert.IsNotNull(cinematicStripes, "ERROR: Cinematic Stripes (CinematicStripes) not assigned for TutorialManager script in GameObject " + gameObject.name);
        UnityEngine.Assertions.Assert.IsNotNull(userWaitPrompts, "ERROR: User Wait Prompts (GameObject) not assigned for TutorialManager script in GameObject " + gameObject.name);
        UnityEngine.Assertions.Assert.IsNotNull(firstMessage, "ERROR: First Message (GameObject) not assigned for TutorialManager script in GameObject " + gameObject.name);
    }

    private void Start()
    {
        player = GameManager.instance.GetPlayer1();
    }

    private void Update()
    {
        if (tutorialRunning)
        {
            if (waitingForUser && !GameManager.instance.gameIsPaused)
            {
                if (InputManager.instance.GetXButtonDown())
                    Continue();
                else if (InputManager.instance.GetOButtonDown())
                    SkipAll();
            }
        }
    }
    #endregion

    #region Public Methods
    public void RequestStartTutorial()
    {
        tutorialRunning = true;
        player.OnRoundOver(); // Triggers stopped state
        cinematicStripes.Show();
        screenFadeController.TurnOpaque();
        screenFadeController.FadeToAlpha(0.9f, 1.0f, ShowFirstMessage);
    }
    #endregion

    #region Private Methods

    private void RequestEndTutorial()
    {
        cinematicStripes.HideAnimated();
        player.OnRoundStarted();
        tutorialRunning = false;
    }

    #region Scripted messages
    private void ShowFirstMessage()
    {
        userWaitPrompts.SetActive(true);
        firstMessage.SetActive(true);
        WaitForUser(OnFirstMessageClosed);
    }

    private void OnFirstMessageClosed()
    {
        userWaitPrompts.SetActive(false);
        firstMessage.SetActive(false);
        if (!skipAll)
        {
            // Next scripted message
            screenFadeController.FadeToTransparent(0.5f, RequestEndTutorial);
        }
        else
        {
            // Finish
            screenFadeController.TurnTransparent();
            RequestEndTutorial();
        }
    }
    #endregion

    #region Helper Methods
    private void WaitForUser(VoidCallback callback)
    {
        waitEndedCallback = callback;
        userWaitPrompts.SetActive(true);
        waitingForUser = true;
    }

    private void Continue()
    {
        waitingForUser = false;
        userWaitPrompts.SetActive(false);
        waitEndedCallback();
    }

    private void SkipAll()
    {
        skipAll = true;
        waitingForUser = false;
        userWaitPrompts.SetActive(false);
        waitEndedCallback();
    }
    #endregion

    #endregion
}
