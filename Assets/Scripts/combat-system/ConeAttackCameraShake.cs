using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class ConeAttackCameraShake : MonoBehaviour {

    private bool shaked;
    private float timeToShake;

    private void OnEnable()
    {
        shaked = false;
        timeToShake = 0.5f;
    }

    private void Update()
    {
        timeToShake -= Time.deltaTime;
        if(timeToShake <= 0.0f && !shaked)
        {
            shaked = true;
            CameraShaker.Instance.ShakeOnce(0.3f, 4.5f, 0.1f, 0.7f);
        }
    }
}
