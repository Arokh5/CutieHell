using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsScroll : MonoBehaviour
{
    [Header("Elements setup")]
    [SerializeField]
    private RectTransform credits;
    [SerializeField]
    private ScreenFadeController fader;
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip backClip;

    [Header("Configuration")]
    [SerializeField]
    private float scrollLimit = 3000.0f;
    [SerializeField]
    private int normalScrollSpeed = 50;
    [SerializeField]
    private int forcedScrollSpeed = 200;

	void Start ()
    {
        credits.localPosition = Vector3.zero;
	}

	void Update ()
    {
        Move();
        GoBack();
    }

    private void Move()
    {
        Vector3 nextPos = credits.localPosition;
        if (InputManager.instance.GetPadUp() || InputManager.instance.GetLeftStickUp() || InputManager.instance.GetRightStickUp())
        {
            nextPos.y += forcedScrollSpeed * Time.deltaTime;
        }
        else if (InputManager.instance.GetPadDown() || InputManager.instance.GetLeftStickDown() || InputManager.instance.GetRightStickDown())
        {
            nextPos.y -= forcedScrollSpeed * Time.deltaTime;
        }
        else
        {
            nextPos.y += normalScrollSpeed * Time.deltaTime;
        }

        if (nextPos.y < 0.0f)
        {
            nextPos.y = 0.0f;
        }

        if (nextPos.y > scrollLimit)
        {
            nextPos.y = 0.0f;
        }
        credits.localPosition = nextPos;
    }

    private void GoBack()
    {
        if (InputManager.instance.GetButtonDown(ControllerButton.CIRCLE))
        {
            audioSource.PlayOneShot(backClip);
            fader.FadeToOpaque(0.5f, LoadTitleScreen);
        }
    }

    private void LoadTitleScreen()
    {
        SceneManager.LoadScene("TitleScreen");
    }
}
