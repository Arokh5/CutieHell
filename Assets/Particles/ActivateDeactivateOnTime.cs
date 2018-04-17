using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateDeactivateOnTime : MonoBehaviour {

    [SerializeField]
    private float startSizeOnActivate;
    [SerializeField]
    private float endSizeOnActivate;
    [SerializeField]
    private float startSizeOnDeactivate;
    [SerializeField]
    private float endSizeOnDeactivate;
    [SerializeField]
    private Color startColorOnActivate;
    [SerializeField]
    private Color endColorOnActivate;
    [SerializeField]
    private Color startColorOnDeactivate;
    [SerializeField]
    private Color endColorOnDeactivate;
    [SerializeField]
    private float timeToActivate;
    [SerializeField]
    private float timeToDeactivate;
    [SerializeField]
    private float timeToFadeIn;
    [SerializeField]
    private float timeToFadeOut;

    private float timer, timeOnFade,sizeInDiference, sizeOutDiference;
    private Projector projector;
    private ProjectorStates state;
    private Color startColorDiference, endColorDiference;

    enum ProjectorStates
    {
        DEACTIVATED,
        ACTIVATING,
        ACTIVE,
        DEACTIVATING,
        DEACTIVATED2,
        DEACTIVATED3
    }

    void OnEnable () {
        projector = this.GetComponent<Projector>();
        Color newColor = projector.material.GetColor("_TintColor");
        newColor = startColorOnActivate;
        projector.material.SetColor("_TintColor", newColor);
        projector.orthographicSize = startSizeOnActivate;
        timer = timeOnFade = 0.0f;
        state = ProjectorStates.DEACTIVATED;
        startColorDiference = endColorOnActivate - startColorOnActivate;
        endColorDiference = endColorOnDeactivate - startColorOnDeactivate;
        sizeInDiference = endSizeOnActivate - startSizeOnActivate;
        sizeOutDiference = endSizeOnDeactivate - startSizeOnDeactivate;
    }

	void Update ()
    {
        timer += Time.deltaTime;
        switch (state)
        {
            case ProjectorStates.DEACTIVATED:
                if (timer >= timeToActivate)
                {
                    timer = 0.0f;
                    ++state;
                }
                break;
            case ProjectorStates.ACTIVATING:
                if (timer >= timeToFadeIn)
                {
                    timer = 0.0f;
                    ++state;
                }
                else
                {
                    Color newColor = projector.material.GetColor("_TintColor");
                    newColor.a += startColorDiference.a * Time.deltaTime / timeToFadeIn;
                    projector.material.SetColor("_TintColor", newColor);
                    projector.orthographicSize += sizeInDiference * Time.deltaTime / timeToFadeIn;
                }
                break;
            case ProjectorStates.ACTIVE:
                if (timer >= timeToDeactivate)
                {
                    timer = 0.0f;
                    ++state;
                }
                break;
            case ProjectorStates.DEACTIVATING:
                if (timer >= timeToFadeOut)
                {
                    timer = 0.0f;
                    ++state;
                }
                else
                {
                    Color newColor = projector.material.GetColor("_TintColor");
                    newColor.a += endColorDiference.a * Time.deltaTime / timeToFadeOut;
                    newColor.a = Mathf.Clamp(newColor.a, 0.0f, 1.0f);
                    projector.material.SetColor("_TintColor", newColor);
                    projector.orthographicSize += sizeOutDiference * Time.deltaTime / timeToFadeOut;
                }
                break;
            case ProjectorStates.DEACTIVATED2:
                {
                    Color newColor = new Color(1.0f,1.0f,1.0f,0.0f);
                    projector.material.SetColor("_TintColor", newColor);
                    ++state;
                }
                break;
            case ProjectorStates.DEACTIVATED3:
                break;
            default:
                break;
        }
	}
}
