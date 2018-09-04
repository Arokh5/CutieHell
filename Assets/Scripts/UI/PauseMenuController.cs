using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{

    #region Fields
    [SerializeField]
    private MenuButton[] pauseButtons = new MenuButton[4];
    private int pauseIndex = 0;
    [SerializeField]
    private GameObject helpPanel;

    private bool controlsScreenActive = false;
    #endregion

    #region Public Methods
    public void HandlePause()
    {
        if (!controlsScreenActive)
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
                    Resume();
                    break;

                case 1:
                    GameManager.instance.RestartGame();
                    break;

                case 2:
                    helpPanel.SetActive(true);
                    controlsScreenActive = true;
                    break;

                case 3:
                    GameManager.instance.ExitGame();
                    break;
            }
        }

        if (InputManager.instance.GetPS4OptionsDown())
        {
            Resume();
        }
    }

    private void Resume()
    {
        GameManager.instance.RequestResumeGamePaused();
        pauseButtons[pauseIndex].UnselectButton();
        pauseIndex = 0;
        pauseButtons[pauseIndex].SelectButton();
    }

    private void GoBack()
    {
        if (InputManager.instance.GetOButtonDown())
        {
            helpPanel.SetActive(false);
            controlsScreenActive = false;
        }
    }
    #endregion
}
