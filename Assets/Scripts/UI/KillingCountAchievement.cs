using UnityEngine;
using System.Collections.Generic;

public class KillingCountAchievement : Combo {

    #region Attributes
    [SerializeField]
    private AttackType attackType;

    private List<int> hittedEnemiesIds;

    private Player player;
	#endregion
	
	#region MonoBehaviour methods
	
	#endregion
	
	// Use this for initialization
	void Start () 
	{
        player = GameManager.instance.GetPlayer1();
        hittedEnemiesIds = new List<int>();
    }

    // Update is called once per frame
    void Update()
    {
        ReviewConditions();

        switch (attackType)
        {
            case AttackType.MINE:
                if (currentCount > 0)
                    ResetCount();
                break;
            case AttackType.STRONG:
                if (player.cameraState != Player.CameraState.STRONG_ATTACK)
                    ResetCount();
                break;
            case AttackType.CONE:
                if (player.cameraState != Player.CameraState.CONEATTACK)
                    ResetCount();
                break;
            case AttackType.METEORITE:
                if (!player.GetIsMeteoritesOn() && currentCount > 0)
                    ResetCount();
                break;
        }

    }

    #region Public methods
    public override void GrantReward()
    {
        StatsManager.instance.RegisterAchievement(this);

        StatsManager.instance.IncreaseRoundPoints(reward);
        TransitionUI.instance.AskForTransition(comboName, comboIcon);
        ResetCount();
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

    public List<int> GetHittedEnemiesIDs()
    {
        return hittedEnemiesIds;
    }
    #endregion

    #region Private methods

    #endregion
}
