using System.Collections.Generic;
using UnityEngine;

public class AttackInfosManager : MonoBehaviour
{
    #region Fields
    public static AttackInfosManager instance;

    public AttackInfo[] attackInfos;
    [HideInInspector]
    public List<ControllerButton> allButtons;
    [HideInInspector]
    public List<AttackType> allAttacks;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);

        VerifyAttackInfos();
        PrepareButtonsList();
        PrepareAttackList();
    }
    #endregion

    #region Public Methods
    public AttackInfo GetAttackInfo(ControllerButton button)
    {
        foreach(AttackInfo info in attackInfos)
        {
            if (info.button == button)
                return info;
        }
        return null;
    }

    public AttackInfo GetAttackInfo(AttackType attack)
    {
        foreach (AttackInfo info in attackInfos)
        {
            if (info.type == attack)
                return info;
        }
        return null;
    }

    public Sprite GetSprite(AttackType attack)
    {
        foreach (AttackInfo info in attackInfos)
        {
            if (info.type == attack)
                return info.buttonSprite;
        }
        return null;
    }
    #endregion

    #region Private Methods
    private bool VerifyAttackInfos()
    {
        bool isValid = true;

        List<ControllerButton> usedButtons = new List<ControllerButton>();
        List<AttackType> usedAttackTypes = new List<AttackType>();
        foreach (AttackInfo info in attackInfos)
        {
            if (usedButtons.Contains(info.button))
            {
                Debug.LogError("ERROR: More than one AttackInfo uses the same ControllerButton (in GameObject '" + gameObject.name + "')!");
                isValid = false;
            }
            else
                usedButtons.Add(info.button);

            if (usedAttackTypes.Contains(info.type))
            {
                Debug.LogError("ERROR: More than one AttackInfo uses the same AttackType (in GameObject '" + gameObject.name + "')!");
                isValid = false;
            }
            else
                usedAttackTypes.Add(info.type);

        }

        return isValid;
    }

    private void PrepareButtonsList()
    {
        allButtons = new List<ControllerButton>();
        foreach (AttackInfo info in attackInfos)
        {
            allButtons.Add(info.button);
        }
    }

    private void PrepareAttackList()
    {
        allAttacks = new List<AttackType>();
        foreach (AttackInfo info in attackInfos)
        {
            allAttacks.Add(info.type);
        }
    }
    #endregion
}
