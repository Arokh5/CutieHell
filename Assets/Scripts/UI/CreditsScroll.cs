using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CreditsScroll : MonoBehaviour {

    [SerializeField]
    private RectTransform credits;
    [SerializeField]
    private ScreenFadeController fader;

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
            nextPos.y += 200 * Time.deltaTime;
        }
        else if (InputManager.instance.GetPadDown() || InputManager.instance.GetLeftStickDown() || InputManager.instance.GetRightStickDown())
        {
            nextPos.y -= 200 * Time.deltaTime;
        }
        else
        {
            nextPos.y += 50 * Time.deltaTime;
        }
        credits.localPosition = nextPos;
    }

    private void GoBack()
    {
        if (InputManager.instance.GetButtonDown(ControllerButton.X) ||
            InputManager.instance.GetButtonDown(ControllerButton.CIRCLE) ||
            InputManager.instance.GetButtonDown(ControllerButton.SQUARE) ||
            InputManager.instance.GetButtonDown(ControllerButton.TRIANGLE))
        {
            fader.FadeToOpaque(0.5f, LoadTitleScreen);
        }
    }

    private void LoadTitleScreen()
    {
        SceneManager.LoadScene("TitleScreen");
    }
}
