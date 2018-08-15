using UnityEngine;

public class ReceivedDamageCombo : Combo
{

    #region Attributes
    private float roundBaseHealth = 0;
    private float roundFinalHealth = 0;
    [SerializeField]
    private RoundScore roundScore;

    #endregion

    #region MonoBehaviour methods

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddMonumentRoundBaseHealth(float monumentRoundBaseHealth)
    {
        roundBaseHealth += monumentRoundBaseHealth;
    }

    public void AddMonumentRoundFinalHealth(float monumentRoundFinalHealth)
    {
        roundFinalHealth += monumentRoundFinalHealth;
    }

    public override void GrantReward()
    {
        score = (int)(reward * (roundFinalHealth / roundBaseHealth));
        roundScore.SetUpDamageReceivedCount(100 - (roundFinalHealth / roundBaseHealth) * 100);
        roundScore.SetUpDamageReceivedScore(score);
        StatsManager.instance.IncreaseRoundPoints(score);
    }
    #endregion
}