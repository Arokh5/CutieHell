using UnityEngine;

public enum TimeLimitation { Timed, Wave, Round }

public class KillingTimeAchievement : Combo
{
    #region Attributes
    [SerializeField]
    TimeLimitation timeLimitation;
    [SerializeField]
    AttackType[] attackTypes;

    private Player player;
    private float[] timeLimit;
    #endregion

    #region MonoBehaviour methods

    #endregion

    // Use this for initialization
    void Start()
    {
        player = GameManager.instance.GetPlayer1();

        timeLimit = new float[attackTypes.Length];
        for (int i = 0; i < timeLimit.Length; ++i)
            timeLimit[i] = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        ReviewConditions();

        for (int i = 0; i < timeLimit.Length; ++i)
        {
            timeLimit[i] -= Time.deltaTime;

            if (timeLimit[i] < 0.0f)
                timeLimit[i] = 0.0f;
        }
    }

    #region Public methods

    public override void GrantReward()
    {
        StatsManager.instance.RegisterAchievement(this);

        StatsManager.instance.IncreaseRoundPoints(reward);
        TransitionUI.instance.AskForTransition(comboName, comboIcon);

        for (int i = 0; i < timeLimit.Length; ++i)
            timeLimit[i] = 0.0f;
    }

    public override void RegisterAttackType(AttackType attackType)
    {
        switch (attackType)
        {
            case AttackType.CONE:
            case AttackType.STRONG:
            case AttackType.METEORITE:
            case AttackType.MINE:
                int i = 0;
                foreach (AttackType type in attackTypes)
                {
                    if (type.Equals(attackType))
                        break;

                    ++i;
                }

                timeLimit[i] = score;
                break;
            default:
                break;
        }
    }

    public override void ReviewConditions()
    {
        bool success = true;
        for (int i = 0; i < timeLimit.Length && success; ++i)
            success = timeLimit[i] != 0.0f;

        if (success)
            GrantReward();
    }

    public TimeLimitation GetTimeLimitationType()
    {
        return timeLimitation;
    }
    #endregion

    #region Private methods

    #endregion
}
