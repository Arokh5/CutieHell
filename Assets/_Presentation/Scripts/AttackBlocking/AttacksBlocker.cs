using UnityEngine;

public class AttacksBlocker : MonoBehaviour
{
    #region Fields
    public static AttacksBlocker instance = null;

    [SerializeField]
    private bool minesBlocked = true;
    [SerializeField]
    private bool blackHoleBlocked = true;
    [SerializeField]
    [Range(1, 5)]
    [Tooltip("The amount of times that the triangle button must be pushed for the black hole to appear AFTER it has been unblocked")]
    private int blackHoleClickCount = 5;

    private int blackHoleAttempts = 0;
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
    public void UnblockMines()
    {
        minesBlocked = false;
    }

    public void UnblockBlackHole()
    {
        blackHoleBlocked = false;
    }

    public bool TryUseMines()
    {
        return !minesBlocked;
    }

    public bool TryUseBlackHole()
    {
        bool canUse = false;

        if (!blackHoleBlocked)
        {
            ++blackHoleAttempts;
            canUse = (blackHoleAttempts >= blackHoleClickCount);
        }

        return canUse;
    }
    #endregion
}
