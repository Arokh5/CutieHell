using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;

public class TutorialController : MonoBehaviour
{
    #region Fields
    private bool running;

    [SerializeField]
    private CinemachineBrain cinemachineBrain;
    [SerializeField]
    private ScreenFadeController screenFadeController;

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

        tutorialEvents = new TutorialEvents();
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
            StartTutorial();
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
        cinemachineBrain.enabled = false;
        gameObject.SetActive(false);
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
