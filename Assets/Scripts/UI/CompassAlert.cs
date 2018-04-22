using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompassAlert : MonoBehaviour {

    #region Fields
    public float blinkFrequency = 1.0f;

    private Image blurImage;
    private Image fixedChildImage;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        if(!blurImage)
            blurImage = GetComponent<Image>();

        if(!fixedChildImage)
            fixedChildImage = GetComponentsInChildren<Image>()[1];

        gameObject.SetActive(false);
    }

    private void Update()
    {
        Blink();
    }
    #endregion

    #region Public Methods
    public void SetColor(Color color)
    {
        fixedChildImage.color = color;
        color.a = blurImage.color.a;
        blurImage.color = color;
    }

    public void TurnOn()
    {
        gameObject.SetActive(true);
    }

    public void TurnOff()
    {
        gameObject.SetActive(false);
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
