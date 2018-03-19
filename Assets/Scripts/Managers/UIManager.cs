using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Fields

    public static UIManager instance;

    [SerializeField]
    private GameObject evilnessBar;

    private int currentWaveNumber = -1;
    [SerializeField]
    private Text waveNumberText;

    [SerializeField]
    private RadialProgressBar waveRadialProgressBar;
    [SerializeField]
    private List<RadialProgressBar> monumentsRadialProgressBars;
    [SerializeField]
    private List<RadialProgressBar> zone1TrapsRadialProgressBars;

    [SerializeField]
    private GameObject repairTrapText;

    #endregion

    #region Properties

    #endregion

    #region MonoBehaviour Methods

    private void Awake()
    {
        instance = this;
    }

    #endregion

    #region Public Methods
    // Called by Player when using or earning Evil Points
    public void SetEvilBarValue(int value)
    {
        evilnessBar.GetComponent<Image>().fillAmount = ((float)value / GameManager.instance.GetPlayer1().GetMaxEvilLevel());
    }

    // Called by Player when increasing its maximum of Evil Point
    public void SetEvilBarMaxValue(int maxValue)
    {
        Debug.LogError("NOT IMPLEMENTED:UIManager::SetEvilBarMaxValue");
    }

    // Called by ZonesConnection when the connection gets opened
    public void ZoneConnectionOpened(int zoneConnectionID)
    {
        Debug.LogError("NOT IMPLEMENTED:UIManager::ZoneConnectionOpened");
    }
    // Called by Trap to update its remaining health
    public void SetTrapConquerRate(int zoneID, int trapID, float normalizedConquerRate)
    {
        if (zoneID == 0)
        {
            RadialProgressBar progressBar = zone1TrapsRadialProgressBars[trapID];
            progressBar.SetNormalizedAmount(normalizedConquerRate);
        }
    }

    // Called by Trap to update its remaining health
    public void SetMonumentConquerRate(int zoneID, float normalizedConquerdRate)
    {
        RadialProgressBar progressBar = monumentsRadialProgressBars[zoneID];
        progressBar.SetNormalizedAmount(normalizedConquerdRate);
    }

    // Called by AISpawnController to move the Wave indicator forward
    public void SetWaveNumberAndProgress(int waveNumber, float normalizedProgress)
    {
        if (currentWaveNumber != waveNumber)
        {
            currentWaveNumber = waveNumber;
            waveNumberText.text = "Wave: " + currentWaveNumber;
        }
        waveRadialProgressBar.SetNormalizedAmount(normalizedProgress);
    }

    public void ShowRepairTrapText()
    {
        repairTrapText.SetActive(true);
    }

    public void HideRepairTrapText()
    {
        repairTrapText.SetActive(false);
    }

    #endregion

    #region Private Methods

    #endregion
}