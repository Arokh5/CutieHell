using System.Collections.Generic;
using UnityEngine;

public class AttackChainsManager : MonoBehaviour
{
    #region Fields
    public static AttackChainsManager instance;

    [SerializeField]
    private AttackChain[] attackChains;

    private List<AttackChain> activeChains = new List<AttackChain>();
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);
    }
    #endregion

    #region Public Methods
    public void ReportAttack(AttackType attack)
    {
        if (HasActiveChains())
        {
            List<AttackChain> chainsToRemove = new List<AttackChain>();
            foreach (AttackChain chain in activeChains)
            {
                if (chain.CanChain(attack))
                {
                    if (!chain.AdvanceChain())
                    {
                        chainsToRemove.Add(chain);
                    }
                }
                else
                {
                    chainsToRemove.Add(chain);
                }
            }

            foreach (AttackChain chain in chainsToRemove)
            {
                activeChains.Remove(chain);
            }
        }
        else
        {
            foreach (AttackChain chain in attackChains)
            {
                if (chain.StartChain(attack))
                    activeChains.Add(chain);
            }
        }
    }

    public bool TestChain(AttackType attack)
    {
        foreach (AttackChain chain in activeChains)
        {
            if (chain.CanChain(attack))
                return true;
        }
        return false;
    }

    /// <summary>
    /// Returns the State for the first AttackChain that is active
    /// </summary>
    /// <returns></returns>
    public State GetNextChainState()
    {
        if (HasActiveChains())
        {
            return activeChains[0].GetFollowUpState();
        }
        return null;
    }
    #endregion

    #region Private Methods

    private bool HasActiveChains()
    {
        return activeChains.Count != 0;
    }
    #endregion
}
