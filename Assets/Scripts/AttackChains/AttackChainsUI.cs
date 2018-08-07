using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackChainsUI : MonoBehaviour
{
    #region Fields
    public static AttackChainsUI instance;

    [SerializeField]
    private Image[] chainPrompts;

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
    public void UpdateDisplay(List<Sprite> sprites)
    {
        int spritesCount = sprites.Count;
        if (spritesCount > chainPrompts.Length)
            Debug.LogWarning("WARNING: AttackChainsUI.UpdateDisplay called in GameObject '" + gameObject.name + "' with more sprites than there are Images in its Chain Prompts array. Only the first " + chainPrompts.Length + " sprites will be displayed!");

        for (int i = 0; i < chainPrompts.Length; ++i)
        {
            Image chainPrompt = chainPrompts[i];
            if (i < spritesCount)
            {
                chainPrompt.sprite = sprites[i];
                chainPrompt.enabled = true;
            }
            else
            {
                chainPrompt.sprite = null;
                chainPrompt.enabled = false;
            }
        }
    }
    #endregion
}
