using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialProgressBar : MonoBehaviour
{
    public Image loadingBar;
    public Text textProgress;
    [SerializeField] private float currentNormalizedAmount;

    // Use this for initialization
    void Start()
    {
        currentNormalizedAmount = 0.0f;

        textProgress.text = ((int)currentNormalizedAmount).ToString() + "%";
        loadingBar.fillAmount = currentNormalizedAmount / 100;
    }

    public void SetNormalizedAmount(float normalizedAmount)
    {
        currentNormalizedAmount = normalizedAmount;

        if (currentNormalizedAmount < 0.0f)
            currentNormalizedAmount = 0.0f;

        if (currentNormalizedAmount > 1.0f)
            currentNormalizedAmount = 1.0f;

        textProgress.text = ((int)(100 * currentNormalizedAmount)).ToString() + "%";
        loadingBar.fillAmount = currentNormalizedAmount;
    }
}
