using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    private delegate void VoidCallback();

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
    private class TutorialMessage
    {
        public GameObject message;
        public Transform[] foregroundUI;
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

    // General
    private bool tutorialRunning;
    private int messageIndex = 0;
    private List<HierarchyInfo> hierarchyInfos = new List<HierarchyInfo>();

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
        UnityEngine.Assertions.Assert.IsNotNull(tutorialForegroundParent, "ERROR: Tutorial Foreground Parent (Transform) not assigned for TutorialManager script in GameObject " + gameObject.name);
        UnityEngine.Assertions.Assert.IsNotNull(userWaitPrompts, "ERROR: User Wait Prompts (GameObject) not assigned for TutorialManager script in GameObject " + gameObject.name);
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

        messageIndex = 0;
        screenFadeController.FadeToAlpha(interMessagesAlpha, 1.0f, ShowNextMessage);
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
        player.OnRoundStarted();
        tutorialRunning = false;
    }

    #region Scripted messages
    private void ShowNextMessage()
    {
        if (messageIndex < tutorialMessages.Length)
        {
            screenFadeController.FadeToAlpha(messagesAlpha, 0.5f * messageTransitionDuration, () =>
            {
                TutorialMessage tutMessage = tutorialMessages[messageIndex];
                tutMessage.message.SetActive(true);
                BringToForeground(tutMessage.foregroundUI);
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
        tutMessage.message.SetActive(false);
        SendToBackground();
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
