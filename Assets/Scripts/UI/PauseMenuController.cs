using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : MonoBehaviour {

    #region Fields
    public bool isTutorialPauseMenu = false;
    [SerializeField]
    private MenuButton[] pauseButtons = new MenuButton[3];
    private int pauseIndex = 0;
    [SerializeField]
    private GameObject optionsScreen;
    private bool inOptionsScreen = false;
    #endregion

    #region Public Methods
    public void HandlePause()
    {
        if (!inOptionsScreen)
        {
            HandleSelection();
            HandleConfirm();
        }
        else
        {
            GoBack();
        }
    }
    #endregion

    #region Private Methods
    private void HandleSelection()
    {
        if (InputManager.instance.GetPadDownDown() || InputManager.instance.GetLeftStickDownDown())
        {
            pauseButtons[pauseIndex].UnselectButton();

            if (pauseIndex == pauseButtons.Length - 1)
            {
                pauseIndex = 0;
            }
            else
            {
                pauseIndex++;
            }

            pauseButtons[pauseIndex].SelectButton();
        }
        else if (InputManager.instance.GetPadUpDown() || InputManager.instance.GetLeftStickUpDown())
        {
            pauseButtons[pauseIndex].UnselectButton();

            if (pauseIndex == 0)
            {
                pauseIndex = pauseButtons.Length - 1;
            }
            else
            {
                pauseIndex--;
            }

            pauseButtons[pauseIndex].SelectButton();
        }
    }

    private void HandleConfirm()
    {
        if (InputManager.instance.GetXButtonDown())
        {
            
            switch (pauseIndex)
            {
                case 0:
                    if (isTutorialPauseMenu)
                        GameManager.instance.SkipTutorial();
                    else
                        GameManager.instance.RestartGame();
                    break;

                case 1:
                    optionsScreen.SetActive(true);
                    inOptionsScreen = true;
                    break;

                case 2:
                    Time.timeScale = 1.0f;
                    GameManager.instance.gameState = GameManager.GameStates.OnGameEnd;
                    break;
            }
        }

        if (InputManager.instance.GetPS4OptionsDown())
        {
            GameManager.instance.ResumeGamePaused();
        }
    }

    private void GoBack()
    {
        if (InputManager.instance.GetOButtonDown())
        {
            optionsScreen.SetActive(false);
            inOptionsScreen = false;
        }
    }
    #endregion
}
