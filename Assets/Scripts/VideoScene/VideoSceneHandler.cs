using UnityEngine;
using UnityEngine.SceneManagement;

public class VideoSceneHandler : MonoBehaviour
{
    #region Fields
    [SerializeField]
    private VideoController videoController;
    [SerializeField]
    private string nextSceneName = "TitleScreen";

    [Header("Skip button")]
    [SerializeField]
    private float skipButtonDisplayTime;
    [SerializeField]
    private GameObject skipButton;

    private float skipButtonTimeLeft = 0.0f;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        UnityEngine.Assertions.Assert.IsNotNull(videoController, "ERROR: Video Controller (VideoController) not assigned for VideoSceneHandler script in GameObject '" + gameObject.name + "'!");
        UnityEngine.Assertions.Assert.IsNotNull(skipButton, "ERROR: Skip Button (GameObject) not assigned for VideoSceneHandler script in GameObject '" + gameObject.name + "'!");
        skipButton.SetActive(false);
    }

    private void Start()
    {
        videoController.StartVideoPlayback(ChangeScene);
    }

    private void Update()
    {
        if (skipButton.activeSelf)
        {
            if (InputManager.instance.GetXButtonDown())
            {
                ChangeScene();
            }
            else if (InputManager.instance.GetSquareButtonDown()
                || InputManager.instance.GetTriangleButtonDown()
                || InputManager.instance.GetOButtonDown())
            {
                skipButtonTimeLeft = skipButtonDisplayTime;
            }
        }
        else
        {
            if (InputManager.instance.GetXButtonDown()
                || InputManager.instance.GetSquareButtonDown()
                || InputManager.instance.GetTriangleButtonDown()
                || InputManager.instance.GetOButtonDown())
            {
                skipButton.SetActive(true);
                skipButtonTimeLeft = skipButtonDisplayTime;
            }
        }


        if (skipButtonTimeLeft > 0.0f)
        {
            skipButtonTimeLeft -= Time.deltaTime;

            if (skipButtonTimeLeft < 0.0f)
            {
                skipButton.SetActive(false);
            }
        }
    }
    #endregion

    #region Private Methods
    private void ChangeScene()
    {
        SceneManager.LoadScene(nextSceneName, LoadSceneMode.Single);
    }


    #endregion
}
