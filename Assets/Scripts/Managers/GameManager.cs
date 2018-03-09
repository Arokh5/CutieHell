using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    #region Public Data

    public static GameManager instance;
    public enum GameStates { Init, Cutscene, Management, Wave, Stats, GameOver };

    public GameStates gameState;

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

    #endregion

    #region Public Methods
    public void OnGameWon()
    {
        Debug.LogError("NOT IMPLEMENTED: GameManager::OnGameWon");
    }

    public void OnGameLost()
    {
        Debug.LogError("NOT IMPLEMENTED: GameManager::OnGameLost");
    }

    public Player GetPlayer1()
    {
        Debug.LogError("NOT IMPLEMENTED: GameManager::GetPlayer1");
        return null;
    }
	#endregion
	
	#region Private Methods
	
	#endregion
}