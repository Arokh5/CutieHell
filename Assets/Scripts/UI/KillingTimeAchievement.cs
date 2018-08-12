using UnityEngine;

public enum TimeLimitation { Timed, Wave, Round }

public class KillingTimeAchievement : Combo
{
    #region Attributes
    [SerializeField]
    private Sprite achievementIcon;
    [SerializeField]
    TimeLimitation timeLimitation;

    private Player player;
    #endregion

    #region MonoBehaviour methods

    #endregion

    // Use this for initialization
    void Start()
    {
        player = GameManager.instance.GetPlayer1();
    }

    // Update is called once per frame
    void Update()
    {
        ReviewConditions();        
    }

    #region Public methods

    public override void GrantReward()
    {
        StatsManager.instance.IncreaseGlobalPoints(reward);
        TransitionUI.instance.AskForTransition(comboName, achievementIcon);
        ResetCount();
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

    public TimeLimitation GetTimeLimitationType()
    {
        return timeLimitation;
    }
    #endregion

    #region Private methods

    #endregion
}
