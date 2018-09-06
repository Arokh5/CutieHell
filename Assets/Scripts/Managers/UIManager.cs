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

    private const float enemiesCountVel = 0.15f;
    #endregion

    #region MonoBehaviour Methods

    private void Awake()
    {
        instance = this;
    }
    #endregion

    #region Public Methods
    public void SetPlayerHealth(float normalizedHealth)
    {
        playerHealthBar.SetHealthBarFill(normalizedHealth);
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