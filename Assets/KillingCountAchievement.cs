using UnityEngine;

public class KillingCountAchievement : Combo {

    #region Attributes
    [SerializeField]
    private AttackType attackType;
    [SerializeField]
    private Sprite achievementIcon;
	#endregion
	
	#region MonoBehaviour methods
	
	#endregion
	
	// Use this for initialization
	void Start () 
	{
		
	}

    // Update is called once per frame
    void Update()
    {
        ReviewConditions();
        ResetCount();
    }

    #region Public methods
    public override void GrantReward()
    {
        StatsManager.instance.IncreaseGlobalPoints(reward);
        TransitionUI.instance.AskForTransition(comboName, achievementIcon);
    }

    public AttackType GetAttackType()
    {
        return attackType;
    }

    public override void IncreaseCurrentCount(int addToCount)
    {
        currentCount += addToCount;
    }

    public override void ReviewConditions()
    {
        if (currentCount >= score)
        {
            GrantReward();
        }
    }
    #endregion

    #region Private methods

    #endregion
}
