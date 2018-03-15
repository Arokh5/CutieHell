using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugManager : MonoBehaviour {

    [Header("Debug stats")]
    [SerializeField]
    private StatsDebug statsDebug;
    [SerializeField]
    private GameObject memoryUsage;

    [Space]
    [Header("Debug camera")]

    [SerializeField]
    private GameObject debugCameraCanvas;
    [SerializeField]
    private Text values;
    [SerializeField]
    private Image grid;

    private CameraController cameraController;
    private Player playerScript;

    private bool showStats = false;
    private bool showCameraDebug = false;
    private bool showGrid = false;

	void Start () {
        cameraController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

	void Update () {
        ProcessInput();
        ActivateDebug();
	}

    private void ProcessInput() 
    {
        if (Input.GetKeyDown(KeyCode.O)) 
        {
            showStats = !showStats;
        }
        if (Input.GetKeyDown(KeyCode.P)) 
        {
            showCameraDebug = !showCameraDebug;
        }
    }
    private void ActivateDebug() 
    {
        if(showStats) {
            statsDebug.Show_Stats = true;
            memoryUsage.SetActive(true);
        } else {
            statsDebug.Show_Stats = false;
            memoryUsage.SetActive(false);
        }
        if (showCameraDebug) {
            debugCameraCanvas.SetActive(true);
            DebugCamera();
        } else {
            debugCameraCanvas.SetActive(false);
        }
    }
    private void DebugCamera() {
        switch (playerScript.state) {
            case Player.PlayerStates.MOVE:
                if (Input.GetKeyDown(KeyCode.Z)) {
                    showGrid = !showGrid;
                }
                grid.gameObject.SetActive(showGrid);
                if (Input.GetKey(KeyCode.X)) {
                    cameraController.distance += Time.deltaTime * 0.5f;
                }
                if (Input.GetKey(KeyCode.C)) {
                    cameraController.distance -= Time.deltaTime * 0.5f;
                }
                if (Input.GetKey(KeyCode.V)) {
                    cameraController.cameraX -= Time.deltaTime * 0.5f;
                }
                if (Input.GetKey(KeyCode.B)) {
                    cameraController.cameraX += Time.deltaTime * 0.5f;
                }
                if (Input.GetKey(KeyCode.N)) {
                    cameraController.cameraY += Time.deltaTime * 0.5f;
                }
                if (Input.GetKey(KeyCode.M)) {
                    cameraController.cameraY -= Time.deltaTime * 0.5f;
                }
                if (Input.GetKey(KeyCode.S)) {
                    cameraController.fov += Time.deltaTime * 4f;
                }
                if (Input.GetKey(KeyCode.D)) {
                    cameraController.fov -= Time.deltaTime * 4f;
                }
                if (Input.GetKey(KeyCode.F)) {
                    cameraController.focusDistance += Time.deltaTime * 0.5f;
                }
                if (Input.GetKey(KeyCode.G)) {
                    cameraController.focusDistance -= Time.deltaTime * 0.5f;
                }
                if (Input.GetKey(KeyCode.H)) {
                    cameraController.focusX -= Time.deltaTime * 0.5f;
                }
                if (Input.GetKey(KeyCode.J)) {
                    cameraController.focusX += Time.deltaTime * 0.5f;
                }
                if (Input.GetKey(KeyCode.K)) {
                    cameraController.focusY += Time.deltaTime * 0.5f;
                }
                if (Input.GetKey(KeyCode.L)) {
                    cameraController.focusY -= Time.deltaTime * 0.5f;
                }
                if (Input.GetKeyDown(KeyCode.Return)) {
                    cameraController.distance = 3.0f;
                    cameraController.cameraX = 0.5f;
                    cameraController.cameraY = 1.75f;
                    cameraController.focusDistance = 0.4f;
                    cameraController.focusX = 0.3f;
                    cameraController.focusY = 1.7f;
                    cameraController.fov = 60;
                    showGrid = false;
                }
                values.text = "Distance : " + cameraController.distance + "\nCameraX : " + 
                    cameraController.cameraX + "\nCameraY : " + cameraController.cameraY + "\nFocus Distance" +
                    cameraController.focusDistance + "\nFocusX : " + cameraController.focusX + "\nFocusY : " + cameraController.focusY
                    + "\nFOV : " + cameraController.fov;
                break;
            case Player.PlayerStates.TURRET:
                if (Input.GetKeyDown(KeyCode.Z)) {
                    showGrid = !showGrid;
                }
                grid.gameObject.SetActive(showGrid);
                if (Input.GetKey(KeyCode.X)) {
                    cameraController.t_distance += Time.deltaTime * 0.5f;
                }
                if (Input.GetKey(KeyCode.C)) {
                    cameraController.t_distance -= Time.deltaTime * 0.5f;
                }
                if (Input.GetKey(KeyCode.N)) {
                    cameraController.t_cameraY += Time.deltaTime * 0.5f;
                }
                if (Input.GetKey(KeyCode.M)) {
                    cameraController.t_cameraY -= Time.deltaTime * 0.5f;
                }
                if (Input.GetKey(KeyCode.S)) {
                    cameraController.t_fov += Time.deltaTime * 4f;
                }
                if (Input.GetKey(KeyCode.D)) {
                    cameraController.t_fov -= Time.deltaTime * 4f;
                }
                if (Input.GetKeyDown(KeyCode.Return)) {
                    cameraController.t_distance = 3.0f;
                    cameraController.t_cameraY = 1.75f;
                    cameraController.t_fov = 40;
                    showGrid = false;
                }
                values.text = "Distance : " + cameraController.t_distance + "\nCameraY : " + cameraController.t_cameraY +
                    "\nFOV : " + cameraController.t_fov;
                break;
        }
    }
}
