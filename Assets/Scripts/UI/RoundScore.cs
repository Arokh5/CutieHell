using UnityEngine;
using System.Collections.Generic;

using UnityEngine.UI;

public class RoundScore : MonoBehaviour {

    public enum ShowingState { ACHIEVEMENTS, STATS, COMPLETED}
    #region Attributes
    [SerializeField]
    private AudioClip roundScoreSFX;
    [Header ("SkillStats")]
    [SerializeField]
    private Text consecutiveKillingsCount;
    [SerializeField]
    private Text receivedDamageCount;
    [SerializeField]
    private Text beatingTimeCount;

    private int currentEvaluatedSkillStatIterator = 0;

    private int consecutiveKillingsReward;
    private int receivedDamageReward;
    private int beatingTimeReward;

    private Text[] scoresCounts;
    private int[] scoresScores;
    private Dictionary<Text, int> skillStatsScores;

    [Header("AchievementStats")]
    [SerializeField]
    private GameObject achievementScorePrefab;
    [SerializeField]
    private Transform achievementsDisplayPosition;
    [SerializeField]
    private int separationOnXaxisBetweenAchievementsIcons;

    private int currentEvaluatedAchievementIterator = 0;

    [Header ("Score")]
    [SerializeField]
    private Text currentScore; //the score value at the current frame
    private int currentScoreTarget; //the target value for current score having added the last evaluated score
    private int currentScoreValue = 0;
    [SerializeField]
    private Text total;
    [SerializeField]
    private int scoreCounterSpeed;

    private bool singleStatFullDisplayed = true;

    private ShowingState showingState;

    private List<Combo> obtainedAchievements = new List<Combo>();
    #endregion

    #region MonoBehaviour methods

    // Use this for initialization
    void Start () 
	{
      
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
                ShowSkillStats();
                break;

            case ShowingState.COMPLETED:

                CloseRoundScorePopup();
                break;
        }
    }
    #endregion

    #region Public methods

    public void ShowRoundScore()
    {
        SoundManager.instance.PlaySfxClip(roundScoreSFX);
        SetUpSkillStats();
        SetUpAchievementsToDisplay();

        showingState = ShowingState.ACHIEVEMENTS;
        Time.timeScale = 0;
    }

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
        receivedDamageReward = damageReceived;
    }

    public void SetUpBeatingTimeScore(int beatingTime)
    {
        beatingTimeReward = beatingTime;
    }

    public void SetUpConsecutiveKillingScore(int consecutiveKillings)
    {
        consecutiveKillingsReward = consecutiveKillings;
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

    private void ShowSkillStats()
    {
        if (singleStatFullDisplayed)
        {
            singleStatFullDisplayed = false;
            scoresCounts[currentEvaluatedSkillStatIterator].transform.parent.gameObject.SetActive(true);

            currentScoreTarget += skillStatsScores[scoresCounts[currentEvaluatedSkillStatIterator]];
        }
        else
        {
            if (currentScoreValue >= currentScoreTarget)
            {
                currentScoreValue = currentScoreTarget;
                if (currentEvaluatedSkillStatIterator < scoresCounts.Length - 1)
                {
                    currentEvaluatedSkillStatIterator += 1;
                }
                else
                {
                    showingState = ShowingState.COMPLETED;
                    currentScore.gameObject.SetActive(false);
                    total.gameObject.SetActive(true);
                }
                singleStatFullDisplayed = true;
            }
            else
            {
                currentScoreValue += scoreCounterSpeed;
            }
        }
    }

    private void SetUpSkillStats()
    {
        scoresCounts = new Text[3];
        scoresCounts[0] = consecutiveKillingsCount;
        scoresCounts[1] = receivedDamageCount;
        scoresCounts[2] = beatingTimeCount;

        scoresScores = new int[3];
        scoresScores[0] = consecutiveKillingsReward;
        scoresScores[1] = receivedDamageReward;
        scoresScores[2] = beatingTimeReward;

        skillStatsScores = new Dictionary<Text, int>();

        for(int i = 0; i < scoresCounts.Length; i++)
        {
            skillStatsScores.Add(scoresCounts[i], scoresScores[i]);
        }
    }

    private void ShowAchievements()
    {
        if(singleStatFullDisplayed)
        {
            singleStatFullDisplayed = false;
            achievementsDisplayPosition.localPosition = new Vector3(achievementsDisplayPosition.localPosition.x + separationOnXaxisBetweenAchievementsIcons, achievementsDisplayPosition.localPosition.y, achievementsDisplayPosition.localPosition.z);
            
            GameObject achievementScore = Instantiate(achievementScorePrefab, this.transform);
            achievementScore.transform.localPosition = achievementsDisplayPosition.localPosition;
            achievementScore.GetComponent<Image>().sprite = obtainedAchievements[currentEvaluatedAchievementIterator].comboIcon;
            achievementScore.GetComponentInChildren<Text>().text = "x" + obtainedAchievements[currentEvaluatedAchievementIterator].GetTimesObtained();

            currentScoreTarget += obtainedAchievements[currentEvaluatedAchievementIterator].reward * obtainedAchievements[currentEvaluatedAchievementIterator].GetTimesObtained();
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
                singleStatFullDisplayed = true;
            }else
            {
                currentScoreValue += scoreCounterSpeed;
            }
        }
    }

    private void SetUpAchievementsToDisplay()
    {
        achievementsDisplayPosition.localPosition = new Vector3(achievementsDisplayPosition.localPosition.x - (obtainedAchievements.Count * 4), achievementsDisplayPosition.localPosition.y, achievementsDisplayPosition.localPosition.z);
    }

    #endregion
}
