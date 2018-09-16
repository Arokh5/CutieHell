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
    [SerializeField]
    private GameObject challengesPanel;

    [Header("SFX")]
    [SerializeField]
    private AudioClip changeSelectionClip;
    [SerializeField]
    private AudioClip clickClip;
    [SerializeField]
    private AudioClip backClip;

    private bool controlsScreenActive = false;
    private bool challengesScreenActive = false;
    #endregion

    #region Public Methods
    public void HandlePause()
    {
        if (!controlsScreenActive && !challengesScreenActive)
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
            PlayChangeSelectionClip();
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
            PlayChangeSelectionClip();
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
            PlayClickClip();
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
                    challengesPanel.SetActive(true);
                    challengesScreenActive = true;
                    break;

                case 4:
                    GameManager.instance.ExitGame();
                    break;
            }
        }

        if (InputManager.instance.GetPS4OptionsDown())
        {
            PlayClickClip();
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
            PlayBackClip();
            helpPanel.SetActive(false);
            controlsScreenActive = false;
            challengesPanel.SetActive(false);
            challengesScreenActive = false;
        }
    }

    private void PlayChangeSelectionClip()
    {
        if (changeSelectionClip)
            SoundManager.instance.PlaySfxClip(changeSelectionClip);
    }

    private void PlayClickClip()
    {
        if (clickClip)
            SoundManager.instance.PlaySfxClip(clickClip);
    }
    private void PlayBackClip()
    {
        if (backClip)
            SoundManager.instance.PlaySfxClip(backClip);
    }
    #endregion
}
