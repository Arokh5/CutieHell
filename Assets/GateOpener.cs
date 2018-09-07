using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateOpener : MonoBehaviour, IZoneTakenListener
{
    [SerializeField]
    [Tooltip("The AIZoneController to listen to")]
    private AIZoneController referenceZone;

    [SerializeField]
    private GameObject leftDoor;
    [SerializeField]
    private GameObject rightDoor;

    private Vector3 leftClosedRotation;
    private Vector3 rightClosedRotation;

    [SerializeField]
    private Vector3 leftOpenedRotation;
    [SerializeField]
    private Vector3 rightOpenedRotation;

    [SerializeField]
    private float timeToOpen = 2.0f;
    
    private bool openingDoor;
    private float openingTime;

    void Start()
    {
        referenceZone.AddIZoneTakenListener(this);
        leftClosedRotation = leftDoor.transform.localEulerAngles;
        rightClosedRotation = rightDoor.transform.localEulerAngles;
        openingDoor = false;
    }

    void Update()
    {
        if (openingDoor)
        {
            openingTime += Time.deltaTime;
            if (openingTime > timeToOpen)
            {
                openingTime = timeToOpen;
                openingDoor = false;
            }

            Vector3 leftRotation = Vector3.Lerp(leftClosedRotation, leftOpenedRotation, openingTime / timeToOpen);
            Vector3 rightRotation = Vector3.Lerp(rightClosedRotation, rightOpenedRotation, openingTime / timeToOpen);

            leftDoor.transform.localEulerAngles = leftRotation;
            rightDoor.transform.localEulerAngles = rightRotation;
        }
    }

    // IZoneTakenListener
    public void OnZoneTaken()
    {
        openingDoor = true;
        openingTime = 0.0f;
    }
}
