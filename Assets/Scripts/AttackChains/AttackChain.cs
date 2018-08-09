using UnityEngine;

[System.Serializable]
public class AttackChain
{
    #region Fields
    [Header("Attack Chain")]
    public string name;
    public AttackType startAttack;
    public FollowUpAttack[] followUps;

    [HideInInspector]
    public float elapsedTime;
    private int followUpIndex = -1;
    #endregion

    #region Public Methods
    public bool VerifyValidity()
    {
        bool isValid = true;

        if (AttackInfosManager.instance.GetAttackInfo(startAttack) == null)
        {
            Debug.LogError("ERROR: In the AttackChain called '" + name + "', the Start Attack doesn't correspond to any AttackInfo defined in the AttackInfosManager!");
        }
        if (followUps.Length == 0)
        {
            Debug.LogError("ERROR: The AttackChain called '" + name + "' has no FollowUpAttacks!");
            isValid = false;
        }
        else
        {
            for (int i = 0; i < followUps.Length; ++i)
            {
                if (AttackInfosManager.instance.GetAttackInfo(followUps[i].attack) == null)
                {
                    Debug.LogError("ERROR: In the AttackChain called '" + name + "', the FollowUpAttack in index " + i + " has a type that doesn't correspond to any AttackInfo defined in the AttackInfosManager!");
                    isValid = false;
                }
            }
        }

        return isValid;
    }

    public bool StartChain(AttackType attack)
    {
        bool success = false;
        if (followUps.Length != 0 && startAttack == attack)
        {
            elapsedTime = 0.0f;
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
            return currentFollowUp.attack == attack
                && currentFollowUp.IsInTimeFrame(elapsedTime);
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
                elapsedTime = 0.0f;
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
        elapsedTime = -1.0f;
        followUpIndex = -1;
    }

    public bool IsInTimeFrame()
    {
        FollowUpAttack followUp = GetCurrentFollowUpAttack();
        if (followUp != null)
        {
            return followUp.IsInTimeFrame(elapsedTime);
        }
        return false;
    }

    public bool HasExpired()
    {
        FollowUpAttack followUp = GetCurrentFollowUpAttack();
        if (followUp != null)
        {
            return elapsedTime > followUp.timing.end;
        }
        return false;
    }

    public State GetFollowUpState()
    {
        if (followUpIndex != -1)
            return followUps[followUpIndex].relatedState;
        else
            return null;
    }

    public AttackType GetFollowUpAttack()
    {
        if (followUpIndex != -1)
            return followUps[followUpIndex].attack;
        else
            return AttackType.NONE;
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
