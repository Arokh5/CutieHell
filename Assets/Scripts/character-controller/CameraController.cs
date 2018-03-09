using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour {

    private Transform player;
    private Player playerScript;

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
    [SerializeField]
    private GameObject debugCanvas;
    [SerializeField]
    private Text values;
    [SerializeField]
    private Image grid;

    /* Player camera values */
    private bool debugCamera = false;
    private bool gridOn;
    private float distance;
    private float cameraX;
    private float cameraY;
    private float focusDistance;
    private float focusX;
    private float focusY;


    /* Turret camera values */
    private float t_distance;
    private float t_cameraX;
    private float t_cameraY;
    private float t_focusDistance;
    private float t_focusX;
    private float t_focusY;


    private void Awake()
    {
        //distance = 3.0f;
        x = 0f;
        y = 0f;

        player = GameObject.Find("Player").transform;
        playerScript = player.gameObject.GetComponent<Player>();

        //For debugg position
        gridOn = false;
        distance = 3.0f;
        cameraX = 0.5f;
        cameraY = 1.75f;
        focusDistance = 0.4f;
        focusX = 0.3f;
        focusY = 1.7f;

        t_distance = 3.0f;
        t_cameraX = 0.0f;
        t_cameraY = 1.75f;
        t_focusDistance = 0.4f;
        t_focusX = 0.0f;
        t_focusY = 1.7f;

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
        if (InputManager.instance.GetRightStickLeft() || InputManager.instance.GetRightStickRight())
            x += xSpeed * InputManager.instance.GetRightStickLeftValue();
        if (InputManager.instance.GetRightStickUp() || InputManager.instance.GetRightStickDown())
            y += ySpeed * InputManager.instance.GetRightStickUpValue();

        y = ClampAngle(y, yMinLimit, yMaxLimit);

        switch (playerScript.state) 
        {
            case Player.PlayerStates.STILL:
                break;
            case Player.PlayerStates.MOVE: 
                {
                    Quaternion rotation = Quaternion.Euler(y, x, 0);
                    float noCollisionDistance = distance;

                    for (float zOffset = distance; zOffset >= 0.5f; zOffset -= 0.05f) {
                        noCollisionDistance = zOffset;
                        Vector3 tempPos = rotation * new Vector3(cameraX, cameraY, -noCollisionDistance) + player.position;

                        if (DoubleViewingPosCheck(tempPos, zOffset)) {
                            break;
                        }
                    }
                    Vector3 position = rotation * new Vector3(cameraX, cameraY, -noCollisionDistance) + player.position;
                    transform.position = position;
                    SetPlayerDirection(rotation.eulerAngles.y);
                    this.transform.LookAt(player.transform.position + player.transform.up * focusY + player.transform.right * focusX + player.transform.forward * focusDistance);
                }

                break;
            case Player.PlayerStates.WOLF:
                break;
            case Player.PlayerStates.FOG:
                break;
            case Player.PlayerStates.TURRET: 
                {
                    Quaternion rotation = Quaternion.Euler(y, x, 0);
                    float noCollisionDistance = t_distance;

                    for (float zOffset = t_distance; zOffset >= 0.5f; zOffset -= 0.05f) {
                        noCollisionDistance = zOffset;
                        Vector3 tempPos = rotation * new Vector3(t_cameraX, t_cameraY, -noCollisionDistance) + player.position;

                        if (DoubleViewingPosCheck(tempPos, zOffset)) {
                            break;
                        }
                    }
                    Vector3 position = rotation * new Vector3(t_cameraX, t_cameraY, -noCollisionDistance) + player.position;
                    transform.position = position;
                    SetPlayerDirection(rotation.eulerAngles.y);
                    this.transform.LookAt(player.transform.position + player.transform.up * t_focusY + player.transform.right * t_focusX + player.transform.forward * t_focusDistance);
                }
                break;
            default:
                break;
        }
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
            if (hit.transform.gameObject.layer == 9) 
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
            if (hit.transform.gameObject.layer == 9) 
            {
                return false;
            }
        }
        return true;
    }

    private void DebugCamera() 
    {
        switch (playerScript.state) {
            case Player.PlayerStates.MOVE:
                if (Input.GetKeyDown(KeyCode.Z)) {
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
                break;
            case Player.PlayerStates.TURRET:
                if (Input.GetKeyDown(KeyCode.Z)) {
                    gridOn = !gridOn;
                }
                grid.gameObject.SetActive(gridOn);
                if (Input.GetKey(KeyCode.X)) {
                    t_distance += Time.deltaTime * 0.5f;
                }
                if (Input.GetKey(KeyCode.C)) {
                    t_distance -= Time.deltaTime * 0.5f;
                }
                if (Input.GetKey(KeyCode.V)) {
                    t_cameraX -= Time.deltaTime * 0.5f;
                }
                if (Input.GetKey(KeyCode.B)) {
                    t_cameraX += Time.deltaTime * 0.5f;
                }
                if (Input.GetKey(KeyCode.N)) {
                    t_cameraY += Time.deltaTime * 0.5f;
                }
                if (Input.GetKey(KeyCode.M)) {
                    t_cameraY -= Time.deltaTime * 0.5f;
                }
                if (Input.GetKey(KeyCode.F)) {
                    t_focusDistance += Time.deltaTime * 0.5f;
                }
                if (Input.GetKey(KeyCode.G)) {
                    t_focusDistance -= Time.deltaTime * 0.5f;
                }
                if (Input.GetKey(KeyCode.H)) {
                    t_focusX -= Time.deltaTime * 0.5f;
                }
                if (Input.GetKey(KeyCode.J)) {
                    t_focusX += Time.deltaTime * 0.5f;
                }
                if (Input.GetKey(KeyCode.K)) {
                    t_focusY += Time.deltaTime * 0.5f;
                }
                if (Input.GetKey(KeyCode.L)) {
                    t_focusY -= Time.deltaTime * 0.5f;
                }
                if (Input.GetKeyDown(KeyCode.Return)) {
                    t_distance = 3.0f;
                    t_cameraX = 0.0f;
                    t_cameraY = 1.75f;
                    t_focusDistance = 0.4f;
                    t_focusX = 0.0f;
                    t_focusY = 1.7f;
                    gridOn = true;
                }
                values.text = "Distance : " + t_distance + "\nCameraX : " + t_cameraX + "\nCameraY : " + t_cameraY + "\nFocus Distance" +
                    t_focusDistance + "\nFocusX : " + t_focusX + "\nFocusY : " + t_focusY;
                break;
        }

    }
}