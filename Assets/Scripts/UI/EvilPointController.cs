using UnityEngine;
using UnityEngine.UI;

public class EvilPointController : MonoBehaviour {

    enum EvilPointState { FULL, VOID, CHARGING }

    #region Attributes
    private EvilPointState evilPointState;
    private float evilCurrentAmount;

    [Header("UI Elements")]
    [SerializeField]
    private Image evilBarFillness;
    [SerializeField]
    private Image evilBarBase;
    [SerializeField]
    private Image evilBarIcon;
    [SerializeField]
    private Image evilBarProgressMarker;
    #endregion

    #region MonoBehaviour methods
    // Use this for initialization
    void Start()
    {
        UnityEngine.Assertions.Assert.IsNotNull(evilBarFillness, "ERROR: No evilBarFillnesS set up in gameobject" + gameObject.name);
        UnityEngine.Assertions.Assert.IsNotNull(evilBarBase, "ERROR: No evilBarBase set up in gameobject" + gameObject.name);
        UnityEngine.Assertions.Assert.IsNotNull(evilBarIcon, "ERROR: No evilBarIcon set up in gameobject" + gameObject.name);
        UnityEngine.Assertions.Assert.IsNotNull(evilBarProgressMarker, "ERROR: No evilBarProgressMarker set up in gameobject" + gameObject.name);
        evilCurrentAmount = GameManager.instance.GetPlayer1().GetMaxEvilLevel();
        evilPointState = EvilPointState.FULL;

    }

    void Update()
    {
        
    }
    #endregion

    #region Public methods

    public void ModifyEvilPoint(float amount)
    {
        evilCurrentAmount = amount;
        if(evilCurrentAmount == 1)
        {

        }else if(evilCurrentAmount == 0)
        {

        }
        else
        {

        }
    }

    public void completeFill()
    {
        Color transparentMarker = evilBarProgressMarker.color;
        transparentMarker.a = 0f;
        evilBarProgressMarker.color = transparentMarker;

        evilBarIcon.color = Color.white;

        evilBarFillness.fillAmount = 1f;

        evilPointState = EvilPointState.FULL;
    }

    public void completeVoid()
    {
        Color opaqueMarker = evilBarProgressMarker.color;
        opaqueMarker.a = 1f;
        evilBarProgressMarker.color = opaqueMarker;

        evilBarIcon.color = Color.grey;

        evilPointState = EvilPointState.VOID;
    }
    #endregion

    #region Private methods

    #endregion
}
