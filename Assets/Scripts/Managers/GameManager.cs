using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    #region Fields

    public static GameManager instance;

    [SerializeField]
    private AISpawnController aiSpawnController;
    [SerializeField]
    private ScenarioController scenarioController;

    public bool gameIsPaused;
    [SerializeField]
    PauseMenuController pauseMenuController;

    public enum GameStates { OnStartMenu, InGame, OnWaveEnd, OnGameEnd, OnGamePaused };
    public GameStates gameState;

    [SerializeField]
    private Player player;
    [SerializeField]
    private GameObject gameOverPanel;

    private GameObject crosshair;

    #endregion

    #region Properties

    #endregion

    #region MonoBehaviour Methods

    private void Awake()
    {
        UnityEngine.Assertions.Assert.IsNotNull(aiSpawnController, "ERROR: The GameManager in gameObject '" + gameObject.name + "' doesn't have an AISpawnController assigned!");
        UnityEngine.Assertions.Assert.IsNotNull(scenarioController, "ERROR: The GameManager in gameObject '" + gameObject.name + "' doesn't have a ScenarioController assigned!");

        crosshair = GameObject.Find("Crosshair");
        gameIsPaused = false;
        instance = this;
    }

    private void Start()
    {
        aiSpawnController.StartNextWave();
        scenarioController.OnNewWaveStarted();
        Debug.Log("Starting wave (index) " + aiSpawnController.GetCurrentWaveIndex() + "!");
    }

    private void Update()
    {
        switch (gameState)
        {
            case GameStates.OnStartMenu:
                break;

            case GameStates.InGame:

                if (InputManager.instance.GetPS4OptionsDown())
                {
                    OnGamePaused();
                }
                break;

            case GameStates.OnWaveEnd:
                UIManager.instance.SetEnemiesKilledCount();
                UIManager.instance.IncreaseEnemiesTimeCount();
                GoToNextWave();
                break;

            case GameStates.OnGameEnd:
                UIManager.instance.SetEnemiesKilledCount();
                UIManager.instance.IncreaseEnemiesTimeCount();
                GoToTitleScreen();
                break;

            case GameStates.OnGamePaused:
                pauseMenuController.HandlePause();
                break;
        }
    }

    #endregion

    #region Public Methods

    public void OnWaveWon()
    {
        Debug.Log("Wave (index) " + aiSpawnController.GetCurrentWaveIndex() + " finished!");

        if (aiSpawnController.HasNextWave())  
        {
            OnWaveEnd();
        }
        else
        {
            Debug.Log("No more waves available!");
            OnGameWon();
        }
    }

    public void OnWaveEnd()
    {
        if (gameState == GameStates.InGame)
        {
            crosshair.SetActive(false);
            gameOverPanel.SetActive(true);
            UIManager.instance.ChangeWaveEndText("WAVE " + (aiSpawnController.GetCurrentWaveIndex() + 1) + " SUCCEEDED");
            UIManager.instance.ChangeEndBtnText("Go To Next Wave");
            gameState = GameStates.OnWaveEnd;
        }
    }

    public void OnGameWon()
    {
        if (gameState == GameStates.InGame)
        {
            crosshair.SetActive(false);
            gameOverPanel.SetActive(true);
            UIManager.instance.ChangeWaveEndText("YOU WIN!");
            UIManager.instance.ChangeEndBtnText("Go To Title Screen");
            gameState = GameStates.OnGameEnd;
        }
    }

    public void OnGameLost()
    {
        if (gameState == GameStates.InGame)
        {
            crosshair.SetActive(false);
            gameOverPanel.SetActive(true);
            UIManager.instance.ChangeWaveEndText("YOU LOSE!");
            UIManager.instance.ChangeEndBtnText("Go To Title Screen");
            gameState = GameStates.OnGameEnd;
        }
    }

    public void OnGamePaused()
    {
        if (gameState == GameStates.InGame)
        {
            Time.timeScale = 0.0f;
            crosshair.SetActive(false);
            pauseMenuController.gameObject.SetActive(true);

            gameIsPaused = true;

            gameState = GameStates.OnGamePaused;
        }
    }

    public void ResumeGamePaused()
    {
        if (gameState == GameStates.OnGamePaused)
        {
            crosshair.SetActive(true);
            pauseMenuController.gameObject.SetActive(false);
            Time.timeScale = 1.0f;

            gameIsPaused = false;

            gameState = GameStates.InGame;
        }
    }

    public Player GetPlayer1()
    {
        return player;
    }

    public void GoToNextWave()
    {
        if (InputManager.instance.GetXButtonDown())
        {
            crosshair.SetActive(true);
            gameOverPanel.SetActive(false);
            StatsManager.instance.ResetKillCounts();
            UIManager.instance.ResetEnemiesCounters();
            StatsManager.instance.ResetBadComboCount();
            aiSpawnController.StartNextWave();
            scenarioController.OnNewWaveStarted();
            gameState = GameStates.InGame;
            Debug.Log("Starting wave (index) " + aiSpawnController.GetCurrentWaveIndex() + "!");
        }
    }

    public void GoToTitleScreen()
    {
        if (InputManager.instance.GetXButtonDown() || gameIsPaused)
        {
            gameIsPaused = false;
            gameState = GameStates.OnStartMenu;
            SceneManager.LoadScene("TitleScreen", LoadSceneMode.Single);
        }
    }

    public void RestartGame()
    {
        gameIsPaused = false;
        gameState = GameStates.InGame;
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    public void SetCrosshairActivate(bool activate)
    {
        crosshair.SetActive(activate);
    }

    #endregion

}
