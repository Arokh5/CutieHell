using UnityEngine;

public class ReceivedDamageCombo : Combo
{

    #region Attributes
    private float roundBaseHealth = 0;
    private float roundFinalHealth = 0;
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
        Debug.Log("TODO: GrantReward() is sending to StatsManager: " + (int)(reward * (roundFinalHealth / roundBaseHealth)));
        StatsManager.instance.IncreaseGlobalPoints((int) (reward * (roundFinalHealth / roundBaseHealth)));
    }
    #endregion
}