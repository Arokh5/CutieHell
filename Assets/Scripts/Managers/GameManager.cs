using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    #region Fields

    public static GameManager instance;
    public enum GameStates { OnStartMenu, InGame, OnGameEnd };
    public GameStates gameState;

    [SerializeField]
    private Player player;
    [SerializeField]
    private GameObject gameOverPanel;

    #endregion

    #region Properties

    #endregion

    #region MonoBehaviour Methods

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        switch (gameState)
        {
            case GameStates.OnStartMenu:
                break;

            case GameStates.InGame:
                break;

            case GameStates.OnGameEnd:
                GoToTitleScreen();
                break;
        }
    }

    #endregion

    #region Public Methods

    public void OnWaveWon()
    {
        OnGameWon();
        Debug.LogError("Substitute the current OnGameWon for the own OnWaveWon logic");
    }

    public void OnGameWon()
    {
        gameOverPanel.SetActive(true);
        gameOverPanel.transform.GetChild(1).GetComponent<Text>().text = "YOU WIN!";
        gameState = GameStates.OnGameEnd;
    }

    public void OnGameLost()
    {
        gameOverPanel.SetActive(true);
        gameOverPanel.transform.GetChild(1).GetComponent<Text>().text = "YOU LOSE!";
        gameState = GameStates.OnGameEnd;
    }

    public Player GetPlayer1()
    {
        return player;
    }

    public void GoToTitleScreen()
    {
        if (InputManager.instance.GetXButtonDown())
        {
            gameState = GameStates.OnStartMenu;
            SceneManager.LoadScene("TitleScreen", LoadSceneMode.Single);
        }
    }

    #endregion

    #region Private Methods

    #endregion
}