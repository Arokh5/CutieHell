using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICompass : MonoBehaviour {

    #region Fields
    public Transform referenceTransform;
    public CompassIcon compassIconPrefab;
    [Range(60, 180)]
    public float fovDegrees = 90.0f;
    public CompassIconData[] compassIconsData;

    private RectTransform rectTransform;
    private CompassIcon[] compassIcons;
    private Vector2 size;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        if (compassIconsData == null || compassIconsData.Length == 0)
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

        // Create the spawnerIcon images
        compassIcons = new CompassIcon[compassIconsData.Length];
        for (int i = 0; i < compassIcons.Length; ++i)
        {
            compassIcons[i] = Instantiate(compassIconPrefab, gameObject.transform, false);
            compassIcons[i].SetImage(compassIconsData[i].iconSprite);
            compassIcons[i].gameObject.SetActive(false);
        }
    }

    private void Update () {
        UpdateIconsPosition();
	}
    #endregion

    #region Private Methods
    private void UpdateIconsPosition()
    {
        Vector2 referenceForward = new Vector2(referenceTransform.forward.x, referenceTransform.forward.z);

        for (int i = 0; i < compassIconsData.Length; ++i)
        {
            Vector3 referenceToIconV3 = compassIconsData[i].transform.position - referenceTransform.position;
            Vector2 referenceToIcon = new Vector2(referenceToIconV3.x, referenceToIconV3.z);
            float angle = -Vector2.SignedAngle(referenceForward, referenceToIcon);

            if (Mathf.Abs(angle) > 0.5f * fovDegrees)
            {
                compassIcons[i].gameObject.SetActive(false);
            }
            else
            {
                compassIcons[i].gameObject.SetActive(true);
                float xPos = angle / (0.5f * fovDegrees) * 0.5f * size.x;
                compassIcons[i].localPosition = xPos * Vector3.right;
            }
        }
    }
    #endregion
}
