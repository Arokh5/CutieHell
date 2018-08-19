using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsScroll : MonoBehaviour {

    [SerializeField]
    private RectTransform credits;

	void Start ()
    {
        credits.localPosition = Vector3.zero;
	}

	void Update ()
    {
        Move();
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
}
