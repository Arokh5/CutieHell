using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : MonoBehaviour {

    #region Fields
    [SerializeField]
    private MenuButton[] pauseButtons = new MenuButton[3];
    private int pauseIndex = 0;
    #endregion

    #region Public Methods
    public void HandlePause()
    {
        HandleSelection();
        HandleConfirm();
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
            Time.timeScale = 1.0f;

            switch (pauseIndex)
            {
                case 0:
                    GameManager.instance.RestartGame();
                    break;

                case 1:
                    GameManager.instance.gameState = GameManager.GameStates.OnGameEnd;
                    break;
            }
        }

        if (InputManager.instance.GetPS4OptionsDown())
        {
            GameManager.instance.ResumeGamePaused();
        }
    }
    #endregion
}
