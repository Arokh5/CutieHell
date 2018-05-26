using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TutorialController : MonoBehaviour
{
    #region Fields
    private bool running;

    [SerializeField]
    private CinemachineBrain cinemachineBrain;
    [SerializeField]
    private ScreenFadeController screenFadeController;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        UnityEngine.Assertions.Assert.IsNotNull(cinemachineBrain, "ERROR: The TutorialController in gameObject '" + gameObject.name + "' doesn't have a CinemachineBrain assigned!");
        UnityEngine.Assertions.Assert.IsNotNull(screenFadeController, "ERROR: The TutorialController in gameObject '" + gameObject.name + "' doesn't have a ScreenFadeController assigned!");
    }

    private void Update()
    {
        if (running && InputManager.instance.GetPS4OptionsDown())
            EndTutorial();
    }
    #endregion

    #region Public Methods
    public void StartTurotial()
    {
        running = true;
    }

    public void EndTutorial()
    {
        screenFadeController.FadeToOpaque(OnTutorialEnded);
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
    #endregion
}
