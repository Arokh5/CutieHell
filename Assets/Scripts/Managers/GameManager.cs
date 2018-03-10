using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    #region Public Data

    public static GameManager instance;
    public enum GameStates { OnStartMenu, InGame, OnGameEnd };
    public GameStates gameState;

    [SerializeField]
    private Player player;

    #endregion

    #region Private Serialized Fields

    #endregion

    #region Private Non-Serialized Fields

    #endregion

    #region Properties

    #endregion

    #region MonoBehaviour Methods

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
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
    }

    public void OnGameLost()
    {
        gameState = GameStates.OnStartMenu;
    }

    public Player GetPlayer1()
    {
        return player;
    }

    #endregion

    #region Private Methods

    #endregion
}