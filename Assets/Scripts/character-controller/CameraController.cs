using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour {

    private Transform player;

    private const float xSpeed = 2.5f;
    private const float ySpeed = 1.2f;
    private const float yMinLimit = -20f;
    private const float yMaxLimit = 40f;
    private const float lerpSpeed = 0.1f;

    private int collisionLayers;
    //private float distance;
    private float x;
    private float y;

    [Header("Debug components to drag")]
    //For debugg position
    private bool debugCamera = false;
    [SerializeField]
    private GameObject debugCanvas;
    private float distance;
    private float cameraX;
    private float cameraY;
    private float focusDistance;
    private float focusX;
    private float focusY;
    [SerializeField]
    private Text values;
    [SerializeField]
    private Image grid;
    private bool gridOn;

    private void Awake()
    {
        collisionLayers = 1 << 4;
        //distance = 3.0f;
        x = 0f;
        y = 0f;

        player = GameObject.Find("Player").transform;
        //For debugg position

        distance = 3.0f;
        cameraX = 0.5f;
        cameraY = 1.75f;
        focusDistance = 0.4f;
        focusX = 0.3f;
        focusY = 1.7f;
        gridOn = false;
    }

    private void Start()
    {
        Vector2 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
    }

    private void Update()
    {
        RotateCamera();
        if (debugCanvas != null) {
            if (Input.GetKeyDown(KeyCode.P)) {
                debugCamera = !debugCamera;
            }
            if (debugCamera) {
                debugCanvas.SetActive(true);
                DebugCamera();
            } else {
                debugCanvas.SetActive(false);
            }
        }
    }

    private void RotateCamera()
    {
        if(InputManager.instance.GetRightStickLeft() || InputManager.instance.GetRightStickRight())
            x += xSpeed * InputManager.instance.GetRightStickLeftValue();
        if(InputManager.instance.GetRightStickUp() || InputManager.instance.GetRightStickDown())
            y += ySpeed * InputManager.instance.GetRightStickUpValue();

        y = ClampAngle(y, yMinLimit, yMaxLimit);

        Quaternion rotation = Quaternion.Euler(y, x, 0);

        /* Checks for collisions */
        float noCollisionDistance = distance;

        for( float zOffset = distance; zOffset >= 0.5f; zOffset -= 0.05f) 
        {
            noCollisionDistance = zOffset;
            Vector3 tempPos = rotation * new Vector3(cameraX, cameraY, -noCollisionDistance) + player.position;

            if (DoubleViewingPosCheck(tempPos, zOffset)) 
            {
                break;
            }
        }
        /* Ends collision detection */

        Vector3 position = rotation * new Vector3(cameraX, cameraY, -noCollisionDistance) + player.position;

        transform.position = position;

        SetPlayerDirection(rotation.eulerAngles.y);

        this.transform.LookAt(player.transform.position + player.transform.up  * focusY + player.transform.right * focusX + player.transform.forward * focusDistance);
    }

    private float LerpRotation(float cameraRotationY)
    {
        float playerRotationY = player.rotation.eulerAngles.y;

        if (cameraRotationY != playerRotationY)
        {
            playerRotationY = Mathf.LerpAngle(playerRotationY, cameraRotationY, lerpSpeed);
        }
        return playerRotationY;
    }

    private void SetPlayerDirection(float rotation)
    {
        if (!Input.GetMouseButton(0))
        {
            player.rotation = Quaternion.Euler(player.rotation.x, rotation, player.rotation.z);
        }
    }

    private float ClampAngle(float angle, float min, float max)
    {
        while (angle < -360)
            angle += 360;
        while (angle > 360)
            angle -= 360;

        return Mathf.Clamp(angle, min, max);
    }

    private bool DoubleViewingPosCheck(Vector3 checkPos, float offset) 
    {
        float playerFocusHeight = player.GetComponent<CapsuleCollider>().height * 0.5f;
        return ViewingPosCheck(checkPos, playerFocusHeight) && ReverseViewingPosCheck(checkPos, playerFocusHeight, offset);
    }

    private bool ViewingPosCheck(Vector3 checkPos, float deltaPlayerHeight) 
    {
        RaycastHit hit;

        if (Physics.Raycast(checkPos, player.position + (Vector3.up * deltaPlayerHeight) - checkPos, out hit)) 
        {
            if (hit.transform.gameObject.layer == 4) 
            {
                return false;
            }
        }
        return true;
    }

    private bool ReverseViewingPosCheck(Vector3 checkPos, float deltaPlayerHeight, float offset) {
        RaycastHit hit;
        Debug.DrawRay(player.position + (Vector3.up * deltaPlayerHeight), checkPos - player.position, Color.green);
        if (Physics.Raycast(player.position + (Vector3.up * deltaPlayerHeight), checkPos - player.position, out hit, offset)) 
        {   
            if (hit.transform.gameObject.layer == 4) 
            {
                return false;
            }
        }
        return true;
    }

    private void DebugCamera() 
    {
        if (Input.GetKeyDown(KeyCode.Z)) 
        {
            gridOn = !gridOn;
        }
        grid.gameObject.SetActive(gridOn);
        if (Input.GetKey(KeyCode.X)) {
            distance += Time.deltaTime * 0.5f;
        }
        if (Input.GetKey(KeyCode.C)) {
            distance -= Time.deltaTime * 0.5f;
        }
        if (Input.GetKey(KeyCode.V)) {
            cameraX -= Time.deltaTime * 0.5f;
        }
        if (Input.GetKey(KeyCode.B)) {
            cameraX += Time.deltaTime * 0.5f;
        }
        if (Input.GetKey(KeyCode.N)) {
            cameraY += Time.deltaTime * 0.5f;
        }
        if (Input.GetKey(KeyCode.M)) {
            cameraY -= Time.deltaTime * 0.5f;
        }
        if (Input.GetKey(KeyCode.F)) {
            focusDistance += Time.deltaTime * 0.5f;
        }
        if (Input.GetKey(KeyCode.G)) {
            focusDistance -= Time.deltaTime * 0.5f;
        }
        if (Input.GetKey(KeyCode.H)) {
            focusX -= Time.deltaTime * 0.5f;
        }
        if (Input.GetKey(KeyCode.J)) {
            focusX += Time.deltaTime * 0.5f;
        }
        if (Input.GetKey(KeyCode.K)) {
            focusY += Time.deltaTime * 0.5f;
        }
        if (Input.GetKey(KeyCode.L)) {
            focusY -= Time.deltaTime * 0.5f;
        }
        if (Input.GetKeyDown(KeyCode.Return)) {
            distance = 3.0f;
            cameraX = 0.5f;
            cameraY = 1.75f;
            focusDistance = 0.4f;
            focusX = 0.3f;
            focusY = 1.7f;
            gridOn = true;
        }
        values.text = "Distance : " + distance + "\nCameraX : " + cameraX + "\nCameraY : " + cameraY + "\nFocus Distance" +
            focusDistance + "\nFocusX : " + focusX + "\nFocusY : " + focusY;
    }
}