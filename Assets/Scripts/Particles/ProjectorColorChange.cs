using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectorColorChange : MonoBehaviour {

    [SerializeField]
    private bool pingPong;
    [SerializeField]
    private float timer;
    [Header("Default colors")]
    [SerializeField]
    private Color startColor;
    [SerializeField]
    private Color endColor;
    [Header("Alternate colors")]
    [SerializeField]
    private Color alternateStartColor;
    [SerializeField]
    private Color alternateEndColor;

    private Color referenceColor;

    private float timeElapsed;
    private Projector projector;
    private Color colorDiference;
    private float speed;

	void Start ()
    {
        projector = this.GetComponent<Projector>();
        colorDiference = endColor - startColor;
        if (pingPong)
        {
            speed = Mathf.PI;
        }
        else
        {
            speed = Mathf.PI / 2.0f;
        }
        speed /= timer;
	}

	void Update ()
    {
        timeElapsed += Time.deltaTime * speed;
        if (timeElapsed >= Mathf.PI)
            timeElapsed = 0.0f;
        float sin = Mathf.Sin(timeElapsed);
        Color newColor = new Color(
            referenceColor.r + colorDiference.r * sin,
            referenceColor.g + colorDiference.g * sin,
            referenceColor.b + colorDiference.b * sin,
            projector.material.GetColor("_TintColor").a
            );
        projector.material.SetColor("_TintColor", newColor);
	}

    public void SwitchToDefaultColor()
    {
        colorDiference = endColor - startColor;
        referenceColor = startColor;
    }

    public void SwitchToAlternateColor()
    {
        colorDiference = alternateEndColor - alternateStartColor;
        referenceColor = alternateStartColor;
    }
}
