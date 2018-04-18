using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompassAlert : MonoBehaviour {

    #region Fields
    public float blinkFrequency = 1.0f;

    private Image blurImage;
    [SerializeField]
    private int onRequests = 0;
    private float elapsedTime;

    public bool on;
    public bool off;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        if(!blurImage)
            blurImage = GetComponent<Image>();

        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (on)
        {
            on = false;
            RequestOn();
        }
        if (off)
        {
            off = false;
            RequestOff();
        }

        if (onRequests > 0)
        {
            Blink();
            elapsedTime += Time.deltaTime;
        }
    }
    #endregion

    #region Public Methods
    public void RequestOn()
    {
        if (onRequests == 0)
        {
            elapsedTime = 0.0f;
            gameObject.SetActive(true);
        }
        ++onRequests;
    }

    public void RequestOff()
    {
        --onRequests;
        if (onRequests == 0)
        {
            gameObject.SetActive(false);
        }
    }
    #endregion

    #region Private Methods
    private void Blink()
    {
        float sinValue = Mathf.Abs(Mathf.Sin(Time.time * (blinkFrequency / 2.0f) * (2.0f * Mathf.PI)));
        Color color = blurImage.color;
        color.a = sinValue;
        blurImage.color = color;
    }
    #endregion
}
