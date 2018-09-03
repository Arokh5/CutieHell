using UnityEngine;
using UnityEngine.UI;

public class RoundVisualGameScore : MonoBehaviour {

    #region Attributes
    [SerializeField]
    private GameObject lockImage;
    [SerializeField]
    private GameObject roundTitle;
    [SerializeField]
    private GameObject roundScore;
    [SerializeField]
    private Text roundScoreText;
    [SerializeField]
    private GameObject achievements;
    [SerializeField]
    private Transform achievementsDisplayPosition;
	#endregion
	
	#region MonoBehaviour methods
	
	#endregion
	
	#region Public methods
	public void DisplayRoundTitle()
    {
        roundTitle.SetActive(true);
    }

    public void DisplayRoundScore()
    {
        roundScore.SetActive(true);
    }

    public void DisplayLock()
    {
        lockImage.gameObject.SetActive(true);
    }

    public void UpdateRoundScore(int value)
    {
        roundScoreText.text = value.ToString();
    }

    public GameObject GetRoundTitle()
    {
        return roundTitle;
    }

    public GameObject GetRoundScore()
    {
        return roundScore;
    }

    public GameObject GetAchievements()
    {
        return achievements;
    }

    public GameObject GetAchievementsDisplayPosition()
    {
        return achievementsDisplayPosition.gameObject;
    }

    #endregion

    #region Private methods

    #endregion
}
