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
    private float verticalOffset;
    private bool front;
    private float heightLimit;
    private float upwardsDistance;
    private float startingDepth;
    private float previousRotation;

    void Start()
    {
        horizontalOffset = mainCamera.pixelWidth * horizontalOffsetPercentage;
        verticalOffset = mainCamera.pixelHeight * bottomOffset;
        startingDepth = 0f;
        /* Get arrow to point up */
        previousRotation = 0f;
    }

    void Update()
    {
        arrow.gameObject.SetActive(true);

        Vector3 screenPos = mainCamera.WorldToScreenPoint(target.position);
        float distance = Vector3.Distance(mainCamera.transform.position, target.transform.position);

        front = screenPos.z >= 0f;

        if (front)
        {
            /* Check if objective marker is out of bounds */
            bool outOfBounds = false;
            if (screenPos.x < horizontalOffset)
            {
                screenPos.x = horizontalOffset;
                if (startingDepth == 0f)
                    startingDepth = screenPos.z;
                outOfBounds = true;
            }
            if (screenPos.x > mainCamera.pixelWidth - horizontalOffset)
            {
                screenPos.x = mainCamera.pixelWidth - horizontalOffset;
                if (startingDepth == 0f)
                    startingDepth = screenPos.z;
                outOfBounds = true;
            }

            /* If not out of bounds just limit vertical offset */
            if (!outOfBounds)
            {
                if (screenPos.y > mainCamera.pixelHeight * topOffset)
                {
                    screenPos.y = mainCamera.pixelHeight * topOffset;
                    outOfBounds = true;
                }

                if (screenPos.y < verticalOffset)
                {
                    screenPos.y = verticalOffset;
                    outOfBounds = true;
                }

                startingDepth = 0f;

                if (!outOfBounds)
                    arrow.gameObject.SetActive(false);
            }
            else
            /* Else, adjust the vertical coordinate of the marker */
            {
                heightLimit = screenPos.y;

                if (heightLimit > mainCamera.pixelHeight * topOffset)
                    heightLimit = mainCamera.pixelHeight * topOffset;

                if (heightLimit < verticalOffset)
                    heightLimit = verticalOffset;

                upwardsDistance = (heightLimit - verticalOffset);

                /* Magical number */
                screenPos.y = verticalOffset + (upwardsDistance * (screenPos.z / (distance * 0.75f)));

            }
        }
        else
        {
            /* If not on front switch horizontal coordinate */
            screenPos.x = mainCamera.pixelWidth - screenPos.x;

            if (screenPos.x < horizontalOffset)
            {
                screenPos.x = horizontalOffset;
            }
            else if (screenPos.x > mainCamera.pixelWidth - horizontalOffset)
            {
                screenPos.x = mainCamera.pixelWidth - horizontalOffset;
            }

            /* Stick vertical coordinate to bottom of screen */
            screenPos.y = verticalOffset;
        }

        RectTransform iconAnchor = gameObject.GetComponent<Image>().rectTransform;
        RectTransform arrowAnchor = arrow.rectTransform;

        iconAnchor.position = screenPos;

        /* Get angle between straight up vector (initial arrow position) and current center of screen */
        Vector3 centerOfScreen = mainCamera.ViewportToScreenPoint(new Vector3(0.5f, 0.5f, 0f));
        Vector3 centerOfIcon = transform.position;
        centerOfIcon.z = 0f;

        float angle = Vector3.SignedAngle(new Vector3(0f, 1f, 0f), centerOfScreen - centerOfIcon, new Vector3(0f, 0f, 1f));
        Vector3 arrowRotation = arrowAnchor.parent.localRotation.eulerAngles;
        arrowRotation.z = angle;
        arrowAnchor.parent.localRotation = Quaternion.Euler(arrowRotation);
        //if (Mathf.Abs(previousRotation - angle) > float.Epsilon)
        //{
        //    arrow.transform.parent.Rotate(new Vector3(0f, 0f, -previousRotation));
        //    arrow.transform.parent.Rotate(new Vector3(0f, 0f, angle));
        //    previousRotation = angle;
        //}
    }
}
