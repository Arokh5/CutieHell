using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioSceneManager : MonoBehaviour
{
    #region Fields
    [SerializeField]
    private ControllerButton menuButton = ControllerButton.X;
    [SerializeField]
    [Range(1, 5)]
    private int pushCount = 2;
    [SerializeField]
    private float audioFadeDuration = 2.0f;

    private int currentPushCount = 0;
    private bool fading = false;
    private float fadeTimeLeft = 0.0f;
    private AudioSource audioSource;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        UnityEngine.Assertions.Assert.IsNotNull(audioSource, "ERROR: Audio Source (AudioSource) could not be found by AudioSceneManager script in GameObject " + gameObject.name);
    }

    private void Update()
    {
        if (InputManager.instance.GetButtonDown(menuButton))
        {
            ++currentPushCount;
            if (currentPushCount >= pushCount && !fading)
            {
                fading = true;
                fadeTimeLeft = audioFadeDuration;
            }
        }

        if (fading)
        {
            Fade();
        }
    }
    #endregion

    #region Private Methods
    private void Fade()
    {
        fadeTimeLeft -= Time.deltaTime;
        if (fadeTimeLeft < 0.0f)
        {
            fadeTimeLeft = 0.0f;
        }

        float volume = fadeTimeLeft / audioFadeDuration;
        audioSource.volume = volume;

        if (fadeTimeLeft <= 0.0f)
        {
            GoToTitleScreen();
        }
    }

    private void GoToTitleScreen()
    {
        SceneManager.LoadScene("TitleScreen");
    }
    #endregion
}
