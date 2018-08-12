using UnityEngine;
using UnityEngine.UI;

public enum AchievementType { CONSECUTIVEKILLING, CONSECUTIVEHITTING}
public class Achievements : Combo {

    public static Achievements instance;

    #region Attributes
    [SerializeField]
    private KillingCountAchievement[] killingCountAchievements;
    [SerializeField]
    private KillingCountAchievement[] hittingCountAchievements;
    [SerializeField]
    private KillingTimeAchievement[] killingTimeAchievements;
    #endregion
    #region MonoBehaviour methods

    #endregion

    // Use this for initialization
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    #region Public methods
    
    public void IncreaseCurrentCountKillingType(int addToCount, AttackType attackType)
    {
       for (int i = 0; i < killingCountAchievements.Length; i++)
        {
            if(killingCountAchievements[i].GetAttackType() == attackType)
            {
                killingCountAchievements[i].IncreaseCurrentCount(addToCount);
                return;
            }
        }
    }

    //We use same killingCountAchievements list
    public void IncreaseCurrentCountHitType(int addToCount, AttackType attackType)
    {
        for (int i = 0; i < hittingCountAchievements.Length; i++)
        {
            if (hittingCountAchievements[i].GetAttackType() == attackType)
            {
                hittingCountAchievements[i].IncreaseCurrentCount(addToCount);
                return;
            }
        }
    }

    public void IncreaseCurrentTimeKillingType(TimeLimitation timeType, int addToCount = 1)
    {
        for (int i = 0; i < killingTimeAchievements.Length; i++)
        {
            if (killingTimeAchievements[i].GetTimeLimitationType() == timeType)
            {
                killingTimeAchievements[i].IncreaseCurrentCount(addToCount);
                return;
            }
        }
    }
    #endregion

    #region Private methods

    #endregion
}
