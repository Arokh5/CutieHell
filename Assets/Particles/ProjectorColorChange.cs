using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectorColorChange : MonoBehaviour {

    [SerializeField]
    private bool pingPong;
    [SerializeField]
    private float timer;
    [SerializeField]
    private Color startColor;
    [SerializeField]
    private Color endColor;

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
            startColor.r + colorDiference.r * sin,
            startColor.g + colorDiference.g * sin,
            startColor.b + colorDiference.b * sin,
            projector.material.GetColor("_TintColor").a
            );
        projector.material.SetColor("_TintColor", newColor);
	}
}
