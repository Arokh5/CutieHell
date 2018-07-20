using UnityEngine;
using UnityEngine.UI;

public class Achievement : Combo {

    #region Attributes
    [SerializeField]
    private TransitionUI achievementTransition;
    [SerializeField]
    private Image icon;
    [SerializeField]
    private 
    #endregion

    #region MonoBehaviour methods

    #endregion

    // Use this for initialization
    void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.H))
        {
            GrantReward();
        }
	}

    #region Public methods
    public override void GrantReward()
    {
        StatsManager.instance.IncreaseGlobalPoints(reward);
        TransitionUI.instance.AskForTransition(name, icon);
    }

    #endregion

    #region Private methods

    #endregion
}
