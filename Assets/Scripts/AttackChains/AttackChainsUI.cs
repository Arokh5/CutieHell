using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackChainsUI : MonoBehaviour
{
    #region Fields
    public static AttackChainsUI instance;

    [SerializeField]
    private FollowUpButtonPrompt[] fubPrompts;

    private int activeChainPrompts = 0;
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
    public void UpdateDisplay(List<FollowUpPromptInfo> followUpPromptInfos)
    {
        int fupInfosCount = followUpPromptInfos.Count;

        if (fupInfosCount > 0)
            fupInfosCount = fupInfosCount;

        if (fupInfosCount > fubPrompts.Length)
            Debug.LogWarning("WARNING: AttackChainsUI.UpdateDisplay called in GameObject '" + gameObject.name + "' with more elements than there are Images in its fubPrompts array. Only the first " + fubPrompts.Length + " elements will be displayed!");

        for (int i = 0; i < fubPrompts.Length; ++i)
        {
            FollowUpButtonPrompt fubPrompt = fubPrompts[i];
            if (i < fupInfosCount)
            {
                fubPrompt.RequestShow(followUpPromptInfos[i]);
            }
            else
            {
                fubPrompt.Deactivate();
            }
        }
    }
    #endregion
}
