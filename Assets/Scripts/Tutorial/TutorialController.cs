using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;

public class TutorialController : MonoBehaviour
{
    #region Fields
    [Header("UI objects")]
    [SerializeField]
    private GameObject crosshair;

    [Header("Tutorial config")]
    [SerializeField]
    private CinemachineBrain cinemachineBrain;
    [SerializeField]
    private ScreenFadeController screenFadeController;

    [Header("Player")]
    [SerializeField]
    private Player player;
    [SerializeField]
    private State playerDefaultState;
    [SerializeField]
    private State[] tutorialStates;
    private int playerStateIndex = -1;

    private bool running;
    private bool paused;
    private PlayableDirector director;
    private TutorialEvents tutorialEvents;
    private TutorialEnemiesManager tutorialEnemiesManager;
    private AIZoneController[] zoneControllers;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        UnityEngine.Assertions.Assert.IsNotNull(cinemachineBrain, "ERROR: The TutorialController in gameObject '" + gameObject.name + "' doesn't have a CinemachineBrain assigned!");
        UnityEngine.Assertions.Assert.IsNotNull(screenFadeController, "ERROR: The TutorialController in gameObject '" + gameObject.name + "' doesn't have a ScreenFadeController assigned!");
        director = GetComponent<PlayableDirector>();
        UnityEngine.Assertions.Assert.IsNotNull(director, "ERROR: A PlayableDirector Component could not be found by TutorialController in GameObject " + gameObject.name);
        tutorialEvents = GetComponent<TutorialEvents>();
        UnityEngine.Assertions.Assert.IsNotNull(tutorialEvents, "ERROR: A TutorialEvents Component could not be found by TutorialController in GameObject " + gameObject.name);
        zoneControllers = FindObjectsOfType<AIZoneController>();
        tutorialEnemiesManager = new TutorialEnemiesManager();
        tutorialEvents.SetTutorialEnemiesManager(tutorialEnemiesManager);
    }

    private void Update()
    {
        if (!paused && GameManager.instance.gameIsPaused)
        {
            paused = true;
            if (running)
                director.playableGraph.GetRootPlayable(0).SetSpeed(0);
                // Using director.Pause() allows the cameras to snap back to the default priority settings.
        }
        else if (paused && !GameManager.instance.gameIsPaused)
        {
            paused = false;
            if (running)
                director.playableGraph.GetRootPlayable(0).SetSpeed(1);
            crosshair.SetActive(false);
        }

        if (running)
        {
            if (InputManager.instance.GetPS4OptionsDown())
                EndTutorial();
        }
    }
    #endregion

    #region Public Methods
    public void RequestStartTutorial()
    {
        if (!running)
        {
            playerStateIndex = -1;
            NextPlayerState();
            tutorialEvents.OnTutorialStarted();
            screenFadeController.FadeToTransparent(StartTutorial);
        }
    }

    public void LaunchEvent(int eventIndex)
    {
        tutorialEvents.LaunchEvent(eventIndex);
    }

    public void NextPlayerState()
    {
        player.TransitionToState(tutorialStates[++playerStateIndex]);
    }
    #endregion

    #region Private Methods
    private void OnTutorialEnded()
    {
        running = false;
        director.Stop();
        cinemachineBrain.enabled = false;
        gameObject.SetActive(false);
        
        player.TransitionToState(playerDefaultState);

        foreach (AIZoneController zoneController in zoneControllers)
        {
            zoneController.DestroyAllEnemies();
        }

        tutorialEnemiesManager.ClearEnemies();
        tutorialEvents.OnTutorialEnded();

        GameManager.instance.OnTutorialFinished();
    }

    private void StartTutorial()
    {
        running = true;
        director.Play();
    }

    private void EndTutorial()
    {
        screenFadeController.FadeToOpaque(OnTutorialEnded);
    }
    #endregion
}
