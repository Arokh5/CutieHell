using UnityEngine;
using UnityEngine.UI;

public class RoundInfoController : MonoBehaviour
{
    #region Fields
    [SerializeField]
    private Text currentRoundText;
    [SerializeField]
    private Text totalRoundsText;
    [SerializeField]
    private Text currentWaveText;
    [SerializeField]
    private Text totalWavesText;
    [SerializeField]
    private Text enemiesCountText;
    [SerializeField]
    private FillIndicator waveDelayFillIndicator;
    [SerializeField]
    private Text waveComingPrompt;

    private int currentRoundNumber = -1;
    private int totalRoundsCount = -1;
    private int currentWaveNumber = -1;
    private int totalWavesCount = -1;
    private int enemiesCount = -1;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        UnityEngine.Assertions.Assert.IsNotNull(currentRoundText, "ERROR: Current Round Text (Text) not assigned for RoundInfoController in gameObject '" + gameObject.name + "'!");
        UnityEngine.Assertions.Assert.IsNotNull(totalRoundsText, "ERROR: Current Wave Text (Text) not assigned for RoundInfoController in gameObject '" + gameObject.name + "'!");
        UnityEngine.Assertions.Assert.IsNotNull(currentWaveText, "ERROR: Current Wave Text (Text) not assigned for RoundInfoController in gameObject '" + gameObject.name + "'!");
        UnityEngine.Assertions.Assert.IsNotNull(totalWavesText, "ERROR: Total Waves Text (Text) not assigned for RoundInfoController in gameObject '" + gameObject.name + "'!");
        UnityEngine.Assertions.Assert.IsNotNull(enemiesCountText, "ERROR: Enemies Count Text (Text) not assigned for RoundInfoController in gameObject '" + gameObject.name + "'!");
        UnityEngine.Assertions.Assert.IsNotNull(waveDelayFillIndicator, "ERROR: Wave Delay Fill Indicator (FillIndicator) not assigned for RoundInfoController in gameObject '" + gameObject.name + "'!");
        UnityEngine.Assertions.Assert.IsNotNull(waveComingPrompt, "ERROR: Wave Coming Prompt (Text) not assigned for RoundInfoController in gameObject '" + gameObject.name + "'!");
    }
    #endregion

    #region Public Methods
    public int GetEnemiesCount()
    {
        return enemiesCount;
    }

    public void SetRoundIndicator(int currentRoundNumber, int totalRoundsNumber)
    {
        SetCurrentRound(currentRoundNumber);
        SetTotalRounds(totalRoundsNumber);
    }

    public void SetCurrentRound(int currentRoundNumber)
    {
        if (this.currentRoundNumber != currentRoundNumber)
        {
            this.currentRoundNumber = currentRoundNumber;
            currentRoundText.text = currentRoundNumber.ToString();
        }
    }

    public void SetTotalRounds(int totalRoundsCount)
    {
        if (this.totalRoundsCount != totalRoundsCount)
        {
            this.totalRoundsCount = totalRoundsCount;
            totalRoundsText.text = totalRoundsCount.ToString();
        }
    }

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
        if (normalizedFill == 1.0f)
            normalizedFill = 0.0f;
        waveDelayFillIndicator.SetFill(normalizedFill);
    }

    public void SetWaveDelayVisibility(bool isVisible)
    {
        waveDelayFillIndicator.enabled = isVisible;
    }

    public void SetWaveComingPromptVisibility(bool isVisible)
    {
        waveComingPrompt.gameObject.SetActive(isVisible);
    }
    #endregion
}
