using UnityEngine;
using UnityEngine.UI;

public class RoundInfoController : MonoBehaviour
{
    #region Fields
    [SerializeField]
    private Text currentWaveText;
    [SerializeField]
    private Text totalWavesText;
    [SerializeField]
    private Text enemiesCountText;
    [SerializeField]
    private FillIndicator waveDelayFillIndicator;
    [SerializeField]
    private Text rushPrompt;

    private int currentWaveNumber = -1;
    private int totalWavesCount = -1;
    private int enemiesCount = -1;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        UnityEngine.Assertions.Assert.IsNotNull(currentWaveText, "ERROR: currentWaveText not assigned for RoundInfoController in gameObject '" + gameObject.name + "'");
        UnityEngine.Assertions.Assert.IsNotNull(totalWavesText, "ERROR: totalWavesText not assigned for RoundInfoController in gameObject '" + gameObject.name + "'");
        UnityEngine.Assertions.Assert.IsNotNull(enemiesCountText, "ERROR: enemiesCountText not assigned for RoundInfoController in gameObject '" + gameObject.name + "'");
        UnityEngine.Assertions.Assert.IsNotNull(waveDelayFillIndicator, "ERROR: waveDelayFillIndicator not assigned for RoundInfoController in gameObject '" + gameObject.name + "'");
    }
    #endregion

    #region Public Methods
    public void SetWaveIndicator(int currentWaveNumber, int totalWavesNumber)
    {
        SetCurrentWave(currentWaveNumber);
        SetTotalWaves(totalWavesNumber);
    }

    public void SetCurrentWave(int currentWaveNumber)
    {
        if (this.currentWaveNumber != currentWaveNumber)
        {
            this.currentWaveNumber = currentWaveNumber;
            currentWaveText.text = currentWaveNumber.ToString();
        }
    }

    public void SetTotalWaves(int totalWavesCount)
    {
        if (this.totalWavesCount != totalWavesCount)
        {
            this.totalWavesCount = totalWavesCount;
            totalWavesText.text = totalWavesCount.ToString();
        }
    }

    public void SetEnemiesCount(int enemiesCount)
    {
        if (this.enemiesCount != enemiesCount)
        {
            this.enemiesCount = enemiesCount;
            enemiesCountText.text = enemiesCount.ToString();
        }
    }

    public void AddToEnemiesCount(int numberToAdd)
    {
        if (numberToAdd != 0)
        {
            this.enemiesCount += numberToAdd;
            enemiesCountText.text = this.enemiesCount.ToString();
        }
    }

    public void SetWaveDelayFill(float normalizedFill)
    {
        normalizedFill = Mathf.Clamp01(normalizedFill);
        waveDelayFillIndicator.SetFill(normalizedFill);
    }

    public void SetWaveDelayVisibility(bool isVisible)
    {
        waveDelayFillIndicator.enabled = isVisible;
    }

    public void SetRushPromptVisibility(bool isVisible)
    {
        rushPrompt.enabled = isVisible;
    }
    #endregion
}
