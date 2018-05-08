using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveMarker : MonoBehaviour
{

    public float horizontalOffsetPercentage = 0.15f;
    public float verticalOffsetPercentage = 0.25f;
    public Transform target;
    [SerializeField]
    private Camera mainCamera;

    private float horizontalOffset;
    private float verticalOffset;
    private bool front;
    private float exitHeight;
    private float downwardsDistance;
    private float startingDepth;

    void Start()
    {
        horizontalOffset = (mainCamera.pixelWidth / 2) * horizontalOffsetPercentage;
        verticalOffset = (mainCamera.pixelHeight / 2) * verticalOffsetPercentage;
        exitHeight = 0f;
    }

    void Update()
    {
        Vector3 screenPos = transform.position;
        screenPos = mainCamera.WorldToScreenPoint(target.position);

        front = screenPos.z >= 0f;
        
        if (front)
        {
            if (screenPos.x < horizontalOffset)
            {
                if (exitHeight == 0f)
                {
                    exitHeight = mainCamera.pixelHeight * 0.5f;
                    downwardsDistance = (exitHeight - verticalOffset) / screenPos.z;
                    startingDepth = screenPos.z;
                }

                screenPos.y = exitHeight;

                screenPos.x = horizontalOffset;
                screenPos.y -= downwardsDistance * (startingDepth - screenPos.z);
            }
            else if (screenPos.x > mainCamera.pixelWidth - horizontalOffset)
            {
                if (exitHeight == 0f)
                {
                    exitHeight = mainCamera.pixelHeight * 0.5f;
                    downwardsDistance = (exitHeight - verticalOffset) / screenPos.z;
                    startingDepth = screenPos.z;
                }

                screenPos.y = exitHeight;

                screenPos.x = mainCamera.pixelWidth - horizontalOffset;
                screenPos.y -= downwardsDistance * (startingDepth - screenPos.z);
            }
            else
            {
                if (screenPos.y > mainCamera.pixelHeight * 0.7f)
                    screenPos.y = mainCamera.pixelHeight * 0.7f;

                exitHeight = 0f;
            }
        }
        else
        {
            screenPos.x = mainCamera.pixelWidth - screenPos.x;

            if (screenPos.x < horizontalOffset)
            {
                screenPos.x = horizontalOffset;
            }
            else if (screenPos.x > mainCamera.pixelWidth - horizontalOffset)
            {
                screenPos.x = mainCamera.pixelWidth - horizontalOffset;
            }

            screenPos.y = verticalOffset;
        }

        transform.position = screenPos;
    }
}
