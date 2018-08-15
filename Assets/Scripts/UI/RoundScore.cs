using UnityEngine;
using System.Collections.Generic;

using UnityEngine.UI;

public class RoundScore : MonoBehaviour {

    public enum ShowingState { ACHIEVEMENTS, STATS, COMPLETED}
    #region Attributes

    [Header ("SkillStats")]
    [SerializeField]
    private Text consecutiveKillingsCount;
    [SerializeField]
    private Text receivedDamageCount;
    [SerializeField]
    private Text beatingTimeCount;

    private int consecutiveKillingsScore;
    private int receivedDamageScore;
    private int beatingTimeScore;

    [Header("AchievementStats")]
    [SerializeField]
    private Transform AchievementsDisplayPosition;

    [Header ("Score")]
    private Text[] scoresCounts;
    [SerializeField]
    private Text total;


    private ShowingState showingState;

    private Dictionary<Combo, int> obtainedAchievements = new Dictionary<Combo, int>();
    #endregion

    #region MonoBehaviour methods

    // Use this for initialization
    void Start () 
	{
        scoresCounts = new Text[3];
        scoresCounts[0] = consecutiveKillingsCount;
        scoresCounts[1] = receivedDamageCount;
        scoresCounts[2] = beatingTimeCount;

        showingState = ShowingState.ACHIEVEMENTS;
	}
	
	// Update is called once per frame
	void Update () 
	{
        switch (showingState)
        {
            case ShowingState.ACHIEVEMENTS:
                Time.timeScale = 0;

                break;

            case ShowingState.STATS:

                break;

            case ShowingState.COMPLETED:

                CloseRoundScorePopup();
                break;
        }     
        
    }
    #endregion

    #region Public methods
    public void SetUpDamageReceivedCount(float damageReceived)
    {
        receivedDamageCount.text = ( (int) damageReceived).ToString() + "%";
    }

    public void SetUpBeatingTimeCount(float beatingTime)
    {
        string minutes = Mathf.Floor(beatingTime / 60).ToString("00");
        string seconds = (beatingTime % 60).ToString("00");

        beatingTimeCount.text = minutes + ":" + seconds;
    }

    public void SetUpConsecutiveKillingsCount(int consecutiveKillings)
    {
        consecutiveKillingsCount.text = consecutiveKillings.ToString() + "!";
    }

    public void SetUpDamageReceivedScore(int damageReceived)
    {
        receivedDamageScore = damageReceived;
    }

    public void SetUpBeatingTimeScore(int beatingTime)
    {
        beatingTimeScore = beatingTime;
    }

    public void SetUpConsecutiveKillingScore(int consecutiveKillings)
    {
        consecutiveKillingsScore = consecutiveKillings;
    }

    public void SetUpTotalScore(int globalScore)
    {
        total.text = globalScore.ToString();
    }

    public void AddObtainedAchievement(ref Combo achievement)
    {
        if (obtainedAchievements.ContainsKey(achievement))
        {
            obtainedAchievements[achievement] = obtainedAchievements[achievement] + 1;
        }
        else
        {
            obtainedAchievements.Add(achievement, 1);
        }
    }
    #endregion

    #region Private methods
    private void DisplayRoundScore()
    {
        total.gameObject.SetActive(true);

    }
    private void CloseRoundScorePopup()
    {
        if(InputManager.instance.GetXButton())
        {
            total.gameObject.SetActive(false);
            this.gameObject.SetActive(false);

            Time.timeScale = 1;
        }
    }

    #endregion
}
