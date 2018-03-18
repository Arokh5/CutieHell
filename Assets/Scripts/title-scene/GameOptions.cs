using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOptions : MonoBehaviour
{
    #region MonoBehaviour Methods

    private void Update()
    {
        LeaveOptionsMenu();
    }
    #endregion


    #region Private Methods
    void LeaveOptionsMenu()
    {
        if (InputManager.instance.GetOButtonDown())
        {
            SceneManager.LoadScene("TitleScreen", LoadSceneMode.Single);
        }
    }
    #endregion
    
}
