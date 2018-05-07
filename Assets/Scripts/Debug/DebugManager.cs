using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugManager : MonoBehaviour {

    [Header("Debug Panel")]
    [SerializeField]
    private GameObject panel;

    [Space]
    [Header("Debug stats")]
    [SerializeField]
    private StatsDebug statsDebug;

    [Space]
    [Header("Debug camera")]

    [SerializeField]
    private GameObject debugCameraCanvas;
    [SerializeField]
    private Text values;
    [SerializeField]
    private Image grid;

    [Space]
    [Header("World camera debug")]
    [SerializeField]
    private GameObject worldCamera;
    [SerializeField]
    private GameObject instructionsWorldCamera;

    [Space]
    [Header("Waves Debug")]
    [SerializeField]
    private GameObject wavesDebugInfo;

    private CameraController cameraController;
    private Player playerScript;
    private Camera worldCameraComponent;
    private AISpawnController spawnController;

    [SerializeField]
    private Toggle fpsInfo;
    [SerializeField]
    private Toggle cameraDebug;
    [SerializeField]
    private Toggle wavesDebug;
    [SerializeField]
    private Toggle worldCameraDebug;

    private bool showGrid = false;
    private bool followPlayer = true;
    private bool showDebugWindow = false;

    private GameObject player;

    private KeyCode[] numberKeyCodes =
    {
        KeyCode.Alpha0,
        KeyCode.Alpha1,
        KeyCode.Alpha2,
        KeyCode.Alpha3,
        KeyCode.Alpha4,
        KeyCode.Alpha5,
        KeyCode.Alpha6,
        KeyCode.Alpha7,
        KeyCode.Alpha8,
        KeyCode.Alpha9
    };

	void Start () {
        cameraController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        player = GameObject.FindGameObjectWithTag("Player");
        worldCameraComponent = worldCamera.GetComponent<Camera>();
        spawnController = FindObjectOfType<AISpawnController>();
    }

	void Update () {
        ProcessInput();
        ActivateDebug();
	}

    private void ShowOneWindow(ref bool handler)
    {
        bool previousState = handler;
        showDebugWindow = false;
        handler = !previousState;
    }

    private void ProcessInput() 
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowOneWindow(ref showDebugWindow);
        }
    }

    private void ActivateDebug() 
    {
        if (showDebugWindow)
        {
            panel.SetActive(true);
            statsDebug.Show_Stats = true;
        }
        else
        {
            panel.SetActive(false);
            statsDebug.Show_Stats = false;
        }

        if (cameraDebug.isOn) {
            debugCameraCanvas.SetActive(true);
            DebugCamera();
        } else {
            debugCameraCanvas.SetActive(false);
        }

        if (worldCameraDebug.isOn) {
            worldCamera.SetActive(true);
            instructionsWorldCamera.SetActive(true);
            WorldCameraDebug();
        } else {
            worldCamera.SetActive(false);
            instructionsWorldCamera.SetActive(false);
        }

        if (wavesDebug.isOn)
        {
            wavesDebugInfo.SetActive(true);
            WavesDebug();
        }
        else
        {
            wavesDebugInfo.SetActive(false);
        }
    }

    private void DebugCamera() {
        switch (playerScript.cameraState) {
            case Player.CameraState.MOVE:
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
            case Player.CameraState.TURRET:
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

    private void WorldCameraDebug() {

        if (Input.GetKeyDown(KeyCode.C)) followPlayer = !followPlayer;

        if (Input.GetKey(KeyCode.Z)) {
            if (worldCameraComponent.orthographicSize > 1) {
                worldCameraComponent.orthographicSize -= Time.deltaTime * 7;
            }
        }
        if (Input.GetKey(KeyCode.X)) {
            if (worldCameraComponent.orthographicSize < 50) {
                worldCameraComponent.orthographicSize += Time.deltaTime * 7;
            }
        }

        if (followPlayer) {
            worldCamera.transform.position = player.transform.position + Vector3.up * 50;
        } else {
            if (Input.GetKey(KeyCode.UpArrow)) {
                worldCamera.transform.Translate(Vector3.up * Time.deltaTime * 10);
            }
            if (Input.GetKey(KeyCode.DownArrow)) {
                worldCamera.transform.Translate(Vector3.up * Time.deltaTime * -10);
            }
            if (Input.GetKey(KeyCode.LeftArrow)) {
                worldCamera.transform.Translate(Vector3.left * Time.deltaTime * 10);
            }
            if (Input.GetKey(KeyCode.RightArrow)) {
                worldCamera.transform.Translate(Vector3.left * Time.deltaTime * -10);
            }
        }
    }

    private void WavesDebug()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            spawnController.WinCurrentWave();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            spawnController.RestartCurrentWave();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            playerScript.SetEvilLevel(playerScript.GetMaxEvilLevel());
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            playerScript.SetEvilLevel(-playerScript.GetEvilLevel());
        }

        for (int i = 0; i < numberKeyCodes.Length; ++i)
        {
            if (Input.GetKeyDown(numberKeyCodes[i]))
            {
                if (i > 0)
                    spawnController.ForceStartWave(i - 1);
            }
        }
    }
}
