using UnityEngine;
using UnityEngine.UI;

public enum AchievementType { CONSECUTIVEKILLING}
public class Achievements : Combo {

    public static Achievements instance;

    #region Attributes
    [SerializeField]
    private KillingCountAchievement[] killingCountAchievements;
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
            }
        }
    }
    #endregion

    #region Private methods

    #endregion
}
