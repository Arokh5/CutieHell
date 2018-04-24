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
    public RectTransform compassIconsParent;
    [Range(60, 180)]
    public float fovDegrees = 90.0f;
    public float maxDistanceBeforeSnap = 5.0f;
    [Header("Compass Icons config")]
    public float alertDuration = 2.0f;
    [Range(1.0f, 5.0f)]
    public float alertFrequency = 2.0f;
    [Range(0.0f, 0.5f)]
    public float blurGrowthPercent = 0.2f;
    [Range(0.0f, 0.5f)]
    public float imageShrinkPercent = 0.2f;

    [Header("Monument side indicator")]
    [SerializeField]
    private MonumentIndicator monumentIndicator;

    private RectTransform rectTransform;
    private Dictionary<CompassIconOwner, CompassIcon> compassIcons = new Dictionary<CompassIconOwner, CompassIcon>();
    private Vector2 size;
    private bool leftLightOn = false;
    private int leftLightHighestImportance = int.MinValue;
    private Color leftLightColor = Color.white;
    private bool rightLightOn = false;
    private int rightLightHighestImportance = int.MinValue;
    private Color rightLightColor = Color.white;

    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        if(rectTransform == null)
        {
            rectTransform = GetComponent<RectTransform>();
            UnityEngine.Assertions.Assert.IsNotNull(rectTransform, "ERROR: The UICompass in gameObject '" + gameObject.name + "' doesn't have a RectTransform attached!");
        }

        UnityEngine.Assertions.Assert.IsNotNull(monumentIndicator, "ERROR: The UICompass in gameObject '" + gameObject.name + "' doesn't have a monumentIndicator assigned!");

        size = rectTransform.sizeDelta;
    }

    private void Update()
    {
        leftLightOn = false;
        leftLightHighestImportance = int.MinValue;
        rightLightOn = false;
        rightLightHighestImportance = int.MinValue;
        UpdateIcons();
        UpdateLights();
    }
    #endregion

    #region Public Methods
    public void Register(CompassIconOwner owner)
    {
        CompassIcon newIcon = Instantiate(compassIconPrefab, compassIconsParent, false);
        newIcon.alertDuration = alertDuration;
        newIcon.alertFrequency = alertFrequency;
        newIcon.blurGrowthPercent = blurGrowthPercent;
        newIcon.imageShrinkPercent = imageShrinkPercent;
        newIcon.SetBackground(owner.backgroundSprite);
        newIcon.SetLogo(owner.logoSprite);
        newIcon.TurnOff();
        compassIcons[owner] = newIcon;
    }

    public void SetCompassIconFill(CompassIconOwner owner, float normalizedFillAmount)
    {
        CompassIcon compassIcon = compassIcons[owner];
        compassIcon.SetFill(normalizedFillAmount);

        if (owner.GetComponent<Monument>())
        {
            monumentIndicator.SetFill(normalizedFillAmount);
        }
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
            if (referenceToIcon.sqrMagnitude < maxDistanceBeforeSnap * maxDistanceBeforeSnap)
            {
                compassIcon.TurnOn();
                compassIcon.localPosition = Vector3.zero;
                compassIcon.rectTransform.SetSiblingIndex(compassIcons.Count - 1);
            }
            else
            { 
                float angle = -Vector2.SignedAngle(referenceForward, referenceToIcon);

                if (Mathf.Abs(angle) > 0.5f * fovDegrees)
                {
                    compassIcon.TurnOff();
                    if (compassIcon.alertTimeLeft > 0)
                    {
                        if (angle < 0)
                        {
                            leftLightOn = true;
                            if (owner.iconImportance > leftLightHighestImportance)
                            {
                                leftLightHighestImportance = owner.iconImportance;
                                leftLightColor = owner.alertColor;
                            }

                        }
                        else
                        {
                            rightLightOn = true;
                            if (owner.iconImportance > rightLightHighestImportance)
                            {
                                rightLightHighestImportance = owner.iconImportance;
                                rightLightColor = owner.alertColor;
                            }
                        }
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
    }

    private void UpdateLights()
    {
        if (leftLightOn)
        {
            leftLight.SetColor(leftLightColor);
            leftLight.TurnOn();
        }
        else
            leftLight.TurnOff();

        if (rightLightOn)
        {
            rightLight.SetColor(rightLightColor);
            rightLight.TurnOn();
        }
        else
            rightLight.TurnOff();
    }
    #endregion
}
