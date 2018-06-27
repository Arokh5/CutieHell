using UnityEngine;
using UnityEngine.UI;

public class RoundScore : MonoBehaviour {

    #region Attributes
    [Header("Scores Description Texts")]
    [SerializeField]
    private Text consecutiveKilling;
    [SerializeField]
    private Text damageReceived;
    [SerializeField]
    private Text roundTime;
    [SerializeField]
    private Transform total;

    [Header("Scores Description Display Positions")]
    [SerializeField]
    private Transform consecutiveKillingPosition;
    [SerializeField]
    private Transform damageReceivedPosition;
    [SerializeField]
    private Transform roundTimePosition;
    #endregion

    #region MonoBehaviour methods

    // Use this for initialization
    void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
    #endregion

    #region Public methods
    public void DisplayRoundScore()
    {

    }
    #endregion

    #region Private methods

    #endregion
}
