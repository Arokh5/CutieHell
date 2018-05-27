using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;

public class TutorialController : MonoBehaviour
{
    #region Fields
    [Header("UI objects")]
    [SerializeField]
    private GameObject tutObjectiveMarker;
    [SerializeField]
    private GameObject tutObjectiveIcon;
    [SerializeField]
    private GameObject[] bannersAndMarkers;
    [SerializeField]
    private GameObject crosshair;

    [Header("Tutorial config")]
    [SerializeField]
    private CinemachineBrain cinemachineBrain;
    [SerializeField]
    private ScreenFadeController screenFadeController;

    private bool running;
    private PlayableDirector director;
    private TutorialEvents tutorialEvents;
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
    }

    private void Update()
    {
        if (running && InputManager.instance.GetPS4OptionsDown())
            EndTutorial();
    }
    #endregion

    #region Public Methods
    public void RequestStartTutorial()
    {
        if (!running)
        {
            foreach (GameObject go in bannersAndMarkers)
                go.SetActive(false);

            tutObjectiveIcon.SetActive(true);
            tutObjectiveMarker.SetActive(false);
            crosshair.SetActive(false);

            screenFadeController.FadeToTransparent(StartTutorial);
        }
    }

    public void LaunchEvent(int eventIndex)
    {
        tutorialEvents.LaunchEvent(eventIndex);
    }
    #endregion

    #region Private Methods
    private void OnTutorialEnded()
    {
        running = false;
        director.Stop();
        cinemachineBrain.enabled = false;
        gameObject.SetActive(false);

        foreach (GameObject go in bannersAndMarkers)
            go.SetActive(true);

        tutObjectiveIcon.SetActive(false);
        tutObjectiveMarker.SetActive(false);
        crosshair.SetActive(true);

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
