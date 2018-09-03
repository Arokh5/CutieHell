using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameScore : MonoBehaviour {

    #region Attributes
    [Header("GameScore Pop-ups")]
    [SerializeField]
    private RoundVisualGameScore[] roundsVisualsGameScore = new RoundVisualGameScore[5];
    [SerializeField]
    private Image ribbon;
    [SerializeField]
    private GameObject achievementScorePrefab;

    [Header("Sprites")]
    [SerializeField]
    private Sprite gameWonRibbon;
    [SerializeField]
    private Sprite gameWonBackground;
    [SerializeField]
    private Sprite gameLostRibbon;
    [SerializeField]
    private Sprite gameLostBackground;
    [SerializeField]
    private GameObject totalScore;
    [SerializeField]
    private GameObject continueButton;

    [SerializeField]
    private float countingSpeed = 250f;

    [SerializeField]
    private float intervalTime = 0.5f;
    private float elapsedTime = 0f;
    private int indexAchievement = 0;

    private int gameTotalScore = 0;
    private int numOfRoundsCompleted;

    private float currentRoundScoreValue = 0;
    private int currentRoundBeingDisplayed = -1;
    private bool iscurrentRoundScoreCompleted = false;
    private bool iscurrentRoundAchievementsCompleted = false;

    private bool displayingInfo = false;
    private bool followingRoundsAreBlocked = false;

    private int[] roundTotalScores = new int[5];
    private List<List<Sprite>> roundObtainedCombos = new List<List<Sprite>>();
    private List<List<int>> roundCombosTimesObtained = new List<List<int>>();
    #endregion

    #region MonoBehaviour methods
    private void Update()
    {
        if (displayingInfo)
        {
            if (!followingRoundsAreBlocked)
            {
                if (!iscurrentRoundScoreCompleted)
                {
                    UpdateScore();
                }
                else if (!iscurrentRoundAchievementsCompleted)
                {
                    UpdateAchievements();
                }

                if (InputManager.instance.GetXButtonDown())
                {
                    SkipRoundWithoutLock();
                }
            }
            else
            {
                ShowRoundWithLock();
            }
        }
    }
    #endregion

    #region Public methods
    public void StoreRoundInformation(int roundScore, List<Sprite> obtainedAchievementsIcons, List<int> timesObtained)
    {
        int currentRound = GameManager.instance.GetRoundsCompleted() - 1;

        roundTotalScores[currentRound] = roundScore;
        roundObtainedCombos.Add(new List<Sprite>());
        roundCombosTimesObtained.Add(new List<int>());

        for (int i = 0; i < obtainedAchievementsIcons.Count; i++)
        {
            roundObtainedCombos[currentRound] .Add(obtainedAchievementsIcons[i]);
        }

        for (int i = 0; i < timesObtained.Count; i++)
        {
            roundCombosTimesObtained[currentRound].Add(timesObtained[i]);
        }

    }

    public void ShowGameScore(bool gameWon)
    {
        numOfRoundsCompleted = GameManager.instance.GetRoundsCompleted();

        DisplayRightPopup(gameWon);
        PrepareToShowNextRound();
        displayingInfo = true;

    }
    #endregion

    #region Private methods
    private void DisplayRightPopup(bool gameWon)
    {
        this.gameObject.SetActive(true);
        if (gameWon)
        {
            ribbon.sprite = gameWonRibbon;
            this.gameObject.GetComponent<Image>().sprite = gameWonBackground;
        }
        else
        {
            ribbon.sprite = gameLostRibbon;
            this.gameObject.GetComponent<Image>().sprite = gameLostBackground;
        }
    }


    private void UpdateScore()
    {
        //Activation Order is Title>Score>Achievements

        //Check if roundTitle has already been enabled
        if (!roundsVisualsGameScore[currentRoundBeingDisplayed].GetRoundTitle().activeSelf)
        {
            roundsVisualsGameScore[currentRoundBeingDisplayed].GetRoundTitle().SetActive(true);
        }
        else
        {
            //Check if roundScore has already been enabled
            if (!roundsVisualsGameScore[currentRoundBeingDisplayed].GetRoundScore().activeSelf)
            {
                if (elapsedTime >= intervalTime)
                {
                    roundsVisualsGameScore[currentRoundBeingDisplayed].GetRoundScore().SetActive(true);
                    elapsedTime = 0;
                }
            }
            else
            {
                //While showing Score, keep counting till reaching the total round score
                IncrementRoundScore(countingSpeed);
                //Check if roundAchievements has already been enabled and all score has been already computed
                if (iscurrentRoundScoreCompleted)
                {      
                    roundsVisualsGameScore[currentRoundBeingDisplayed].GetAchievements().SetActive(true);
                    elapsedTime = intervalTime;
                 //exception for elapsedTime = 0, we want to enter on next elapsed time check  
                }
            }
        }

        elapsedTime += Time.deltaTime;
    }

    private void UpdateAchievements()
    {
        
        if (indexAchievement >= roundObtainedCombos[currentRoundBeingDisplayed].Count - 1)
        {
            iscurrentRoundAchievementsCompleted = true;
            PrepareToShowNextRound();
            //TODO Reset all info for next round
        }
        else if (elapsedTime >= intervalTime)
        {
            GameObject achievementScore = Instantiate(achievementScorePrefab, this.transform);
            achievementScore.transform.parent = roundsVisualsGameScore[currentRoundBeingDisplayed].GetAchievements().transform;
            achievementScore.transform.localPosition = roundsVisualsGameScore[currentRoundBeingDisplayed].GetAchievementsDisplayPosition().transform.localPosition;
            achievementScore.transform.localPosition = new Vector3(achievementScore.transform.localPosition.x + 50 * indexAchievement, achievementScore.transform.localPosition.y, achievementScore.transform.localPosition.z);
            achievementScore.transform.localScale = new Vector3(0.9f, 0.9f, 1);
            achievementScore.GetComponent<Image>().sprite = roundObtainedCombos[currentRoundBeingDisplayed][indexAchievement];
            achievementScore.GetComponentInChildren<Text>().text = "x" + roundCombosTimesObtained[currentRoundBeingDisplayed][indexAchievement].ToString();

            indexAchievement += 1;
            elapsedTime = 0;
        }      

        elapsedTime += Time.deltaTime;
    }

    private void PrepareToShowNextRound()
    {

        if (currentRoundBeingDisplayed >= 4)
        {
            displayingInfo = false;
            return;
        }

        if (currentRoundBeingDisplayed >= numOfRoundsCompleted - 1)
        {
            followingRoundsAreBlocked = true;
        }

        iscurrentRoundScoreCompleted = false;
        iscurrentRoundAchievementsCompleted = false;
        ++currentRoundBeingDisplayed;

    }

    private void IncrementRoundScore(float value)
    {
        currentRoundScoreValue += value * Time.deltaTime;
        if (currentRoundScoreValue > roundTotalScores[currentRoundBeingDisplayed])
        {
            currentRoundScoreValue = roundTotalScores[currentRoundBeingDisplayed];
        }

        roundsVisualsGameScore[currentRoundBeingDisplayed].UpdateRoundScore((int)currentRoundScoreValue);

        if (currentRoundScoreValue >= roundTotalScores[currentRoundBeingDisplayed])
        {
            RestartCurrentRoundScore();
            iscurrentRoundScoreCompleted = true;
        }

    }

    private void RestartCurrentRoundScore()
    {
        currentRoundScoreValue = 0;
    }

    private void ShowRoundWithLock()
    {
        roundsVisualsGameScore[currentRoundBeingDisplayed].DisplayLock();
        PrepareToShowNextRound();
    }

    private void SkipRoundWithoutLock()
    {
        currentRoundScoreValue = roundTotalScores[currentRoundBeingDisplayed];
    }
    #endregion
}
