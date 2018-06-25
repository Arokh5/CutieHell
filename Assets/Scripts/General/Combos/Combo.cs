using UnityEngine;

public class Combo : MonoBehaviour
{

    #region Attributes
    public string comboName;
    public string description;
    public int reward;
    protected int currentCount = 0;
    protected bool comboEnabled = false;
    #endregion

    #region MonoBehaviour Methods
    public void Awake()
    {
        ResetCount();
    }
    #endregion
    //Most of the game combos will have different completition conditions. This method will check the combo progress.
    #region Public methods

    public void EnableCombo()
    {
        comboEnabled = true;
    }

    public void DisableCombo()
    {
        comboEnabled = false;
    }

    public virtual void IncreaseCurrentCount(int addToCount)
    {
        currentCount += addToCount;
    }

    public virtual void ResetCount()
    {
        currentCount = 0;
    }

    public virtual void ReviewConditions()
    {

    }

    public virtual void GrantReward()
    {
        StatsManager.instance.IncreaseGlobalPoints(reward);
    }
    #endregion

}