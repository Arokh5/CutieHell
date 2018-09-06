using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideWhenPlayerDies : MonoBehaviour {

    private Vector2 initialPos;
    private Vector2 finalPos;
    public bool hide;
    public bool hideToTheLeft;
    public float distanceToMove;
    private RectTransform rectTransform;


	void Awake () {
        rectTransform = this.GetComponent<RectTransform>();
        initialPos = rectTransform.anchoredPosition;
        Debug.Log(rectTransform.anchoredPosition + this.transform.name);
        if (hideToTheLeft)
        {
            finalPos = initialPos + Vector2.left * distanceToMove;
        }
        else
        {
            finalPos = initialPos + Vector2.right * distanceToMove;
        }
	}

	void Update ()
    {
        if (hide)
        {
            Hide();
        }
        else
        {
            Show();
        }
	}

    public void Hide()
    {
        rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, finalPos, 0.3f);
    }

    public void Show()
    {
        rectTransform.anchoredPosition = Vector3.Lerp(rectTransform.anchoredPosition, initialPos, 0.3f);
    }
}
