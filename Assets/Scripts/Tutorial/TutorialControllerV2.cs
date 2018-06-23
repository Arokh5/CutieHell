using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;

public class TutorialControllerV2 : TutorialController
{
    #region Fields
    [Header("Tutorial config")]
    [SerializeField]
    private CinemachineBrain cinemachineBrain;
    [SerializeField]
    private ScreenFadeController screenFadeController;
    [SerializeField]
    private CinematicStripes stripes;

    [Header("UI objects")]
    [SerializeField]
    private GameObject startMessage;
    [SerializeField]
    private float startMessageDuration;
    [SerializeField]
    private GameObject endMessage;
    [SerializeField]
    private float endMessageDuration;
    [SerializeField]
    private GameObject tutorialUIParent;
    [SerializeField]
    private GameObject crosshair;

    [Header("Player")]
    [SerializeField]
    private Player player;
    [SerializeField]
    private State playerDefaultState;
    [SerializeField]
    private State[] tutorialStates;

    private PlayableDirector director;
    private TutorialEvents tutorialEvents;

    private bool running;
    private bool paused;
    private int playerStateIndex = -1;
    private Vector3 playerStartingPos;
    private Quaternion playerStartingRot;

    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        UnityEngine.Assertions.Assert.IsNotNull(cinemachineBrain, "ERROR: The TutorialControllerV2 in gameObject '" + gameObject.name + "' doesn't have a CinemachineBrain assigned!");
        UnityEngine.Assertions.Assert.IsNotNull(screenFadeController, "ERROR: The TutorialControllerV2 in gameObject '" + gameObject.name + "' doesn't have a ScreenFadeController assigned!");
        UnityEngine.Assertions.Assert.IsNotNull(stripes, "ERROR: The TutorialControllerV2 in gameObject '" + gameObject.name + "' doesn't have a CinematicStripes assigned!");

        director = GetComponent<PlayableDirector>();
        UnityEngine.Assertions.Assert.IsNotNull(director, "ERROR: A PlayableDirector Component could not be found by TutorialController in GameObject " + gameObject.name);
        tutorialEvents = GetComponent<TutorialEvents>();
        UnityEngine.Assertions.Assert.IsNotNull(tutorialEvents, "ERROR: A TutorialEvents Component could not be found by TutorialController in GameObject " + gameObject.name);

        playerStartingPos = player.transform.position;
        playerStartingRot = player.transform.rotation;
    }
    #endregion

    #region Public Methods
    public override void PauseTutorial(bool pause)
    {
        throw new System.NotImplementedException();
    }

    public override bool IsRunning()
    {
        throw new System.NotImplementedException();
    }

    public override void RequestStartTutorial()
    {
        if (!running)
        {
            playerStateIndex = -1;
            NextPlayerState();  // StoppedState
            tutorialEvents.OnTutorialStarted();
            startMessage.SetActive(true);
            screenFadeController.TurnOpaque();
            Invoke("TutorialStarter", startMessageDuration);
        }
    }

    public override void RequestBypassTutorial()
    {
        screenFadeController.TurnOpaque();
        OnTutorialEnded();
    }

    public override void RequestEndTutorial()
    {
        throw new System.NotImplementedException();
    }

    public override void LaunchEvent(int eventIndex)
    {
        throw new System.NotImplementedException();
    }

    public override void NextPlayerState()
    {
        throw new System.NotImplementedException();
    }

    public override int GetEnemiesCount()
    {
        throw new System.NotImplementedException();
    }

    public override void PauseTimelineAndReleaseCamera()
    {
        throw new System.NotImplementedException();
    }

    public override void ResumeTimelineAndCaptureCamera()
    {
        throw new System.NotImplementedException();
    }
    #endregion

    #region Private Methods
    private void OnTutorialEnded()
    {
        running = false;
        director.Stop();
        cinemachineBrain.enabled = false;
        player.transform.position = playerStartingPos;
        player.transform.rotation = playerStartingRot;
        Camera.main.GetComponent<CameraController>().SetCameraXAngle(0);
        tutorialUIParent.SetActive(false);
        gameObject.SetActive(false);

        player.TransitionToState(playerDefaultState);

        tutorialEvents.OnTutorialEnded();

        endMessage.SetActive(true);
        Invoke("TutorialEnder", endMessageDuration);
    }

    private void TutorialStarter()
    {
        startMessage.SetActive(false);
        screenFadeController.FadeToTransparent(StartTutorial);
    }

    private void TutorialEnder()
    {
        endMessage.SetActive(false);
        GameManager.instance.OnTutorialFinished();
    }

    private void StartTutorial()
    {
        running = true;
        director.Play();
    }
    #endregion
}
