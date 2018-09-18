using UnityEngine;
using UnityEngine.SceneManagement;

public class VideoSceneHandler : MonoBehaviour
{
    #region Fields
    [SerializeField]
    private VideoController videoController;
    [SerializeField]
    private string nextSceneName = "TitleScreen";
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        UnityEngine.Assertions.Assert.IsNotNull(videoController, "ERROR: Video Controller (VideoController) not assigned for VideoSceneHandler script in GameObject '" + gameObject.name + "'!");
    }

    private void Start()
    {
        videoController.StartVideoPlayback(ChangeScene);
    }
    #endregion

    #region Private Methods
    private void ChangeScene()
    {
        SceneManager.LoadScene(nextSceneName, LoadSceneMode.Single);
    }
    #endregion
}
