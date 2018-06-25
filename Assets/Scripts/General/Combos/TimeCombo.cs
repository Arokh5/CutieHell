using UnityEngine;

public class TimeCombo : Combo {

    #region Attributes
    private float beatingTime = 0f;
    [SerializeField]
    private float minimumBeatingTime;
    #endregion

    #region MonoBehaviour methods
    // Use this for initialization
    void Start () 
	{
        Debug.Log("TODO: Temporal implementation, " + comboName + " has to begin right after the tutorial ends");
	}
	
	// Update is called once per frame
	void Update () 
	{
        if(!GameManager.instance.gameIsPaused)
            beatingTime += Time.deltaTime;
	}

    public override void GrantReward()
    {
        Debug.Log("TODO: GrantReward() is sending to StatsManager: " + reward * (int)(minimumBeatingTime - beatingTime));
        StatsManager.instance.IncreaseGlobalPoints(reward * (int)(minimumBeatingTime - beatingTime));
    }
    #endregion
}