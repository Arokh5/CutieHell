using System.Collections;
using System.Collections.Generic;
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

    [Header("01-ShowStatue")]
    [SerializeField]
    private GameObject infoStatue;

    [Header("01-ShowMausoleum")]
    [SerializeField]
    private GameObject infoMausoleum;

    [Header("01-ShowFountain")]
    [SerializeField]
    private GameObject infoFountain;

    private TutorialController tutorialController;
    private TutorialEvent[] events;
    private Player player;
    private bool waitingForPlayer = false;
    private TutorialEvent waitEndedCallback = null;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        tutorialController = GetComponent<TutorialController>();
        UnityEngine.Assertions.Assert.IsNotNull(tutorialController, "ERROR: A TutorialController Component could not be found by TutorialEventsV2 in GameObject " + gameObject.name);
        UnityEngine.Assertions.Assert.IsNotNull(crosshair, "ERROR: The TutorialEventsV2 in gameObject '" + gameObject.name + "' doesn't have a GameObject (crosshair) assigned!");
        UnityEngine.Assertions.Assert.IsNotNull(continuePrompt, "ERROR: The TutorialEventsV2 in gameObject '" + gameObject.name + "' doesn't have a GameObject (continuePrompt) assigned!");
        UnityEngine.Assertions.Assert.IsNotNull(skipPrompt, "ERROR: The TutorialEventsV2 in gameObject '" + gameObject.name + "' doesn't have a GameObject (skipPrompt) assigned!");
        UnityEngine.Assertions.Assert.IsNotNull(infoPromptController, "ERROR: The TutorialEventsV2 in gameObject '" + gameObject.name + "' doesn't have a InformationPromptController assigned!");
        UnityEngine.Assertions.Assert.IsNotNull(infoStatue, "ERROR: The TutorialEventsV2 in gameObject '" + gameObject.name + "' doesn't have a GameObject (infoStatue) assigned!");
        UnityEngine.Assertions.Assert.IsNotNull(infoMausoleum, "ERROR: The TutorialEventsV2 in gameObject '" + gameObject.name + "' doesn't have a GameObject (ìnfoMausoleum) assigned!");
        UnityEngine.Assertions.Assert.IsNotNull(infoFountain, "ERROR: The TutorialEventsV2 in gameObject '" + gameObject.name + "' doesn't have a GameObject (infoFountain) assigned!");

        events = new TutorialEvent[]{
            ShowVlad,               // 00
            ShowMonumentsPrompt,    // 01
            ShowStatue,             // 02
            ShowMausoleum,          // 03
            ShowFountain            // 04
        };
    }

    private void Start()
    {
        player = GameManager.instance.GetPlayer1();
    }
    #endregion

    #region Public Methods
    public override void SetTutorialEnemiesManager(TutorialEnemiesManager tutorialEnemiesManager)
    {
        throw new System.NotImplementedException();
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

    public override void OnTutorialEnded()
    {
        crosshair.SetActive(true);
        continuePrompt.SetActive(false);
        skipPrompt.SetActive(false);
        SetNormalGameUIVisibility(false);
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
    #endregion
}
