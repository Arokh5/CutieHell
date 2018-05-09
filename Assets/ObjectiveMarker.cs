using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveMarker : MonoBehaviour
{

    public float horizontalOffsetPercentage = 0.05f;
    public float bottomOffset = 0.1f;
    public float topOffset = 0.7f;
    public Transform target;
    [SerializeField]
    private Camera mainCamera;

    private float horizontalOffset;
    private float verticalOffset;
    private bool front;
    private float heightLimit;
    private float upwardsDistance;
    private float startingDepth;

    void Start()
    {
        horizontalOffset = mainCamera.pixelWidth * horizontalOffsetPercentage;
        verticalOffset = mainCamera.pixelHeight * bottomOffset;
        startingDepth = 0f;
    }

    void Update()
    {
        Vector3 screenPos = mainCamera.WorldToScreenPoint(target.position);
        float distance = Vector3.Distance(mainCamera.transform.position, target.transform.position);

        front = screenPos.z >= 0f;

        if (front)
        {
            /* Check if objective marker is out of bounds */
            bool exit = false;
            if (screenPos.x < horizontalOffset)
            {
                screenPos.x = horizontalOffset;
                if (startingDepth == 0f)
                  startingDepth = screenPos.z;
                exit = true;
            }
            if (screenPos.x > mainCamera.pixelWidth - horizontalOffset)
            {
                screenPos.x = mainCamera.pixelWidth - horizontalOffset;
                if (startingDepth == 0f)
                    startingDepth = screenPos.z;
                exit = true;
            }

            /* If not out of bounds just limit vertical offset */
            if (!exit)
            {
                if (screenPos.y > mainCamera.pixelHeight * topOffset)
                    screenPos.y = mainCamera.pixelHeight * topOffset;

                if (screenPos.y < verticalOffset)
                    screenPos.y = verticalOffset;

                startingDepth = 0f;
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

        transform.position = screenPos;
    }
}
