using UnityEngine;

public class TutorialEventsV2 : TutorialEvents
{
    private delegate void TutorialEvent();

    #region Fields
    [Header("UI")]
    [SerializeField]
    private GameObject crosshair;
    [SerializeField]
    private GameObject continuePrompt;
    [SerializeField]
    private GameObject skipPrompt;
    [SerializeField]
    private GameObject[] normalGameUI;

    [Header("Info Prompts")]
    [SerializeField]
    private InformationPromptController infoPromptController;
    [SerializeField]
    private string[] infoPrompts;

    [Header("02-ShowStatue")]
    [SerializeField]
    private GameObject infoStatue;

    [Header("03-ShowMausoleum")]
    [SerializeField]
    private GameObject infoMausoleum;

    [Header("04-ShowFountain")]
    [SerializeField]
    private GameObject infoFountain;

    [Header("06-ShowSpawnerLeft")]
    [SerializeField]
    private GameObject infoSpawnerLeft;

    [Header("07-ShowSpawnerCenter")]
    [SerializeField]
    private GameObject infoSpawnerCenter;

    [Header("08-ShowSpawnerRight")]
    [SerializeField]
    private GameObject infoSpawnerRight;

    [Header("10-ShowPathFountain")]
    [SerializeField]
    private GameObject infoPathFountain;
    [SerializeField]
    private GameObject infoFountainHealth;
    [SerializeField]
    private GameObject fountainBanner;

    [Header("11-ShowPathMausoleum")]
    [SerializeField]
    private GameObject infoPathMausoleum;
    [SerializeField]
    private GameObject infoMausoleumHealth;
    [SerializeField]
    private GameObject mausoleumBanner;

    [Header("12-ShowPathStatue")]
    [SerializeField]
    private GameObject infoPathStatue;
    [SerializeField]
    private GameObject infoStatueHealth;
    [SerializeField]
    private GameObject statueBanner;

    [Header("13-ShowShieldIcon")]
    [SerializeField]
    private GameObject infoShieldIcon;
    [SerializeField]
    private GameObject objectiveMarkers;

    [Header("15-PlayerStartedMoving")]
    [SerializeField]
    private GameObject playerExitTrigger;

    [Header("16-ArrivedAtFountain")]
    [SerializeField]
    private GameObject fountainEntryTrigger;

    private TutorialController tutorialController;
    private TutorialEvent[] events;
    private Player player;
    private bool waitingForPlayer = false;
    private TutorialEvent waitEndedCallback = null;
    private bool tutorialContinues = false;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        tutorialController = GetComponent<TutorialController>();
        UnityEngine.Assertions.Assert.IsNotNull(tutorialController, "ERROR: A TutorialController Component could not be found by TutorialEventsV2 in GameObject " + gameObject.name);

        // Asigned from the Inspector
        UnityEngine.Assertions.Assert.IsNotNull(crosshair, "ERROR: The TutorialEventsV2 in gameObject '" + gameObject.name + "' doesn't have a GameObject (crosshair) assigned!");
        UnityEngine.Assertions.Assert.IsNotNull(continuePrompt, "ERROR: The TutorialEventsV2 in gameObject '" + gameObject.name + "' doesn't have a GameObject (continuePrompt) assigned!");
        UnityEngine.Assertions.Assert.IsNotNull(skipPrompt, "ERROR: The TutorialEventsV2 in gameObject '" + gameObject.name + "' doesn't have a GameObject (skipPrompt) assigned!");
        UnityEngine.Assertions.Assert.IsNotNull(infoPromptController, "ERROR: The TutorialEventsV2 in gameObject '" + gameObject.name + "' doesn't have a InformationPromptController assigned!");
        UnityEngine.Assertions.Assert.IsNotNull(infoStatue, "ERROR: The TutorialEventsV2 in gameObject '" + gameObject.name + "' doesn't have a GameObject (infoStatue) assigned!");
        UnityEngine.Assertions.Assert.IsNotNull(infoMausoleum, "ERROR: The TutorialEventsV2 in gameObject '" + gameObject.name + "' doesn't have a GameObject (ìnfoMausoleum) assigned!");
        UnityEngine.Assertions.Assert.IsNotNull(infoFountain, "ERROR: The TutorialEventsV2 in gameObject '" + gameObject.name + "' doesn't have a GameObject (infoFountain) assigned!");

        UnityEngine.Assertions.Assert.IsNotNull(infoSpawnerLeft, "ERROR: The TutorialEventsV2 in gameObject '" + gameObject.name + "' doesn't have a GameObject (infoSpawnerLeft) assigned!");
        UnityEngine.Assertions.Assert.IsNotNull(infoSpawnerCenter, "ERROR: The TutorialEventsV2 in gameObject '" + gameObject.name + "' doesn't have a GameObject (infoSpawnerCenter) assigned!");
        UnityEngine.Assertions.Assert.IsNotNull(infoSpawnerRight, "ERROR: The TutorialEventsV2 in gameObject '" + gameObject.name + "' doesn't have a GameObject (infoSpawnerRight) assigned!");

        UnityEngine.Assertions.Assert.IsNotNull(infoPathFountain, "ERROR: The TutorialEventsV2 in gameObject '" + gameObject.name + "' doesn't have a GameObject (infoPathFountain) assigned!");
        UnityEngine.Assertions.Assert.IsNotNull(infoFountainHealth, "ERROR: The TutorialEventsV2 in gameObject '" + gameObject.name + "' doesn't have a GameObject (infoFountainHealth) assigned!");
        UnityEngine.Assertions.Assert.IsNotNull(fountainBanner, "ERROR: The TutorialEventsV2 in gameObject '" + gameObject.name + "' doesn't have a GameObject (fountainBanner) assigned!");
        UnityEngine.Assertions.Assert.IsNotNull(infoPathMausoleum, "ERROR: The TutorialEventsV2 in gameObject '" + gameObject.name + "' doesn't have a GameObject (infoPathMausoleum) assigned!");
        UnityEngine.Assertions.Assert.IsNotNull(infoMausoleumHealth, "ERROR: The TutorialEventsV2 in gameObject '" + gameObject.name + "' doesn't have a GameObject (infoMausoleumHealth) assigned!");
        UnityEngine.Assertions.Assert.IsNotNull(mausoleumBanner, "ERROR: The TutorialEventsV2 in gameObject '" + gameObject.name + "' doesn't have a GameObject (mausoleumBanner) assigned!");
        UnityEngine.Assertions.Assert.IsNotNull(infoPathStatue, "ERROR: The TutorialEventsV2 in gameObject '" + gameObject.name + "' doesn't have a GameObject (infoPathStatue) assigned!");
        UnityEngine.Assertions.Assert.IsNotNull(infoStatueHealth, "ERROR: The TutorialEventsV2 in gameObject '" + gameObject.name + "' doesn't have a GameObject (infoStatueHealth) assigned!");
        UnityEngine.Assertions.Assert.IsNotNull(statueBanner, "ERROR: The TutorialEventsV2 in gameObject '" + gameObject.name + "' doesn't have a GameObject (statueBanner) assigned!");

        UnityEngine.Assertions.Assert.IsNotNull(infoShieldIcon, "ERROR: The TutorialEventsV2 in gameObject '" + gameObject.name + "' doesn't have a GameObject (infoShieldIcon) assigned!");
        UnityEngine.Assertions.Assert.IsNotNull(objectiveMarkers, "ERROR: The TutorialEventsV2 in gameObject '" + gameObject.name + "' doesn't have a GameObject (objectiveMarkers) assigned!");

        events = new TutorialEvent[]{
            ShowVlad,               // 00
            ShowMonumentsPrompt,    // 01
            ShowStatue,             // 02
            ShowMausoleum,          // 03
            ShowFountain,           // 04
            ShowSpawnersPrompt,     // 05
            ShowSpawnerLeft,        // 06
            ShowSpawnerCenter,      // 07
            ShowSpawnerRight,       // 08
            ShowEnemiesPathPrompt,  // 09
            ShowPathFountain,       // 10
            ShowPathMausoleum,      // 11
            ShowPathStatue,         // 12
            ShowShieldIcon,         // 13
            FinishTutorial,         // 14
            PlayerStartedMoving,    // 15
            ArrivedAtFountain       // 16
        };
    }

    private void Start()
    {
        player = GameManager.instance.GetPlayer1();
        playerExitTrigger.SetActive(false);
        fountainEntryTrigger.SetActive(false);
    }
    #endregion

    #region Public Methods
    public override void SetTutorialEnemiesManager(TutorialEnemiesManager tutorialEnemiesManager)
    {
        
    }

    public override void OnTutorialWillStart()
    {
        crosshair.SetActive(false);
        continuePrompt.SetActive(false);
        skipPrompt.SetActive(false);
        SetNormalGameUIVisibility(false);
    }

    public void OnTutorialStarted()
    {
        skipPrompt.SetActive(true);
    }

    public void OnTutorialWillEnd()
    {
        continuePrompt.SetActive(false);
        skipPrompt.SetActive(false);
        SetNormalGameUIVisibility(true);
        crosshair.SetActive(true);
    }

    public override void OnTutorialEnded()
    {
        if (!tutorialContinues)
        {
            gameObject.SetActive(false);
            GameManager.instance.OnTutorialFinished();
        }
        else
        {
            GameManager.instance.OnTutorialFinished(TutorialEndFadeCallback);
        }
    }

    public override void LaunchEvent(int eventIndex)
    {
        if (eventIndex >= 0 && eventIndex < events.Length)
        {
            //Debug.Log("Launching event with index " + eventIndex);
            events[eventIndex]();
        }
        else
            Debug.LogError("ERROR: eventIndex parameter (" + eventIndex + ") out of range in TutorialEventsV2::LaunchEvent in gameObject '" + gameObject.name + "'!");
    }

    public void OnXPressed()
    {
        if (waitingForPlayer)
        {
            Continue();
        }
    }
    #endregion

    #region Private Methods
    #region Events
    // 00
    private void ShowVlad()
    {
        infoPromptController.ShowPrompt(infoPrompts[0]);
        WaitForUser(ShowVladOver);
    }

    // 00 OVER
    private void ShowVladOver()
    {
        infoPromptController.HidePrompt();
    }

    // 01
    private void ShowMonumentsPrompt()
    {
        infoPromptController.ShowPrompt(infoPrompts[1]);
    }

    // 02
    private void ShowStatue()
    {
        infoStatue.SetActive(true);
        WaitForUser(ShowStatueOver);
    }

    // 02 OVER
    private void ShowStatueOver()
    {
        infoStatue.SetActive(false);
    }

    // 03
    private void ShowMausoleum()
    {
        infoMausoleum.SetActive(true);
        WaitForUser(ShowMausoleumOver);
    }

    // 03 OVER
    private void ShowMausoleumOver()
    {
        infoMausoleum.SetActive(false);
    }

    // 04
    private void ShowFountain()
    {
        infoFountain.SetActive(true);
        WaitForUser(ShowFountainOver);
    }

    // 04 OVER
    private void ShowFountainOver()
    {
        infoFountain.SetActive(false);
        infoPromptController.HidePrompt();
    }

    // 05
    private void ShowSpawnersPrompt()
    {
        infoPromptController.ShowPrompt(infoPrompts[2]);
    }

    // 06
    private void ShowSpawnerLeft()
    {
        infoSpawnerLeft.SetActive(true);
        WaitForUser(ShowSpawnerLeftOver);
    }

    // 06 OVER
    private void ShowSpawnerLeftOver()
    {
        infoSpawnerLeft.SetActive(false);
    }

    // 07
    private void ShowSpawnerCenter()
    {
        infoSpawnerCenter.SetActive(true);
        WaitForUser(ShowSpawnerCenterOver);
    }

    // 07 OVER
    private void ShowSpawnerCenterOver()
    {
        infoSpawnerCenter.SetActive(false);
    }

    // 08
    private void ShowSpawnerRight()
    {
        infoSpawnerRight.SetActive(true);
        WaitForUser(ShowSpawnerRightOver);
    }

    // 08 OVER
    private void ShowSpawnerRightOver()
    {
        infoSpawnerRight.SetActive(false);
        infoPromptController.HidePrompt();
    }

    // 09
    private void ShowEnemiesPathPrompt()
    {
        infoPromptController.ShowPrompt(infoPrompts[3]);
    }

    // 10
    private void ShowPathFountain()
    {
        infoPathFountain.SetActive(true);
        infoFountainHealth.SetActive(true);
        fountainBanner.SetActive(true);
        WaitForUser(ShowPathFountainOver);
    }

    // 10 OVER
    private void ShowPathFountainOver()
    {
        infoPathFountain.SetActive(false);
        infoFountainHealth.SetActive(false);
    }

    // 11
    private void ShowPathMausoleum()
    {
        infoPathMausoleum.SetActive(true);
        infoMausoleumHealth.SetActive(true);
        mausoleumBanner.SetActive(true);
        WaitForUser(ShowPathMausoleumOver);
    }

    // 11 OVER
    private void ShowPathMausoleumOver()
    {
        infoPathMausoleum.SetActive(false);
        infoMausoleumHealth.SetActive(false);
    }

    // 12
    private void ShowPathStatue()
    {
        infoPathStatue.SetActive(true);
        infoStatueHealth.SetActive(true);
        statueBanner.SetActive(true);
        WaitForUser(ShowPathStatueOver);
    }

    // 12 OVER
    private void ShowPathStatueOver()
    {
        infoPathStatue.SetActive(false);
        infoStatueHealth.SetActive(false);
        infoPromptController.HidePrompt();
    }

    // 13
    private void ShowShieldIcon()
    {
        infoPromptController.ShowPrompt(infoPrompts[4]);
        infoShieldIcon.SetActive(true);
        objectiveMarkers.SetActive(true);
        WaitForUser(ShowShieldIconOver);
    }

    // 13 OVER
    private void ShowShieldIconOver()
    {
        infoShieldIcon.SetActive(false);
        infoPromptController.HidePrompt();
    }

    // 14
    private void FinishTutorial()
    {
        tutorialContinues = true;
        playerExitTrigger.transform.position = player.transform.position;
        playerExitTrigger.SetActive(true);
        tutorialController.RequestEndTutorial();
    }

    // 15
    private void PlayerStartedMoving()
    {
        fountainEntryTrigger.SetActive(true);
    }

    // 16
    private void ArrivedAtFountain()
    {
        ArrivedAtFountainOver();
    }

    // 16 OVER
    private void ArrivedAtFountainOver()
    {
        GameManager.instance.StartNextRound();
        gameObject.SetActive(false);
    }

    #endregion

    private void WaitForUser(TutorialEvent callback)
    {
        waitEndedCallback = callback;
        tutorialController.PauseTutorial(true);
        continuePrompt.SetActive(true);
        waitingForPlayer = true;
    }

    private void Continue()
    {
        waitingForPlayer = false;
        continuePrompt.SetActive(false);
        tutorialController.PauseTutorial(false);
        waitEndedCallback();
    }

    private void SetNormalGameUIVisibility(bool visible)
    {
        foreach (GameObject go in normalGameUI)
        {
            go.SetActive(visible);
        }
    }

    private void TutorialEndFadeCallback()
    {
        infoPromptController.ShowPrompt(infoPrompts[5]);
    }
    #endregion
}
