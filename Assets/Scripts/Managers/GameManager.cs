using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameManager : MonoBehaviour
{
    #region Fields

    public static GameManager instance;

    [SerializeField]
    private bool skipTutorial = false;
    public TutorialController tutorialController;
    [SerializeField]
    private ScreenFadeController screenFadeController;

    [SerializeField]
    private AISpawnController aiSpawnController;
    [SerializeField]
    private ScenarioController scenarioController;

    public bool gameIsPaused;
    [SerializeField]
    private PauseMenuController pauseMenuController;
    [SerializeField]
    private PauseMenuController tutorialPauseMenuController;

    public enum GameStates { OnStartMenu, InGame, OnRoundEnd, OnGameEnd, OnGamePaused };
    public GameStates gameState;

    [SerializeField]
    private Player player;
   
    [SerializeField]
    private GameObject crosshair;

    [SerializeField]
    private GameObject gameOverPanel;
    [SerializeField]
    private RoundScore roundScore;


    #endregion

    #region Properties

    #endregion

    #region MonoBehaviour Methods

    private void Awake()
    {
        UnityEngine.Assertions.Assert.IsNotNull(aiSpawnController, "ERROR: The GameManager in gameObject '" + gameObject.name + "' doesn't have an AISpawnController assigned!");
        UnityEngine.Assertions.Assert.IsNotNull(scenarioController, "ERROR: The GameManager in gameObject '" + gameObject.name + "' doesn't have a ScenarioController assigned!");
        UnityEngine.Assertions.Assert.IsNotNull(tutorialController, "ERROR: The GameManager in gameObject '" + gameObject.name + "' doesn't have a TutorialController assigned!");
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
        if (skipTutorial)
            tutorialController.RequestBypassTutorial();
        else
            tutorialController.RequestStartTutorial();
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
                UIManager.instance.IncreaseEnemiesTimeCount();
                GoToNextRound();
                break;

            case GameStates.OnGameEnd:
                UIManager.instance.IncreaseEnemiesTimeCount();
                GoToTitleScreen();
                break;

            case GameStates.OnGamePaused:
                if (tutorialController.IsRunning())
                    tutorialPauseMenuController.HandlePause();
                else
                    pauseMenuController.HandlePause();
                break;
        }
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
    public void OnTutorialFinished(FadeCallback callback = null)
    {
        screenFadeController.FadeToTransparent(callback != null ? callback : null); // StartNextRound);
    }

    public void OnRoundWon()
    {
        Debug.Log("Round (index) " + aiSpawnController.GetCurrentRoundIndex() + " finished!");
        player.OnRoundOver();

        if (aiSpawnController.HasNextRound())  
        {
            OnRoundEnd();
        }
        else
        {
            Debug.Log("No more rounds available!");
            OnGameWon();
        }
    }

    public void OnRoundEnd()
    {
        if (gameState == GameStates.InGame)
        {
//            crosshair.SetActive(false);

            roundScore.gameObject.SetActive(true);
            StatsManager.instance.GetMaxCombo().GrantReward();
            StatsManager.instance.GetTimeCombo().GrantReward();
            StatsManager.instance.GetReceivedDamageCombo().GrantReward();
            roundScore.SetUpTotalScore(StatsManager.instance.GetGlobalPoints());
            gameState = GameStates.OnRoundEnd;
        }
    }

    public void OnGameWon()
    {
        if (gameState == GameStates.InGame)
        {
            //crosshair.SetActive(false);

            //Debug.Log("TODO: Still same code in OnRoundEnd");
            roundScore.gameObject.SetActive(true);
            StatsManager.instance.GetMaxCombo().GrantReward();
            StatsManager.instance.GetTimeCombo().GrantReward();
            StatsManager.instance.GetReceivedDamageCombo().GrantReward();
            roundScore.SetUpTotalScore(StatsManager.instance.GetGlobalPoints());

            gameState = GameStates.OnGameEnd;   
        }
    }

    public void OnGameLost()
    {
        if (gameState == GameStates.InGame)
        {
            player.OnRoundOver();
            //crosshair.SetActive(false);
            gameOverPanel.SetActive(true);
            gameOverPanel.SetActive(true);
            UIManager.instance.ChangeRoundEndText("YOU LOSE!");
            UIManager.instance.ChangeEndBtnText("Go To Title Screen");
            gameState = GameStates.OnGameEnd;
        }
    }

    public void OnGamePaused()
    {
        if (gameState == GameStates.InGame)
        {
            Time.timeScale = 0.0f;
            //crosshair.SetActive(false);

            if (tutorialController.IsRunning())
                tutorialPauseMenuController.gameObject.SetActive(true);
            else
                pauseMenuController.gameObject.SetActive(true);

            gameIsPaused = true;

            gameState = GameStates.OnGamePaused;
        }
    }

    public void ResumeGamePaused()
    {
        if (gameState == GameStates.OnGamePaused)
        {
            if (tutorialController.IsRunning())
            {
                tutorialPauseMenuController.gameObject.SetActive(false);
            }
            else
            {
                pauseMenuController.gameObject.SetActive(false);
                //crosshair.SetActive(true);
            }

            Time.timeScale = 1.0f;

            gameIsPaused = false;

            gameState = GameStates.InGame;
        }
    }

    public void SkipTutorial()
    {
        if (gameState == GameStates.OnGamePaused)
        {
            //crosshair.SetActive(true);
            tutorialPauseMenuController.gameObject.SetActive(false);
            tutorialController.RequestEndTutorial();

            Time.timeScale = 1.0f;

            gameIsPaused = false;

            gameState = GameStates.InGame;
        }
    }

    public Player GetPlayer1()
    {
        return player;
    }

    public void GoToNextRound()
    {
        if (InputManager.instance.GetXButtonDown())
        {
            //crosshair.SetActive(true);
            gameOverPanel.SetActive(false);
            StatsManager.instance.ResetKillCounts();
            StatsManager.instance.ResetBadComboCount();
            StatsManager.instance.ResetGlobalPoins();
            StatsManager.instance.GetMaxCombo().ResetCount();
            StatsManager.instance.GetTimeCombo().ResetCount();
            StatsManager.instance.GetReceivedDamageCombo().ResetCount();
            gameState = GameStates.InGame;

            StartNextRound();
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
        Time.timeScale = 1.0f;
        gameIsPaused = false;
        gameState = GameStates.InGame;
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    public void StartNextRound()
    {
        aiSpawnController.StartNextRound();
        UIManager.instance.indicatorsController.OnNewRoundStarted();
        player.OnRoundStarted();
    }
    #endregion

    #region Private Methods
#if UNITY_EDITOR
    private void EditorPaused(PauseState state)
    {
        if (state == PauseState.Paused)
            OnApplicationPause(true);
    }
#endif
    #endregion
}
