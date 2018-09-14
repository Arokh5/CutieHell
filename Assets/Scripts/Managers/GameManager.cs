using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameManager : MonoBehaviour
{
    #region Fields

    public static GameManager instance;

    [SerializeField]
    private TutorialManager tutorialManager;
    [SerializeField]
    private ScreenFadeController screenFadeController;

    private bool avoidPlayerUpdate;
    private Player.CameraState previousCameraState;

    [SerializeField]
    private AISpawnController aiSpawnController;
    [SerializeField]
    private ScenarioController scenarioController;

    public bool gameIsPaused;
    [SerializeField]
    private PauseMenuController pauseMenuController;

    public enum GameStates { OnStartMenu, InGame, OnRoundEnd, OnGameEnd, OnGamePaused };
    public GameStates gameState;

    [SerializeField]
    private Player player;
   
    [SerializeField]
    private GameObject crosshair;

    [SerializeField]
    private RoundScore roundScore;

    [Header("Game End")]
    [SerializeField]
    private GameScore gameScore;
    [SerializeField]
    private AudioClip victoryClip;
    [SerializeField]
    private AudioClip defeatClip;
    [SerializeField]
    private GameObject[] uiToHide;

    private int roundsCompleted = 0;

    private bool unpauseNextFrame = false;

#if UNITY_EDITOR
    [Header("Editor tools")]
    [SerializeField]
    private bool forceGameLoss;
#endif
    
    #endregion

    #region MonoBehaviour Methods

    private void Awake()
    {
        UnityEngine.Assertions.Assert.IsNotNull(aiSpawnController, "ERROR: The GameManager in gameObject '" + gameObject.name + "' doesn't have an AISpawnController assigned!");
        UnityEngine.Assertions.Assert.IsNotNull(scenarioController, "ERROR: The GameManager in gameObject '" + gameObject.name + "' doesn't have a ScenarioController assigned!");
        UnityEngine.Assertions.Assert.IsNotNull(tutorialManager, "ERROR: The GameManager in gameObject '" + gameObject.name + "' doesn't have a TutorialManager assigned!");
        UnityEngine.Assertions.Assert.IsNotNull(screenFadeController, "ERROR: The GameManager in gameObject '" + gameObject.name + "' doesn't have a ScreenFadeController assigned!");
        //UnityEngine.Assertions.Assert.IsNotNull(crosshair, "ERROR: The GameManager in gameObject '" + gameObject.name + "' doesn't have a Crosshair assigned!");

        gameIsPaused = false;
        instance = this;

#if UNITY_EDITOR
        EditorApplication.pauseStateChanged += EditorPaused;
#endif
    }

    private void Start()
    {
        FreezePlayer();
        tutorialManager.RequestStartTutorial(OnTutorialFinished);
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

            case GameStates.OnRoundEnd:
                // GoToNextRound() is now called from the RoundScore script
                break;

            case GameStates.OnGameEnd:
                // GoToTitleScreen() is now called from the GameScore script or from the ExitGame method (called by PauseMenuController)
                break;

            case GameStates.OnGamePaused:
                if (unpauseNextFrame)
                {
                    ResumeGamePaused();
                }
                else
                {
                    pauseMenuController.HandlePause();
                }
                break;
        }
#if UNITY_EDITOR
        if (forceGameLoss)
        {
            forceGameLoss = false;
            scenarioController.OnFinalZoneConquered();
        }
#endif
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            OnGamePaused();
        }
    }
    #endregion

    #region Public Methods
    public bool LaunchTutorialEvent(int eventIndex, TutorialManager.VoidCallback endCallback = null)
    {
        bool launched = false;
        TutorialManager.VoidCallback finishCallback = OnTutorialEventFinished;

        if (endCallback != null)
        {
            // Ensure the GameManager callback is called before any other callback
            finishCallback += endCallback;
        }

        if (tutorialManager.LaunchEventMessage(eventIndex, finishCallback))
        {
            Debug.Log("INFO: (GameManager) Tutorial Event " + eventIndex + " launched!");
            FreezePlayer();
            launched = true;
        }
        return launched;
    }

    public bool CanUpdatePlayer()
    {
        return !gameIsPaused && !avoidPlayerUpdate;
    }

    public void PauseEnemySpawning()
    {
        aiSpawnController.PauseRound();
    }

    public void ResumeEnemySpawning()
    {
        aiSpawnController.ResumeRound();
    }

    public int GetEnemiesCount()
    {
        return scenarioController.GetEnemiesCount();
    }

    public float GetCurrentMonumentNormalizedHealth()
    {
        return scenarioController.GetCurrentMonumentNormalizedHealth();
    }

    public void ExitGame()
    {
        GoToTitleScreen();
    }

    public void OnRoundWon()
    {
        Debug.Log("INFO: Round (index) " + aiSpawnController.GetCurrentRoundIndex() + " finished!");
        player.OnRoundOver();

        ++roundsCompleted;
        OnRoundEnd();
    }

    public void OnRoundEnd()
    {
        if (gameState == GameStates.InGame)
        {
            //crosshair.SetActive(false);
            
            StatsManager.instance.GetMaxCombo().GrantReward();
            StatsManager.instance.GetTimeCombo().GrantReward();
            StatsManager.instance.GetReceivedDamageCombo().GrantReward();

            roundScore.gameObject.SetActive(true);
            roundScore.SetUpTotalScore(StatsManager.instance.GetRoundPoints(), roundsCompleted);
            roundScore.ShowRoundScore();
            gameState = GameStates.OnRoundEnd;
        }
    }

    public void OnGameWon()
    {
        if (gameState == GameStates.OnRoundEnd)
        {
            StatsManager.instance.GetMaxCombo().GrantReward();
            StatsManager.instance.GetTimeCombo().GrantReward();
            StatsManager.instance.GetReceivedDamageCombo().GrantReward();

            gameState = GameStates.OnGameEnd;
            SoundManager.instance.PlayMusicClip(victoryClip, true);
            HideUIOnGameEnd();
            gameScore.ShowGameScore(true);
        }
    }

    public void OnGameLost()
    {
        if (gameState == GameStates.InGame)
        {
            player.OnRoundOver();
            //crosshair.SetActive(false);

            gameState = GameStates.OnGameEnd;
            SoundManager.instance.PlayMusicClip(defeatClip, false);
            HideUIOnGameEnd();
            gameScore.ShowGameScore(false);
        }
    }

    public void OnGamePaused()
    {
        if (gameState == GameStates.InGame)
        {
            TimeManager.instance.FreezeTime();
            //crosshair.SetActive(false);

            pauseMenuController.gameObject.SetActive(true);

            gameIsPaused = true;

            gameState = GameStates.OnGamePaused;
        }
    }

    public void RequestResumeGamePaused()
    {
        if (gameState == GameStates.OnGamePaused)
        {
            unpauseNextFrame = true;
        }
    }

    public Player GetPlayer1()
    {
        return player;
    }

    public void GoToNextRound()
    {
        if (aiSpawnController.HasNextRound())
        {
            StatsManager.instance.ResetKillCounts();
            StatsManager.instance.ResetRoundPoints();
            StatsManager.instance.GetMaxCombo().ResetCombo();
            StatsManager.instance.GetTimeCombo().ResetCombo();
            StatsManager.instance.GetReceivedDamageCombo().ResetCombo();
            gameState = GameStates.InGame;

            aiSpawnController.StartNextRound();
            if (aiSpawnController.GetCurrentRoundIndex() > 0)
                player.OnRoundStarted();
        }
        else
        {
            Debug.Log("INFO: No more rounds available!");
            OnGameWon();
        }
        
    }

    public void GoToTitleScreen()
    {
        TimeManager.instance.ResumeTime();
        FreezePlayer();
        gameIsPaused = false;
        gameState = GameStates.OnStartMenu;
        SceneManager.LoadScene("TitleScreen", LoadSceneMode.Single);
    }

    public void RestartGame()
    {
        TimeManager.instance.ResumeTime();
        FreezePlayer();
        gameIsPaused = false;
        gameState = GameStates.InGame;
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    public int GetRoundsCompleted()
    {
        return roundsCompleted;
    }
   
    #endregion

    #region Private Methods
    private void HideUIOnGameEnd()
    {
        if (uiToHide != null)
        {
            foreach (GameObject go in uiToHide)
            {
                go.SetActive(false);
            }
        }
    }

    private void FreezePlayer()
    {
        avoidPlayerUpdate = true;
        previousCameraState = player.cameraState;
        player.cameraState = Player.CameraState.STILL;
    }

    private void ReleasePlayer()
    {
        player.cameraState = previousCameraState;
        avoidPlayerUpdate = false;
    }

    private void OnTutorialFinished()
    {
        ReleasePlayer();
        player.OnRoundStarted();
        Debug.Log("INFO: (GameManager) Tutorial finished!");
    }

    private void OnTutorialEventFinished()
    {
        ReleasePlayer();
        Debug.Log("INFO: (GameManager) Tutorial Event finished!");
    }

    private void ResumeGamePaused()
    {
        unpauseNextFrame = false;

        pauseMenuController.gameObject.SetActive(false);
        //crosshair.SetActive(true);

        TimeManager.instance.ResumeTime();

        gameIsPaused = false;

        gameState = GameStates.InGame;
    }

#if UNITY_EDITOR
    private void EditorPaused(PauseState state)
    {
        if (state == PauseState.Paused)
            OnApplicationPause(true);
    }
#endif
    #endregion
}
