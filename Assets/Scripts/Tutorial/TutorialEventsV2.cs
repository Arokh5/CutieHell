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

    [Header("Info Prompts")]
    public InformationPromptController infoPromptController;
    [SerializeField]
    private string[] infoPrompts;

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


        events = new TutorialEvent[]{
            ShowVladMessage         // 00
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

    public override void OnTutorialStarted()
    {
        crosshair.SetActive(false);
        continuePrompt.SetActive(false);
        skipPrompt.SetActive(true);
    }

    public override void OnTutorialEnded()
    {
        crosshair.SetActive(true);
        continuePrompt.SetActive(false);
        skipPrompt.SetActive(false);
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
            continuePrompt.SetActive(false);
            waitingForPlayer = false;
            waitEndedCallback();
        }
    }
    #endregion

    #region Private Methods
    #region Events
    // 00
    private void ShowVladMessage()
    {
        infoPromptController.ShowPrompt(infoPrompts[0]);
        WaitForUser(VladMessageWaitOver);
    }

    // 00 after wait
    private void VladMessageWaitOver()
    {
        infoPromptController.HidePrompt();
    }

    #endregion

    private void WaitForUser(TutorialEvent callback)
    {
        waitEndedCallback = callback;
        continuePrompt.SetActive(true);
        waitingForPlayer = true;
    }
    #endregion
}
