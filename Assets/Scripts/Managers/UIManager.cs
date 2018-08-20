using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Fields

    public static UIManager instance;

    public MonumentsHealthBar monumentsHealthBar;
    public MarkersController markersController;
    public RoundInfoController roundInfoController;

    [Header("Player Health")]
    [Space]
    [SerializeField]
    private HealthBar playerHealthBar;
    [SerializeField]
    private GameObject buttonMashPrompt;

    [Header("Round End")]
    [SerializeField]
    private Text basicEnemies;
    [SerializeField]
    private Text rangeEnemies;
    [SerializeField]
    private Text conquerorEnemies;

    [SerializeField]
    private Text roundEndText;
    [SerializeField]
    private Text endBtnText;

    [SerializeField]
    private GameObject slimeDead;
    [SerializeField]
    private GameObject bearDead;
    [SerializeField]
    private GameObject conquerorDead;

    [SerializeField]
    private AudioClip coinSfx;

    private const float enemiesCountVel = 0.15f;
    #endregion

    #region MonoBehaviour Methods

    private void Awake()
    {
        instance = this;
        SetPlayerHealthButtonMashVisibility(false);
    }
    #endregion

    #region Public Methods
    public void SetPlayerHealth(float normalizedHealth)
    {
        playerHealthBar.SetHealthBarFill(normalizedHealth);
    }

    public void SetPlayerHealthButtonMashVisibility(bool isVisible)
    {
        buttonMashPrompt.SetActive(isVisible);
    }

    public void ChangeRoundEndText(string text)
    {
        roundEndText.text = text;
    }

    public void ChangeEndBtnText(string text)
    {
        endBtnText.text = text;
    }

    // Called by ZonesConnection when the connection gets opened
    public void ZoneConnectionOpened(int zoneConnectionID)
    {
        Debug.LogError("NOT IMPLEMENTED:UIManager::ZoneConnectionOpened");
    }
    #endregion

    #region Private Methods
    private void SetLocalXPos(RectTransform rectTransform, float xValue)
    {
        Vector3 refPos = rectTransform.localPosition;
        refPos.x = xValue;
        rectTransform.localPosition = refPos;
    }
    #endregion
}