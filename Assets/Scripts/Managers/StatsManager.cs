using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsManager : MonoBehaviour
{

    #region Fields

    public static StatsManager instance;

    private int basicEnemiesKilled;
    private int conquerorEnemiesKilled;
    private int rangeEnemiesKilled;

    private int roundPoints;

    [Header("Round time")]
    [SerializeField]
    RoundScore roundScore;
    [SerializeField]
    private int roundTimeReward;
    [SerializeField]
    private float roundMaxTime;
    private float roundTime;
    private bool roundActive = false;

    [Header("Combos")]
    [SerializeField]
    private MaxCombo maxCombo;
    [SerializeField]
    private TimeCombo timeCombo;
    [SerializeField]
    private ReceivedDamageCombo receivedDamageCombo;

    [SerializeField]
    private GameScore gameScore;
    #endregion

    #region Properties

    public int GetBasicEnemiesKilled()
    {
        return basicEnemiesKilled;
    }

    public int GetConquerorEnemiesKilled()
    {
        return conquerorEnemiesKilled;
    }

    public int GetRangeEnemiesKilled()
    {
        return rangeEnemiesKilled;
    }

    public void IncreaseRoundPoints(int points)
    {
        this.roundPoints += points;
    }

    public void ResetRoundTime()
    {
        roundTime = 0f;
    }

    public void SetRoundState(bool active)
    {
        roundActive = active;
    }

    public void PassRoundScoreInfoToGameScore(int roundScore, List<Combo> obtainedAchievements, List<int> timesObtained)
    {
        gameScore.StoreRoundInformation(roundScore, obtainedAchievements, timesObtained);
    }
    #endregion

    #region MonoBehaviour Methods

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        ResetKillCounts();
        ResetRoundPoints();
        ResetRoundTime();
    }

    private void Update()
    {

        if (roundActive)
        {
            IncreaseRoundTime();
        }
    }

    private void UpdateCombosBasedOnKills()
    {
        maxCombo.IncreaseCurrentCount(1);
    }
    #endregion

    #region Public Methods

    // Called by AIEnemy upon dying
    public void RegisterKill(EnemyType enemyType)
    {
        switch (enemyType)
        {
            case EnemyType.BASIC:
                basicEnemiesKilled++;
                break;

            case EnemyType.CONQUEROR:
                conquerorEnemiesKilled++;
                break;

            case EnemyType.RANGE:
                rangeEnemiesKilled++;
                break;
        }

        UpdateCombosBasedOnKills();
    }

    public void RegisterAchievement(Combo achievement)
    {
        roundScore.AddObtainedAchievement(ref achievement);
    }

    public void IncreaseRoundTime()
    {
        roundTime += Time.deltaTime;
        if (roundTime > roundMaxTime) roundTime = roundMaxTime;
    }

    public void WinRoundPoints()
    {
        IncreaseRoundPoints((int)Mathf.Round(roundMaxTime - roundTime) * roundTimeReward);
    }

    public void ResetKillCounts()
    {
        basicEnemiesKilled = 0;
        rangeEnemiesKilled = 0;
        conquerorEnemiesKilled = 0;
    }

    public void ResetRoundPoints()
    {
        roundPoints = 0;
    }

    public int GetRoundPoints()
    {
        return roundPoints;
    }

    public Combo GetMaxCombo()
    {
        return maxCombo;
    }

    public Combo GetTimeCombo()
    {
        return timeCombo;
    }

    public ReceivedDamageCombo GetReceivedDamageCombo()
    {
        return receivedDamageCombo;
    }
    #endregion
}