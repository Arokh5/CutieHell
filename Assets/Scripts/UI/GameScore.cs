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

    private int gameTotalScore = 0;

    private int[] roundTotalScores = new int[5];
    private List<List<Combo>> roundObtainedCombos = new List<List<Combo>>();
    private List<List<int>> rountCombosTimesObtained = new List<List<int>>();
 	#endregion
	
	#region MonoBehaviour methods
	
	#endregion

	#region Public methods
    public void StoreRoundInformation(int roundScore, List<Combo> obtainedAchievements, List<int> timesObtained)
    {
        int currentRound = GameManager.instance.GetCurrentRoundNum();

        roundTotalScores[currentRound] = roundScore;
        roundObtainedCombos.Add(obtainedAchievements);
        rountCombosTimesObtained.Add(timesObtained);

    }
	#endregion
	
	#region Private methods
	
	#endregion
}
