using UnityEngine;
using UnityEngine.UI;

public class EvilManaController : MonoBehaviour {

    #region Attributes
    [Header("Frames")]
    [SerializeField]
    private EvilFrameController evilWeakFrame;
    [SerializeField]
    private EvilFrameController evilStrongFrame;

    private float maxPlayerEvil = 0;
    private bool isMainFrameActive = true;
    #endregion

    #region MonoBehaviour Methods
    // Use this for initialization
    void Start()
    {
        UnityEngine.Assertions.Assert.IsNotNull(evilWeakFrame, "Error : evilWeakFrame is not assigned in GameObject: " + gameObject.name);
        UnityEngine.Assertions.Assert.IsNotNull(evilStrongFrame, "Error: evilStrongFrame is not assigned in GameObject: " + gameObject.name);

        maxPlayerEvil = GameManager.instance.GetPlayer1().GetMaxEvilLevel();
    }

    // Update is called once per frame
    void Update()
    {

    }
    #endregion

    #region Public methods
    public void SetMainFrameActive(bool active)
    {
        isMainFrameActive = active;
    }

    public bool GetMainFrameActive()
    {
        return isMainFrameActive;
    }

    public void UpdateCurrentEvil(float currentEvil)
    {
        if (isMainFrameActive)
        {
            evilStrongFrame.UpdateFragmentsFiller(currentEvil);

            //Check if mainFrame has to be deactivated
            if (currentEvil <= maxPlayerEvil / 2)
            {
                evilStrongFrame.gameObject.SetActive(false);
                isMainFrameActive = false;

                evilWeakFrame.UpdateFragmentsFiller(currentEvil);
            }
        }
        else
        {
            evilWeakFrame.UpdateFragmentsFiller(currentEvil);

            //Check if mainFrame has to be activated
            if (currentEvil > maxPlayerEvil / 2)
            {
                evilStrongFrame.gameObject.SetActive(true);
                isMainFrameActive = true;

                evilStrongFrame.UpdateFragmentsFiller(currentEvil);
            }
        }
    }
    #endregion

    #region Private methods

    #endregion
}
