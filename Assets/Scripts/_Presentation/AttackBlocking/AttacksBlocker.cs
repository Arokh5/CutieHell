using UnityEngine;

public class AttacksBlocker : MonoBehaviour
{
    #region Fields
    public static AttacksBlocker instance = null;
    public bool canUseMines = false;
    public bool canUseBlackHole = false;
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
}
