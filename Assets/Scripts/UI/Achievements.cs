using UnityEngine;
using UnityEngine.UI;
using System;

public enum AchievementType { CONSECUTIVEKILLING, CONSECUTIVEHITTING}
public class Achievements : Combo {

    public static Achievements instance;

    #region Attributes
    [Header("Prefabs")]
    [SerializeField]
    private GameObject marksmanPrefab;
    [SerializeField]
    private GameObject slicenDicePrefab;

    [SerializeField]
    private KillingCountAchievement[] killingCountAchievements;
    
    private KillingCountAchievement[] hittingCountAchievements;
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
            if (killingCountAchievements[i].GetAttackType() == attackType)
            {
                killingCountAchievements[i].IncreaseCurrentCount(addToCount);
                return;
            }
        }
    }

    //We use same killingCountAchievements list
    public void IncreaseCurrentCountHitType(int addToCount, AttackType attackType, int achievementID, int enemyID)
    {
        for (int i = 0; i < hittingCountAchievements.Length; i++)
        {
            //if attackType and achievementID (which instance is representing) matches
            if (hittingCountAchievements[i].GetAttackType() == attackType && hittingCountAchievements[i].GetComboID() == achievementID)
            {
                // if hasn't hitted this enemy before
                if(!hittingCountAchievements[i].GetHittedEnemiesIDs().Contains(enemyID))
                {
                    hittingCountAchievements[i].IncreaseCurrentCount(addToCount);
                    hittingCountAchievements[i].GetHittedEnemiesIDs().Add(enemyID);
                    return;
                }         
            }
        }
    }

    public void IncreaseCurrentTimeKillingType(TimeLimitation timeType, int addToCount = 1)
    {
        //for (int i = 0; i < killingTimeAchievements.Length; i++)
        //{
        //    if (killingTimeAchievements[i].GetTimeLimitationType() == timeType)
        //    {
        //        killingTimeAchievements[i].IncreaseCurrentCount(addToCount);
        //        return;
        //    }
        //}
    }

    public int InstanceNewAchievement( GameObject achievement)
    {
        Combo[] requestedAchievementTypeCurrentlyActive = null;
        GameObject achievementInstantiation = null;
        bool arrayNeedsAnIncrement = false;

        if(hittingCountAchievements != null && hittingCountAchievements.Length != 0)
        {
            requestedAchievementTypeCurrentlyActive = new Combo[hittingCountAchievements.Length];
            requestedAchievementTypeCurrentlyActive = hittingCountAchievements;
        }
        //on first request
        else
        {
            hittingCountAchievements = new  KillingCountAchievement[1];
            achievementInstantiation = Instantiate(achievement);
            achievementInstantiation.GetComponent<Combo>().SetComboID(0);
            hittingCountAchievements[0] = achievementInstantiation.GetComponent<KillingCountAchievement>();

            return 0;
        }              

        if(requestedAchievementTypeCurrentlyActive != null)
        {
            for (int i = 0; i < requestedAchievementTypeCurrentlyActive.Length; i++)
            {
                //There's an spot on the array
                if (requestedAchievementTypeCurrentlyActive[i] == null)
                {
                    achievementInstantiation = Instantiate(achievement);
                    achievementInstantiation.GetComponent<Combo>().SetComboID(i);
                    break;
                } //No empty spot on the whole array
                else if (i+1 == requestedAchievementTypeCurrentlyActive.Length)
                {
                    arrayNeedsAnIncrement = true;

                    achievementInstantiation = Instantiate(achievement);

                    Array.Resize(ref requestedAchievementTypeCurrentlyActive, requestedAchievementTypeCurrentlyActive.Length + 1);
                    achievementInstantiation.GetComponent<Combo>().SetComboID(i+1);
                }
            }

            //Add the new achievement instantiation to the corresponding array

            if(arrayNeedsAnIncrement)
                Array.Resize(ref hittingCountAchievements, hittingCountAchievements.Length + 1);
            hittingCountAchievements[achievementInstantiation.GetComponent<Combo>().GetComboID()] = achievementInstantiation.GetComponent<KillingCountAchievement>();          
            return achievementInstantiation.GetComponent<Combo>().GetComboID();
        }

        Debug.LogError("Error finding a new ID for an achievement " + achievement.GetComponent<Combo>().comboName + " instantiation request");
        return int.MaxValue;
    }

    public void DestroyAchievementInstantiation(AchievementType type, int id)
    {

        switch (type)
        {
            case AchievementType.CONSECUTIVEHITTING:
                if(hittingCountAchievements != null)
                {
                   GameObject.Destroy(hittingCountAchievements[id].gameObject);
                }
                break;

            case AchievementType.CONSECUTIVEKILLING:
                if (killingCountAchievements != null)
                {
                    GameObject.Destroy(killingCountAchievements[id].gameObject);
                }
                break;
        }
    }

    public GameObject GetMarksman()
    {
        return marksmanPrefab;
    }

    public GameObject GetSliceNDice()
    {
        return slicenDicePrefab;
    }
    #endregion

    #region Private methods

    #endregion
}
