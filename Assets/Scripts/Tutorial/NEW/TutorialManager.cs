using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public delegate void VoidCallback();

    [System.Serializable]
    private struct HierarchyInfo
    {
        private Transform transform;
        private Transform parent;
        private int siblingIndex;

        public HierarchyInfo(Transform transform)
        {
            this.transform = transform;
            parent = transform.parent;
            siblingIndex = transform.GetSiblingIndex();
        }

        public void ReturnToOrigin()
        {
            transform.SetParent(parent);
            transform.SetSiblingIndex(siblingIndex);
        }
    }

    [System.Serializable]
    public class TutorialMessage
    {
        public GameObject message;
        public Transform[] foregroundUI;
        [ShowOnly]
        public bool alreadyShown = false;
    }

    #region Fields
    [Header("Setup")]
    [SerializeField]
    private ScreenFadeController screenFadeController;
    [SerializeField]
    private CinematicStripes cinematicStripes;
    [SerializeField]
    private Transform tutorialForegroundParent;
    
    [Header("Scripted messages")]
    [SerializeField]
    [Tooltip("The alpha value of the screen overlay when showing a message")]
    [Range(0.0f, 1.0f)]
    private float messagesAlpha = 0.9f;
    [SerializeField]
    [Tooltip("The alpha value of the screen overlay when transitioning between messages")]
    [Range(0.0f, 1.0f)]
    private float interMessagesAlpha = 0.85f;
    [SerializeField]
    [Tooltip("The duration (in seconds) of the transition between messages")]
    [Range(0.0f, 2.0f)]
    private float messageTransitionDuration = 0.3f;
    [SerializeField]
    private GameObject userWaitPrompts;
    [SerializeField]
    private TutorialMessage[] tutorialMessages;

    [Header("Event messages")]
    [SerializeField]
    private TutorialMessage[] eventMessages;

    // General
    private bool tutorialRunning;
    private VoidCallback scriptedMessagesEndCallback = null;
    private int messageIndex = 0;
    private List<HierarchyInfo> hierarchyInfos = new List<HierarchyInfo>();
    private float originalTimeScale;

    private VoidCallback eventEndCallback = null;
    private TutorialMessage activeEventMessage = null;

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
        UnityEngine.Assertions.Assert.IsNotNull(tutorialForegroundParent, "ERROR: Tutorial Foreground Parent (Transform) not assigned for TutorialManager script in GameObject " + gameObject.name);
        UnityEngine.Assertions.Assert.IsNotNull(userWaitPrompts, "ERROR: User Wait Prompts (GameObject) not assigned for TutorialManager script in GameObject " + gameObject.name);
    }

    private void Update()
    {
        if (tutorialRunning && !GameManager.instance.gameIsPaused)
        {
            if (waitingForUser)
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
    public void RequestStartTutorial(VoidCallback tutorialEndCallback = null)
    {
        tutorialRunning = true;

        scriptedMessagesEndCallback = tutorialEndCallback;
        cinematicStripes.Show();
        screenFadeController.TurnOpaque();

        messageIndex = 0;
        screenFadeController.FadeToAlpha(interMessagesAlpha, 1.0f, ShowNextMessage);
    }

    public bool LaunchEventMessage(int eventIndex, VoidCallback endCallback = null)
    {
        bool launched = false;

        if (!skipAll)
        {
            if (activeEventMessage == null)
            {
                if (eventIndex >= 0 && eventIndex < eventMessages.Length)
                {
                    TutorialMessage eventMessage = eventMessages[eventIndex];
                    if (!eventMessage.alreadyShown)
                    {
                        launched = true;

                        activeEventMessage = eventMessage;
                        this.eventEndCallback = endCallback;
                        tutorialRunning = true;
                        TimeManager.instance.FreezeTime();
                        screenFadeController.FadeToAlpha(messagesAlpha, 0.5f, () =>
                        {
                            ShowTutorialMessage(activeEventMessage);
                            WaitForUser(OnEventMessageFinished);
                        });
                    }
                }
                else
                {
                    Debug.LogError("ERROR: TutorialManager::LaunchEventMessage called with an eventIndex out of the range [0, " + eventMessages.Length + "[!");
                }
            }
            else
            {
                Debug.LogWarning("WARNING: TutorialManager::LaunchEventMessage called while there is already an active event message. The new event message will be ignored!");
            }
        }

        return launched;
    }
    #endregion

    #region Private Methods
    private void RequestEndTutorial()
    {
        screenFadeController.FadeToTransparent(0.5f, OnTutorialEnded);
        cinematicStripes.HideAnimated();
    }

    private void OnTutorialEnded()
    {
        tutorialRunning = false;

        if (scriptedMessagesEndCallback != null)
        {
            scriptedMessagesEndCallback();
            scriptedMessagesEndCallback = null;
        }
    }

    private void OnEventMessageFinished()
    {
        HideTutorialMessage(activeEventMessage);
        screenFadeController.FadeToTransparent(0.5f, () =>
        {
            TimeManager.instance.ResumeTime();
            activeEventMessage = null;
            tutorialRunning = false;
            if (eventEndCallback != null)
            {
                VoidCallback callback = eventEndCallback;
                eventEndCallback = null;
                callback();
            }
        });
    }

    #region Scripted messages
    private void ShowTutorialMessage(TutorialMessage tutMessage)
    {
        tutMessage.alreadyShown = true;
        if (tutMessage.message)
            tutMessage.message.SetActive(true);

        BringToForeground(tutMessage.foregroundUI);
    }

    private void HideTutorialMessage(TutorialMessage tutMessage)
    {
        if (tutMessage.message)
            tutMessage.message.SetActive(false);

        SendToBackground();
    }

    private void ShowNextMessage()
    {
        if (messageIndex < tutorialMessages.Length)
        {
            screenFadeController.FadeToAlpha(messagesAlpha, 0.5f * messageTransitionDuration, () =>
            {
                TutorialMessage tutMessage = tutorialMessages[messageIndex];
                ShowTutorialMessage(tutMessage);
                WaitForUser(OnMessageClosed);
            });
        }
        else
        {
            RequestEndTutorial();
        }
    }

    private void OnMessageClosed()
    {
        TutorialMessage tutMessage = tutorialMessages[messageIndex];
        HideTutorialMessage(tutMessage);
        ++messageIndex;
        if (!skipAll)
        {
            // Next scripted message
            screenFadeController.FadeToAlpha(interMessagesAlpha, 0.5f * messageTransitionDuration, ShowNextMessage);
        }
        else
        {
            // Finish
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

    private void BringToForeground(Transform[] transforms)
    {
        if (transforms != null)
        {
            for (int i = 0; i < transforms.Length; ++i)
            {
                Transform targetTransform = transforms[i];
                hierarchyInfos.Add(new HierarchyInfo(targetTransform));
                targetTransform.SetParent(tutorialForegroundParent);
            }
        }
}

    private void SendToBackground()
    {
        // Must be in reverse order to ensure setting SiblingIndex properly
        for (int i = hierarchyInfos.Count - 1; i >= 0; --i)
        {
            hierarchyInfos[i].ReturnToOrigin();
        }
        hierarchyInfos.Clear();
    }
    #endregion

    #endregion
}
