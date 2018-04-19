using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICompass : MonoBehaviour
{
    #region Fields
    public Transform referenceTransform;
    public CompassAlert leftLight;
    public CompassAlert rightLight;
    public CompassIcon compassIconPrefab;
    [Range(60, 180)]
    public float fovDegrees = 90.0f;
    [Header("Compass Icons config")]
    public float alertDuration = 2.0f;
    [Range(1.0f, 5.0f)]
    public float alertFrequency = 2.0f;
    [Range(0.0f, 0.5f)]
    public float blurGrowthPercent = 0.2f;
    [Range(0.0f, 0.5f)]
    public float imageShrinkPercent = 0.2f;

    private RectTransform rectTransform;
    private Dictionary<CompassIconOwner, CompassIcon> compassIcons = new Dictionary<CompassIconOwner, CompassIcon>();
    private Vector2 size;
    private int leftLightRequests = 0;
    private int rightLightRequests = 0;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        if(rectTransform == null)
        {
            rectTransform = GetComponent<RectTransform>();
            UnityEngine.Assertions.Assert.IsNotNull(rectTransform, "ERROR: The UICompass in gameObject '" + gameObject.name + "' doesn't have a RectTransform attached!");
        }

        size = rectTransform.sizeDelta;
    }

    private void Update()
    {
        leftLightRequests = 0;
        rightLightRequests = 0;
        UpdateIcons();
        UpdateLights();
    }
    #endregion

    #region Public Methods
    public void Register(CompassIconOwner owner)
    {
        CompassIcon newIcon = Instantiate(compassIconPrefab, gameObject.transform, false);
        newIcon.alertDuration = alertDuration;
        newIcon.alertFrequency = alertFrequency;
        newIcon.blurGrowthPercent = blurGrowthPercent;
        newIcon.imageShrinkPercent = imageShrinkPercent;
        newIcon.SetImage(owner.iconSprite);
        newIcon.TurnOff();
        compassIcons[owner] = newIcon;
    }

    public void SetAlertForIcon(CompassIconOwner owner)
    {
        compassIcons[owner].SetAlert();
    }
    #endregion

    #region Private Methods
    private void UpdateIcons()
    {
        Vector2 referenceForward = new Vector2(referenceTransform.forward.x, referenceTransform.forward.z);

        foreach (CompassIconOwner owner in compassIcons.Keys)
        {
            CompassIcon compassIcon = compassIcons[owner];
            Vector3 referenceToIconV3 = owner.transform.position - referenceTransform.position;
            Vector2 referenceToIcon = new Vector2(referenceToIconV3.x, referenceToIconV3.z);
            float angle = -Vector2.SignedAngle(referenceForward, referenceToIcon);

            if (Mathf.Abs(angle) > 0.5f * fovDegrees)
            {
                compassIcon.TurnOff();
                if (compassIcon.alertTimeLeft > 0)
                {
                    if (angle < 0)
                        ++leftLightRequests;
                    else
                        ++rightLightRequests;
                }
            }
            else
            {
                compassIcon.TurnOn();
                float xPos = angle / (0.5f * fovDegrees) * 0.5f * size.x;
                compassIcon.localPosition = xPos * Vector3.right;
            }
        }
    }

    private void UpdateLights()
    {
        if (leftLightRequests > 0)
            leftLight.TurnOn();
        else
            leftLight.TurnOff();

        if (rightLightRequests > 0)
            rightLight.TurnOn();
        else
            rightLight.TurnOff();
    }
    #endregion
}
