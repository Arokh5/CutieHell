using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialProgressBar : MonoBehaviour
{
    public Transform LoadingBar;
    public Transform TextProgress;
    [SerializeField] private float currentAmount;

    // Use this for initialization
    void Start()
    {
        currentAmount = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentAmount < 0.0f)
            currentAmount = 0.0f;

        if (currentAmount > 100.0f)
            currentAmount = 100.0f;

        TextProgress.GetComponent<Text>().text = ((int)currentAmount).ToString() + "%";
        LoadingBar.GetComponent<Image>().fillAmount = currentAmount / 100;
    }

    public void SetCurrentAmount(float amount)
    {
        currentAmount = amount;
    }
}
