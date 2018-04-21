using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialProgressBar : MonoBehaviour
{
    public Image loadingBar;
    public Transform clockHandRotation;
    [SerializeField] private float currentNormalizedAmount;

    // Use this for initialization
    void Start()
    {
        currentNormalizedAmount = 0.0f;

        loadingBar.fillAmount = 1f - currentNormalizedAmount;
    }

    public void SetNormalizedAmount(float normalizedAmount)
    {
        currentNormalizedAmount = normalizedAmount;

        if (currentNormalizedAmount < 0.0f)
            currentNormalizedAmount = 0.0f;

        if (currentNormalizedAmount > 1.0f)
            currentNormalizedAmount = 1.0f;

        float degreesRotated = 360f * currentNormalizedAmount;

        Vector3 currentRotation = clockHandRotation.localEulerAngles;
        currentRotation.z = -degreesRotated;
        clockHandRotation.localEulerAngles = currentRotation;
        loadingBar.fillAmount = 1f - currentNormalizedAmount;
    }
}
