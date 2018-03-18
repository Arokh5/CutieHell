using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameCredits : MonoBehaviour
{
    #region MonoBehaviour Methods

    private void Update()
    {
        LeaveCreditsMenu();
    }
    #endregion


    #region Private Methods
    void LeaveCreditsMenu()
    {
        if (InputManager.instance.GetOButtonDown())
        {
            SceneManager.LoadScene("TitleScreen", LoadSceneMode.Single);
        }
    }
    #endregion

}
