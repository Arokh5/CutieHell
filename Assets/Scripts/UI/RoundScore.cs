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
    private GameObject achievementScorePrefab;
    [SerializeField]
    private Transform achievementsDisplayPosition;
    private bool singleAchievementFullDisplayed =  true;
    private int currentEvaluatedAchievementIterator = 0;
    private int playerTotalRoundScoreOnCurrentAchievement;
    [SerializeField]
    private int separationOnXaxisBetweenAchievementsIcons;

    [Header ("Score")]
    private Text[] scoresCounts;
    [SerializeField]
    private Text currentScore; //the score value at the current frame
    private int currentScoreTarget; //the target value for current score having added the last evaluated score
    private int currentScoreValue = 0;
    [SerializeField]
    private Text total;


    private ShowingState showingState;

    private List<Combo> obtainedAchievements = new List<Combo>();
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
        SetUpAchievementsToDisplay();
        Time.timeScale = 0;
	}
	
	// Update is called once per frame
	void Update () 
	{
        currentScore.text = currentScoreValue.ToString();

        switch (showingState)
        {
            case ShowingState.ACHIEVEMENTS:
                if (obtainedAchievements.Count != 0 && obtainedAchievements != null)
                    ShowAchievements();
                else
                    showingState = ShowingState.STATS;
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
        if (!obtainedAchievements.Contains(achievement))
        {
            obtainedAchievements.Add(achievement);
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

    private void ShowAchievements()
    {
        if(singleAchievementFullDisplayed)
        {
            singleAchievementFullDisplayed = false;
            achievementsDisplayPosition.localPosition = new Vector3(achievementsDisplayPosition.localPosition.x + separationOnXaxisBetweenAchievementsIcons, achievementsDisplayPosition.localPosition.y, achievementsDisplayPosition.localPosition.z);
            
            GameObject achievementScore = Instantiate(achievementScorePrefab, this.transform);
            achievementScore.transform.localPosition = achievementsDisplayPosition.localPosition;
            achievementScore.GetComponent<Image>().sprite = obtainedAchievements[currentEvaluatedAchievementIterator].comboIcon;
            achievementScore.GetComponentInChildren<Text>().text = "x" + obtainedAchievements[currentEvaluatedAchievementIterator].GetTimesObtained();

            currentScoreTarget += obtainedAchievements[currentEvaluatedAchievementIterator].reward;
        }
        else
        {
            if(currentScoreValue >= currentScoreTarget)
            {
                currentScoreValue = currentScoreTarget;
                if(currentEvaluatedAchievementIterator < obtainedAchievements.Count-1)
                {
                    currentEvaluatedAchievementIterator += 1;
                }
                else
                {
                    showingState = ShowingState.STATS;
                }
                singleAchievementFullDisplayed = true;
            }else
            {
                currentScoreValue += 18;
            }
        }
    }

    private void SetUpAchievementsToDisplay()
    {
        achievementsDisplayPosition.localPosition = new Vector3(achievementsDisplayPosition.localPosition.x - (obtainedAchievements.Count * 4), achievementsDisplayPosition.localPosition.y, achievementsDisplayPosition.localPosition.z);
    }

    #endregion
}
