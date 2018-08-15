using UnityEngine;

public class TimeCombo : Combo {

    #region Attributes
    private float beatingTime = 0f;
    [SerializeField]
    private float minimumBeatingTime;
    [SerializeField]
    RoundScore roundScore;
    #endregion

    #region MonoBehaviour methods
    // Use this for initialization
    void Start () 
	{
        //Debug.Log("TODO: Temporal implementation, " + comboName + " has to begin right after the tutorial ends");
	}
	
	// Update is called once per frame
	void Update () 
	{
        if(!GameManager.instance.gameIsPaused)
            beatingTime += Time.deltaTime;
	}

    public override void GrantReward()
    {
        if ((int)(minimumBeatingTime - beatingTime) < 0)
            beatingTime = minimumBeatingTime;
        score = reward * (int)(minimumBeatingTime - beatingTime);

        roundScore.SetUpBeatingTimeCount(beatingTime);
        roundScore.SetUpBeatingTimeScore(score);
        StatsManager.instance.IncreaseRoundPoints(score);
    }
    #endregion
}