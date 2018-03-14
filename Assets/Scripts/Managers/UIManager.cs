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
    [SerializeField]
    private List<RadialProgressBar> listRadialProgressBar;

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

    #endregion

    #region Private Methods

    // Called by Player when using or earning Evil Points
    public void SetEvilBarValue(int value)
    {
        Debug.Log("Evil: " + ((float)value / GameManager.instance.GetPlayer1().GetMaxEvilLevel()));
        evilnessBar.GetComponent<Image>().fillAmount = ((float)value / GameManager.instance.GetPlayer1().GetMaxEvilLevel());
    }

    public void SetRadialProgressBar(int idx, float amount)
    {
        RadialProgressBar progressBar = listRadialProgressBar[idx];
        progressBar.SetCurrentAmount(amount);
    }

    // Called by Player when increasing its maximum of Evil Point
    public void SetEvilBarMaxValue(int maxValue)
    {
        Debug.LogError("NOT IMPLEMENTED:UIManager::SetEvilBarMaxValue");
    }
    // Called by ZonesConnection when the connection gets opened
    public void ZoneConnectionOpened(uint zoneConnectionID)
    {
        Debug.LogError("NOT IMPLEMENTED:UIManager::ZoneConnectionOpened");
    }
    // Called by Trap to update its remaining health
    public void SetTrapHealth(uint zoneID, uint TrapID, float normalizedHealth)
    {
        Debug.LogError("NOT IMPLEMENTED:UIManager::SetTrapHealth");
    }

    // Called by Trap to update its remaining health
    public void SetMonumentHealth(uint zoneID, float normalizedHealth)
    {
        Debug.LogError("NOT IMPLEMENTED:UIManager::SetMonumentHealth");
    }

     // Called by AISpawnController to move the Wave indicator forward
    public void SetWaveNumberAndProgress(uint waveNumber, float normalizedProgress)
    {
        Debug.LogError("NOT IMPLEMENTED:UIManager::SetWaveNumberAndProgress");
    }

    #endregion
}