using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICompass : MonoBehaviour {

    #region Fields
    public Transform referenceTransform;
    [Range(60, 180)]
    public float fovDegrees = 90.0f;
    public CompassIcon[] compassIcons;

    private RectTransform rectTransform;
    private Vector2 size;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        if (compassIcons == null || compassIcons.Length == 0)
        {
            gameObject.SetActive(false);
            Debug.LogWarning("WARNING: The UICompass in gameObject '" + gameObject.name + "' doesn't have any compassIcons assigned so it will be deactivated!");
        }

        if(rectTransform == null)
        {
            rectTransform = GetComponent<RectTransform>();
            UnityEngine.Assertions.Assert.IsNotNull(rectTransform, "ERROR: The UICompass in gameObject '" + gameObject.name + "' doesn't have a RectTransform attached!");
        }
    }

    private void Start()
    {
        size = rectTransform.sizeDelta;
    }

    private void Update () {
		
	}
    #endregion

    #region Public Methods

    #endregion
}
