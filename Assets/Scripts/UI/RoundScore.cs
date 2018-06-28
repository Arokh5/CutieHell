using UnityEngine;
using UnityEngine.UI;

public class RoundScore : MonoBehaviour {

    #region Attributes
    [SerializeField]
    private Text consecutiveKillingsCount;
    [SerializeField]
    private Text receivedDamageCount;
    [SerializeField]
    private Text beatingTimeCount;
    private Text[] scoresCounts;

    [SerializeField]
    private Text consecutiveKillingsScore;
    [SerializeField]
    private Text receivedDamageScore;
    [SerializeField]
    private Text beatingTimeScore;
    private Text[] scoresStats;

    [SerializeField]
    private Text total;

    [SerializeField]
    private Transform[] scoresPosition;
    private Transform[] scoresInitialPosition;

    private bool isFullyDisplayed = false;
    private int currentScorePresentation = 0;
    #endregion

    #region MonoBehaviour methods

    // Use this for initialization
    void Start () 
	{
        scoresCounts = new Text[3];
        scoresCounts[0] = consecutiveKillingsCount;
        scoresCounts[1] = receivedDamageCount;
        scoresCounts[2] = beatingTimeCount;

        scoresStats = new Text[3];
        scoresStats[0] = consecutiveKillingsScore;
        scoresStats[1] = receivedDamageScore;
        scoresStats[2] = beatingTimeScore;

        scoresInitialPosition = new Transform[scoresCounts.Length];

        for(int i = 0; i < scoresCounts.Length; i++)
        {
            scoresInitialPosition[i] = scoresCounts[i].transform;
        }
	}
	
	// Update is called once per frame
	void Update () 
	{
        if(!isFullyDisplayed)
        {
            //Debug.Log("TODO: Substitute timeScale 0 for a pause behaviour");
            Time.timeScale = 0;
            DisplayRoundScore();
        }
        else
        {
            CloseRoundScorePopup();
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

        beatingTimeCount.text = minutes + "' " + seconds + "\"";
    }

    public void SetUpConsecutiveKillingsCount(int consecutiveKillings)
    {
        consecutiveKillingsCount.text = consecutiveKillings.ToString() + "!";
    }

    public void SetUpDamageReceivedScore(int damageReceived)
    {
        receivedDamageScore.text = damageReceived.ToString();
    }

    public void SetUpBeatingTimeScore(int beatingTime)
    {
        beatingTimeScore.text = beatingTime.ToString();
    }

    public void SetUpConsecutiveKillingScore(int consecutiveKillings)
    {
        consecutiveKillingsScore.text = consecutiveKillings.ToString();
    }

    public void SetUpTotalScore(int globalScore)
    {
        total.text = globalScore.ToString();
    }
    #endregion

    #region Private methods
    private void DisplayRoundScore()
    {

        if (currentScorePresentation < scoresCounts.Length)
        {
            if ((int)scoresCounts[currentScorePresentation].transform.position.magnitude - (int)scoresPosition[currentScorePresentation].position.magnitude < 0)
            {
                scoresCounts[currentScorePresentation].transform.position = Vector2.Lerp(scoresCounts[currentScorePresentation].transform.position, scoresPosition[currentScorePresentation].position, 0.3f);
            }
            else
            {
                Color scoreStatColorOpaque = scoresStats[currentScorePresentation].color;
                scoreStatColorOpaque.a = 1.0f;
                scoresStats[currentScorePresentation].color = scoreStatColorOpaque;

                currentScorePresentation += 1;
            }
        }
        else
        {
            total.gameObject.SetActive(true);
            isFullyDisplayed = true;
        }

    }
    private void CloseRoundScorePopup()
    {
        if(InputManager.instance.GetXButton())
        {
            isFullyDisplayed = false;
            currentScorePresentation = 0;

            RestartScoresPosition();
            RestartScoresTransparency();

            total.gameObject.SetActive(false);
            this.gameObject.SetActive(false);

            Time.timeScale = 1;
        }
    }

    private void RestartScoresPosition()
    {
        for (int i = 0; i < scoresCounts.Length; i++)
        {
            scoresCounts[i].transform.position = scoresInitialPosition[i].position;
        }
    }

    private void RestartScoresTransparency()
    {
        Color scoreStatColorTransparent = scoresStats[0].color;
        scoreStatColorTransparent.a = 0.0f;

        for(int i = 0; i < scoresStats.Length; i++)
        {
            scoresStats[i].color = scoreStatColorTransparent;
        }

    }
    #endregion
}
