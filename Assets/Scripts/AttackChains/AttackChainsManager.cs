using UnityEngine;

public class AttackChainsManager : MonoBehaviour
{
    #region Fields
    public static AttackChainsManager instance;
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
        throw new System.NotImplementedException("ERROR: AttackChainsManager::ReportAttack NOT implemented!");
    }

    public bool TestChain(AttackType attack)
    {
        throw new System.NotImplementedException("ERROR: AttackChainsManager::TestChain NOT implemented!");
    }

    public State GetNextChainState()
    {
        throw new System.NotImplementedException("ERROR: AttackChainsManager::GetNextChainState NOT implemented!");
    }
    #endregion
}
