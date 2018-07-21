using UnityEngine;

public class ActivateGameObjectOnTime : MonoBehaviour {

    public GameObject objectToActivate;

    public float timeToActivate;
    private float timeElapsed;

	void OnEnable () {
        timeElapsed = 0.0f;
	}
	
	void Update ()
    {
        if (objectToActivate)
        {
            if (timeElapsed < timeToActivate)
                timeElapsed += Time.deltaTime;
            else
            {
                objectToActivate.SetActive(true);
                objectToActivate = null;
            }
        }
	}
}
