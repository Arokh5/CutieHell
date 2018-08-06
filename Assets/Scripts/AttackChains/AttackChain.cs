using UnityEngine;

[System.Serializable]
public class AttackChain
{
    #region Fields
    [Header("Attack Chain")]
    public string name;
    public AttackType startAttack;
    public FollowUpAttack[] followUps;

    private int followUpIndex = -1;
    #endregion

    #region Public Methods
    public bool StartChain(AttackType attack)
    {
        bool success = false;
        if (followUps.Length != 0 && startAttack == attack)
        {
            followUpIndex = 0;
            success = true;
        }
        return success;
    }

    /// <summary>
    /// Returns true if the attack is the next expected attack in the chain. This method does NOT alter the state of the chain.
    /// </summary>
    /// <param name="attack">The AttackType to test for chaining</param>
    public bool CanChain(AttackType attack)
    {
        FollowUpAttack currentFollowUp = GetCurrentFollowUpAttack();
        if (currentFollowUp != null)
        {
            return currentFollowUp.attack == attack;
        }
        return false;
    }
    
    /// <summary>
    /// Returns true if the Chain is successfully advanced.
    /// Returns false if the chain had not been started, or if the chain has reached its end.
    /// In the latter, the chain is put back into an unstarted state.
    /// </summary>
    public bool AdvanceChain()
    {
        bool success = false;

        if (followUpIndex != -1)
        {
            if (followUpIndex + 1 < followUps.Length)
            {
                ++followUpIndex;
                success = true;
            }
            else
            {
                ResetChain();
            }
        }

        return success;
    }

    public void ResetChain()
    {
        followUpIndex = -1;
    }

    public State GetFollowUpState()
    {
        if (followUpIndex != -1)
            return followUps[followUpIndex].relatedState;
        else
            return null;
    }
    #endregion

    #region Private Methods
    private FollowUpAttack GetCurrentFollowUpAttack()
    {
        if (followUpIndex != -1)
            return followUps[followUpIndex];
        else
            return null;
    }
    #endregion
}
