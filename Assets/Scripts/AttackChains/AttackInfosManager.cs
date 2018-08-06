using UnityEngine;

public class AttackInfosManager : MonoBehaviour
{
    #region Fields
    public static AttackInfosManager instance;

    public AttackInfo[] attackInfos;
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
