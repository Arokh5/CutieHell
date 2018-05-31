using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveProgressFiller : MonoBehaviour
{

    public Image progressFiller;
    [SerializeField]
    private float currentNormalizedAmount;
    [SerializeField]
    private GameObject clockHand;

    // Use this for initialization
    void Start()
    {
        currentNormalizedAmount = 0.0f;
        progressFiller.fillAmount = currentNormalizedAmount;
    }

    public void SetNormalizedAmount(float normalizedAmount)
    {
        currentNormalizedAmount = normalizedAmount;

        if (currentNormalizedAmount < 0.0f)
            currentNormalizedAmount = 0.0f;

        if (currentNormalizedAmount > 1.0f)
            currentNormalizedAmount = 1.0f;

        progressFiller.fillAmount = currentNormalizedAmount;
        Vector3 eulerAngles = clockHand.transform.localEulerAngles;
        eulerAngles.z = -360 * currentNormalizedAmount;
        clockHand.transform.localEulerAngles = eulerAngles;
    }
}
