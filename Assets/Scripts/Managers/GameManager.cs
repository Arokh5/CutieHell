using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    #region Fields

    public static GameManager instance;
    public enum GameStates { OnStartMenu, InGame, OnGameEnd };
    public GameStates gameState;

    [SerializeField]
    private Player player;

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
                break;
        }
    }

    #endregion

    #region Public Methods

    public void OnGameWon()
    {
        gameState = GameStates.OnStartMenu;
        SceneManager.LoadScene("TitleScreen", LoadSceneMode.Single);
    }

    public void OnGameLost()
    {
        gameState = GameStates.OnStartMenu;
        SceneManager.LoadScene("TitleScreen", LoadSceneMode.Single);
    }

    public Player GetPlayer1()
    {
        return player;
    }

    #endregion

    #region Private Methods

    #endregion
}