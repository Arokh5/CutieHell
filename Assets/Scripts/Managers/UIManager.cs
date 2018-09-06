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
    public HideWhenPlayerDies[] objectsToHide;

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

    public void SetUIElementsVisibility(bool visible)
    {
        for(int i = 0; i <objectsToHide.Length; ++i)
        {
            objectsToHide[i].hide = !visible;
        }
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