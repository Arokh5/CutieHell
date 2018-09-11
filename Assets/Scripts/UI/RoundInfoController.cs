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
    [SerializeField]
    [Tooltip("The time (in seconds) that defines when the waveComingPrompt is shown. If the time left for the next wave is less that this value, the prompt is shown. Otherwise it remains hidden.")]
    [Range(0.0f, 50.0f)]
    private float promptTimeThreshold = 10.0f;

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

    public void SetWaveComingInfo(float timeLeft, float totalTime)
    {
        if (timeLeft <= 0.0f || timeLeft > promptTimeThreshold)
        {
            SetWaveDelayFill(0.0f);
            SetWaveDelayVisibility(false);
            SetWaveComingPromptVisibility(false);
        }
        else
        {
            // Here we show it the prompt
            float maxTime = Mathf.Min(promptTimeThreshold, totalTime);
            SetWaveDelayFill(1.0f - (timeLeft / maxTime));
            SetWaveDelayVisibility(true);
            SetWaveComingPromptVisibility(true);
        }
    }

    public void HideWaveComingInfo()
    {
        SetWaveComingInfo(0.0f, 0.0f);
    }
    #endregion

    #region Private Methods
    private void SetWaveDelayFill(float normalizedFill)
    {
        normalizedFill = Mathf.Clamp01(normalizedFill);
        if (normalizedFill == 1.0f)
            normalizedFill = 0.0f;
        waveDelayFillIndicator.SetFill(normalizedFill);
    }

    private void SetWaveDelayVisibility(bool isVisible)
    {
        waveDelayFillIndicator.enabled = isVisible;
    }

    private void SetWaveComingPromptVisibility(bool isVisible)
    {
        waveComingPrompt.gameObject.SetActive(isVisible);
    }
    #endregion
}
