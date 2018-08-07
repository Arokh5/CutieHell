using System.Collections.Generic;
using UnityEngine;

public class AttackChainsManager : MonoBehaviour
{
    #region Fields
    public static AttackChainsManager instance;

    [SerializeField]
    private AttackChain[] attackChains;

    [ShowOnly]
    [SerializeField]
    private List<AttackChain> activeChains = new List<AttackChain>();
    private List<AttackChain> chainsToRemove = new List<AttackChain>();
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);

        VerifyActiveChains();
    }

    private void Update()
    {
        // First, verify for chains expiration
        foreach (AttackChain chain in activeChains)
        {
            chain.elapsedTime += Time.deltaTime;
            if (chain.HasExpired())
            {
                chain.ResetChain();
                chainsToRemove.Add(chain);
            }
        }

        foreach (AttackChain chain in chainsToRemove)
        {
            activeChains.Remove(chain);
        }
        chainsToRemove.Clear();

        // Next, update the state of any UI elements
        UpdateUI();
    }
    #endregion

    #region Public Methods
    public bool ReportAttackAttempt(AttackType attack)
    {
        if (HasActiveChains())
        {
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
            chainsToRemove.Clear();
        }
        else
        {
            foreach (AttackChain chain in attackChains)
            {
                if (chain.StartChain(attack))
                    activeChains.Add(chain);
            }
        }

        return HasActiveChains();
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

    private void UpdateUI()
    {

    }

    private bool VerifyActiveChains()
    {
        bool isValid = true;
        
        foreach(AttackChain chain in attackChains)
        {
            isValid &= chain.VerifyValidity();
        }
        
        if (!isValid)
        {
            Debug.LogError("ERROR: Invalid state of the AttackChainsManager in GameObject '" + gameObject.name + "'!");
        }

        return isValid;
    }
    #endregion
}
