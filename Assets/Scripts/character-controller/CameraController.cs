using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour {

    private Transform player;
    private Player playerScript;

    private const float xSpeed = 2.25f; //2.5f
    private const float ySpeed = 1.0f; // 1.2f
    private const float yMinLimit = -20f;
    private const float yMaxLimit = 40f;
    private const float lerpSpeed = 0.1f;
    private const float transitionTime = 2.0f;
    private float timeOnTransition = 10.0f;
    private Player.CameraState lastState;

    private int collisionLayers;
    private float x;
    private float y;

    /* Player camera values */
    public float distance;
    public float cameraX;
    public float cameraY;
    public float focusDistance;
    public float focusX;
    public float focusY;
    public float fov;


    /* Turret camera values */
    public float t_distance;
    public float t_cameraY;
    public float t_fov;


    private void Awake()
    {
        x = 0f;
        y = 0f;

        player = GameObject.Find("Player").transform;
        playerScript = player.gameObject.GetComponent<Player>();

        /*
        distance = 3.0f;
        cameraX = 0.5f;
        cameraY = 1.75f;
        focusDistance = 0.4f;
        focusX = 0.3f;
        focusY = 1.7f;
        */
        distance = 2.8f;
        cameraX = 0.45f;
        cameraY = 1.8f;
        focusDistance = 0.65f;
        focusX = 0.55f;
        focusY = 1.65f;
        fov = 60f;

        t_distance = -2.2f;
        t_cameraY = 0.6f;
        t_fov = 40f;

    }

    private void Start()
    {
        Vector2 angles = transform.eulerAngles;
        x = 35.5f;
        y = 0.0f;
        lastState = playerScript.cameraState;
    }

    private void Update()
    {
        RotateCamera();
    }

    private void RotateCamera()
    {
        if(!GameManager.instance.gameIsPaused)
        {
            if (InputManager.instance.GetRightStickLeft() || InputManager.instance.GetRightStickRight())
                x += xSpeed * InputManager.instance.GetRightStickLeftValue();
            if (InputManager.instance.GetRightStickUp() || InputManager.instance.GetRightStickDown())
                y += ySpeed * InputManager.instance.GetRightStickUpValue();

            y = ClampAngle(y, yMinLimit, yMaxLimit);
            if (playerScript.cameraState != lastState)
            {
                timeOnTransition = 0.0f;
            }
            switch (playerScript.cameraState)
            {
                case Player.CameraState.STILL:
                    break;
                case Player.CameraState.MOVE:
                    {
                        if (this.transform.parent != null) this.transform.parent = null;
                        Quaternion rotation = Quaternion.Euler(y, x, 0);
                        float noCollisionDistance = distance;

                        for (float zOffset = distance; zOffset >= 0.5f; zOffset -= 0.025f)
                        {
                            noCollisionDistance = zOffset;
                            Vector3 tempPos = rotation * new Vector3(cameraX, cameraY, -noCollisionDistance) + player.position;

                            if (DoubleViewingPosCheck(tempPos, zOffset))
                            {
                                break;
                            }
                        }
                        if (timeOnTransition < transitionTime)
                        {
                            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, fov, 0.1f);
                            timeOnTransition += Time.deltaTime;
                            this.transform.position = Vector3.Lerp(this.transform.position, rotation * new Vector3(cameraX, cameraY, -noCollisionDistance) + player.position, timeOnTransition / 2f);
                        }
                        else
                        {
                            Camera.main.fieldOfView = fov;
                            Vector3 position = rotation * new Vector3(cameraX, cameraY, -noCollisionDistance) + player.position;
                            this.transform.position = position;
                        }
                        SetPlayerDirection(rotation.eulerAngles.y);
                        this.transform.LookAt(player.transform.position + player.transform.up * focusY + player.transform.right * focusX + player.transform.forward * focusDistance);
                    }

                    break;
                case Player.CameraState.WOLF:
                    break;
                case Player.CameraState.FOG:
                    break;
                case Player.CameraState.TURRET:
                    {
                        Quaternion rotation = Quaternion.Euler(y, x, 0);
                        playerScript.currentTrap.transform.rotation = rotation;
                        this.transform.SetParent(playerScript.currentTrap.transform);
                        if (timeOnTransition < transitionTime)
                        {
                            timeOnTransition += Time.deltaTime;
                            this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, new Vector3(0.0f, t_cameraY, t_distance), timeOnTransition / 2f);
                            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, t_fov, 0.1f);
                        }
                        else
                        {
                            Camera.main.fieldOfView = t_fov;
                            this.transform.localPosition = new Vector3(0.0f, t_cameraY, t_distance);
                        }
                        this.transform.localRotation = Quaternion.identity;
                        SetPlayerDirection(rotation.eulerAngles.y);
                    }
                    break;
                default:
                    break;
            }
            lastState = playerScript.cameraState;
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
}