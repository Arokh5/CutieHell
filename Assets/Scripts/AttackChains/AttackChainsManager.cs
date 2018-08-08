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
    private State nextChainState = null;

    private List<AttackChain> chainsToRemove = new List<AttackChain>();
    private List<Sprite> spritesToDisplay = new List<Sprite>();

    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);
    }

    private void Start()
    {
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
    public bool ReportStartChainAttempt(AttackType attack)
    {
        activeChains.Clear();
        foreach (AttackChain chain in attackChains)
        {
            if (chain.StartChain(attack))
                activeChains.Add(chain);
        }
        return HasActiveChains();
    }

    public bool ReportFollowUpAttempt(AttackType attack)
    {
        nextChainState = null;
        foreach (AttackChain chain in activeChains)
        {
            if (chain.elapsedTime > 0)
            {
                if (chain.CanChain(attack))
                {
                    if (nextChainState == null)
                        nextChainState = chain.GetFollowUpState();

                    if (!chain.AdvanceChain())
                    {
                        chainsToRemove.Add(chain);
                    }
                }
                else
                {
                    chain.ResetChain();
                    chainsToRemove.Add(chain);
                }
            }
        }

        foreach (AttackChain chain in chainsToRemove)
        {
            activeChains.Remove(chain);
        }
        chainsToRemove.Clear();

        return nextChainState != null;
    }

    /// <summary>
    /// Returns the State for the first AttackChain that is active
    /// </summary>
    /// <returns></returns>
    public State GetNextChainState()
    {
        return nextChainState;
    }
    #endregion

    #region Private Methods
    private bool HasActiveChains()
    {
        return activeChains.Count != 0;
    }

    private void UpdateUI()
    {
        spritesToDisplay.Clear();
        foreach (AttackChain chain in activeChains)
        {
            if(chain.IsInTimeFrame())
            {
                Sprite spriteToAdd = AttackInfosManager.instance.GetSprite(chain.GetFollowUpAttack());
                if (spriteToAdd)
                    spritesToDisplay.Add(spriteToAdd);
            }
        }

        AttackChainsUI.instance.UpdateDisplay(spritesToDisplay);
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
