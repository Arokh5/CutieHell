using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsManager : MonoBehaviour
{

    #region Fields

    public static StatsManager instance;

    [Header("Bad combo")]
    [SerializeField]
    private int enemiesToBadCombo;
    [SerializeField]
    private int badComboPenalty;
    private int currentShotMissed;

    private int basicEnemiesKilled;
    private int conquerorEnemiesKilled;
    private int rangeEnemiesKilled;

    private int globalPoints;

    [Header("Round time")]
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

    public void IncreaseGlobalPoints(int points)
    {
        this.globalPoints += points;
    }

    public void ResetRoundTime()
    {
        roundTime = 0f;
    }

    public void SetRoundState(bool active)
    {
        roundActive = active;
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
        ResetBadComboCount();
        ResetGlobalPoins();
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

    public void IncreaseRoundTime()
    {
        roundTime += Time.deltaTime;
        if (roundTime > roundMaxTime) roundTime = roundMaxTime;
    }

    public void WinRoundPoints()
    {
        IncreaseGlobalPoints((int)Mathf.Round(roundMaxTime - roundTime) * roundTimeReward);
        Debug.Log("You win " + (int)Mathf.Round(roundMaxTime - roundTime) * roundTimeReward + " round points!!");
    }

    public void ResetKillCounts()
    {
        basicEnemiesKilled = 0;
        rangeEnemiesKilled = 0;
        conquerorEnemiesKilled = 0;
    }

    public void ResetGlobalPoins()
    {
        globalPoints = 0;
    }

    // Called by Player when gaining EP (Evil Points)
    public void RegisterEPGained(int epGained)
    {
        Debug.LogError("NOT IMPLEMENTED: StatsManager::RegisterEPGained");
    }

    // Called by Player when gaining EP but losing it due to having reached the max EP (Evil Points)
    public void RegisterEPLost(int epLost)
    {
        Debug.LogError("NOT IMPLEMENTED: StatsManager::RegisterEPLost");
    }

    // Called by Player when using EP (Evil Points)
    public void RegisterEPUsed(int epUsed)
    {
        Debug.LogError("NOT IMPLEMENTED: StatsManager::RegisterEPUsed");
    }

    public void RegisterWeakAttackMissed()
    {
        ++currentShotMissed;
        if (currentShotMissed == enemiesToBadCombo)
        {
            currentShotMissed = 0;
            GameManager.instance.GetPlayer1().AddEvilPoints(-badComboPenalty);
            UIManager.instance.ShowComboText(UIManager.ComboTypes.BadCombo);
        }
    }

    public void RegisterWeakAttackHit()
    {
        currentShotMissed = 0;
    }

    public void ResetBadComboCount()
    {
        currentShotMissed = 0;
    }

    public Combo GetMaxCombo()
    {
        return maxCombo;
    }

    public Combo GetTimeCombo()
    {
        return timeCombo;
    }
    #endregion
}