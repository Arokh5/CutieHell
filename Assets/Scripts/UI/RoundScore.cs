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

  
    private int consecutiveKillingsScore;
    private int receivedDamageScore;
    private int beatingTimeScore;

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
    #endregion

    #region Private methods
    private void DisplayRoundScore()
    {

        total.gameObject.SetActive(true);
        isFullyDisplayed = true;


    }
    private void CloseRoundScorePopup()
    {
        if(InputManager.instance.GetXButton())
        {
            isFullyDisplayed = false;
            currentScorePresentation = 0;

            total.gameObject.SetActive(false);
            this.gameObject.SetActive(false);

            Time.timeScale = 1;
        }
    }

    #endregion
}
