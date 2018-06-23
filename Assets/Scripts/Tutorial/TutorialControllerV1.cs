using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;

public class TutorialControllerV1 : TutorialController
{
    #region Fields
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

    [Header("Tutorial config")]
    [SerializeField]
    private CinemachineBrain cinemachineBrain;
    [SerializeField]
    private ScreenFadeController screenFadeController;
    [SerializeField]
    private ScenarioController scenarioController;

    [Header("Player")]
    [SerializeField]
    private Player player;
    [SerializeField]
    private State playerDefaultState;
    [SerializeField]
    private State[] tutorialStates;

    [Header("Other")]
    [SerializeField]
    private Monument tutorialMonument;

    private int playerStateIndex = -1;
    private bool running;
    private bool paused;
    private Vector3 playerStartingPos;
    private Quaternion playerStartingRot;
    [HideInInspector]
    public PlayableDirector director;
    private TutorialEvents tutorialEvents;
    private TutorialEnemiesManager tutorialEnemiesManager;
    private AIZoneController[] zoneControllers;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        UnityEngine.Assertions.Assert.IsNotNull(cinemachineBrain, "ERROR: The TutorialController in gameObject '" + gameObject.name + "' doesn't have a CinemachineBrain assigned!");
        UnityEngine.Assertions.Assert.IsNotNull(screenFadeController, "ERROR: The TutorialController in gameObject '" + gameObject.name + "' doesn't have a ScreenFadeController assigned!");
        UnityEngine.Assertions.Assert.IsNotNull(player, "ERROR: The TutorialController in gameObject '" + gameObject.name + "' doesn't have a Player assigned!");
        director = GetComponent<PlayableDirector>();
        UnityEngine.Assertions.Assert.IsNotNull(director, "ERROR: A PlayableDirector Component could not be found by TutorialController in GameObject " + gameObject.name);
        tutorialEvents = GetComponent<TutorialEvents>();
        UnityEngine.Assertions.Assert.IsNotNull(tutorialEvents, "ERROR: A TutorialEvents Component could not be found by TutorialController in GameObject " + gameObject.name);
        zoneControllers = FindObjectsOfType<AIZoneController>();
        tutorialEnemiesManager = new TutorialEnemiesManager();
        tutorialEvents.SetTutorialEnemiesManager(tutorialEnemiesManager);
        playerStartingPos = player.transform.position;
        playerStartingRot = player.transform.rotation;
        startMessage.SetActive(false);
        endMessage.SetActive(false);
    }

    private void Update()
    {
        tutorialEnemiesManager.RemoveDeadEnemies();

        if (!paused && GameManager.instance.gameIsPaused)
        {
            PauseTutorial(true);
        }
        else if (paused && !GameManager.instance.gameIsPaused)
        {
            PauseTutorial(false);
            crosshair.SetActive(false);
        }
    }
    #endregion

    #region Public Methods
    public override void PauseTutorial(bool pause)
    {
        if (running)
        {
            paused = pause;
            if (running && director.playableGraph.IsValid())
                director.playableGraph.GetRootPlayable(0).SetSpeed(pause ? 0 : 1);
            // Using director.Pause() allows the cameras to snap back to the default priority settings.
        }
    }

    public override bool IsRunning()
    {
        return running;
    }

    public override void RequestStartTutorial()
    {
        if (!running)
        {
            player.AddEvilPoints(-12);
            playerStateIndex = -1;
            NextPlayerState();
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
        startMessage.SetActive(false);
        screenFadeController.FadeToOpaque(OnTutorialEnded);
    }

    public override void LaunchEvent(int eventIndex)
    {
        tutorialEvents.LaunchEvent(eventIndex);
    }

    public override void NextPlayerState()
    {
        player.TransitionToState(tutorialStates[++playerStateIndex]);
    }

    public override int GetEnemiesCount()
    {
        return tutorialEvents.GetEnemiesCount();
    }

    public override void PauseTimelineAndReleaseCamera()
    {
        director.Pause();
    }

    public override void ResumeTimelineAndCaptureCamera()
    {
        director.Resume();
    }
    #endregion

    #region Private Methods
    private void OnTutorialEnded()
    {
        running = false;
        director.Stop();
        cinemachineBrain.enabled = false;
        tutorialMonument.TakeDamage(tutorialMonument.GetCurrentHealth(), AttackType.NONE);
        player.transform.position = playerStartingPos;
        player.transform.rotation = playerStartingRot;
        Camera.main.GetComponent<CameraController>().SetCameraXAngle(0);
        player.AddEvilPoints(player.GetMaxEvilLevel() - player.GetEvilLevel());
        tutorialUIParent.SetActive(false);
        gameObject.SetActive(false);

        player.TransitionToState(playerDefaultState);

        foreach (AIZoneController zoneController in zoneControllers)
        {
            if (zoneController.HasEnemies())
            {
                zoneController.DestroyAllEnemies();
                scenarioController.OnZoneEmpty();
            }
        }

        tutorialEnemiesManager.ClearEnemies();
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
