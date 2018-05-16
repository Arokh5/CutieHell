using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveMarker : MonoBehaviour
{
    public float horizontalOffsetPercentage = 0.05f;
    public float bottomOffset = 0.1f;
    public float topOffset = 0.7f;
    public Transform target;
    public Image arrow;
    [SerializeField]
    private Camera mainCamera;

    private float horizontalOffset;
    private float hAngle;

    void Start()
    {
        horizontalOffset = mainCamera.pixelWidth * horizontalOffsetPercentage;

        /* Get arrow to point up */
        float radVFov = mainCamera.fieldOfView * Mathf.Deg2Rad;
        float radHFov = 2 * Mathf.Atan(Mathf.Tan(radVFov / 2) * mainCamera.aspect);
        hAngle = Mathf.Rad2Deg * Mathf.Atan((Mathf.Tan(0.5f * radHFov) * (1 - horizontalOffsetPercentage)));
    }

    void Update()
    {
        arrow.gameObject.SetActive(true);

        Vector2 screenPosition;
        Vector3 iconPosition = Vector3.zero;
        Vector3 targetPos = target.transform.position;
        Vector3 cameraForward = mainCamera.transform.forward;
        cameraForward.y = 0;
        Vector3 cameraToTarget = targetPos - mainCamera.transform.position;
        cameraToTarget.y = 0;
        float tAngle = Vector3.SignedAngle(cameraForward, cameraToTarget, Vector3.up);

        bool behind = tAngle < -90 || tAngle > 90;
        bool front = tAngle > -hAngle && tAngle < hAngle;
        bool left = tAngle >= -90 && tAngle <= -hAngle;
        bool right = tAngle >= hAngle && tAngle <= 90;

        if (behind)
        {
            iconPosition.y = bottomOffset * mainCamera.pixelHeight;
            float halfRange = 0.5f * (mainCamera.pixelWidth - 2 * horizontalOffset);
            if (tAngle < -90)
                iconPosition.x = horizontalOffset + (tAngle - -90) / -90 * halfRange;
            else
                iconPosition.x = horizontalOffset + halfRange + (180 - tAngle) / 90 * halfRange;
        }
        else
        {
            screenPosition = mainCamera.projectionMatrix.MultiplyPoint(mainCamera.worldToCameraMatrix.MultiplyPoint(targetPos));
            screenPosition.x = screenPosition.x * 0.5f + 0.5f;
            screenPosition.y = screenPosition.y * 0.5f + 0.5f;

            if (front)
            {
                if (screenPosition.y < topOffset && screenPosition.y > bottomOffset)
                    arrow.gameObject.SetActive(false);

                iconPosition.y = mainCamera.pixelHeight * Mathf.Clamp(screenPosition.y, bottomOffset, topOffset);
                iconPosition.x = mainCamera.pixelWidth * Mathf.Clamp(screenPosition.x, horizontalOffsetPercentage, 1 - horizontalOffsetPercentage);
            }
            else 
            {
                float upwardsFactor;
                if (left)
                {
                    iconPosition.x = horizontalOffset;
                    upwardsFactor = (90 - -tAngle) / (90 - hAngle);
                }
                else
                {
                    iconPosition.x = mainCamera.pixelWidth - horizontalOffset;
                    upwardsFactor = (90 - tAngle) / (90 - hAngle);
                }

                float screenUpwardsFactor;

                if (screenPosition.y < topOffset)
                {
                    if (screenPosition.y > bottomOffset)
                    {
                        screenUpwardsFactor = bottomOffset + upwardsFactor * (screenPosition.y - bottomOffset);
                    }
                    else
                    {
                        screenUpwardsFactor = bottomOffset;
                    }
                }
                else
                {
                    screenUpwardsFactor = bottomOffset + upwardsFactor * (topOffset - bottomOffset);
                }

                iconPosition.y = mainCamera.pixelHeight * screenUpwardsFactor;

            }

        }

        RectTransform iconTransform = gameObject.GetComponent<Image>().rectTransform;
        RectTransform arrowTransform = arrow.rectTransform;

        iconTransform.position = iconPosition;

        /* Get angle between straight up vector (initial arrow position) and current center of screen */
        Vector3 centerOfScreen = mainCamera.ViewportToScreenPoint(new Vector3(0.5f, 0.5f, 0f));
        Vector3 centerOfIcon = transform.position;
        centerOfIcon.z = 0f;

        float angle = Vector3.SignedAngle(new Vector3(0f, 1f, 0f), centerOfScreen - centerOfIcon, new Vector3(0f, 0f, 1f));
        Vector3 arrowRotation = arrowTransform.parent.localRotation.eulerAngles;
        arrowRotation.z = angle;
        arrowTransform.parent.localRotation = Quaternion.Euler(arrowRotation);
    }
}
