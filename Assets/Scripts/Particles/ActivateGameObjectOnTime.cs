using System.Collections;
using System.Collections.Generic;
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
        if (objectToActivate && !objectToActivate.activeSelf)
        {
            if (timeElapsed < timeToActivate) timeElapsed += Time.deltaTime;
            if (timeToActivate < timeElapsed)
            {
                objectToActivate.SetActive(true);
            }
        }
	}
}
