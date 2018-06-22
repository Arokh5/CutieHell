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
    [Header("Marker")]
    [SerializeField]
    private Image evilBarProgressMarker;
    [SerializeField]
    private float markerRadius;
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
        ActiveProgressMarker();

        float fillPercentage = CalculateFillPercentage();
        CalculateMarkerPosition(fillPercentage);
        evilBarFillness.fillAmount = fillPercentage;
        
    }

    public void CompleteFill()
    {
        Color transparentMarker = evilBarProgressMarker.color;
        transparentMarker.a = 0f;
        evilBarProgressMarker.color = transparentMarker;

        evilBarIcon.color = Color.white;

        evilBarFillness.fillAmount = 1f;

        evilPointState = EvilPointState.FULL;
    }

    public void CompleteVoid()
    {
        Color transparentMarker = evilBarProgressMarker.color;
        transparentMarker.a = 0f;
        evilBarProgressMarker.color = transparentMarker;

        evilBarIcon.color = Color.grey;

        evilBarFillness.fillAmount = 0f;

        evilPointState = EvilPointState.VOID;
    }
   
    #endregion

    #region Private methods
    private void CalculateMarkerPosition(float currentFillPercentage)
    {
        int completeAngleDegrees = 360; 
        float markerAngleRad = (completeAngleDegrees * currentFillPercentage * Mathf.PI) / 180;

        // marker posY will be calculated using angular movement using posY = radius * sen (- markerAngle)
        // marker posY will be calculated using angular movement using posX = radius * cos (- markerAngle)
        float posY = markerRadius * Mathf.Sin(-markerAngleRad);
        float posX = markerRadius * Mathf.Cos(-markerAngleRad);

        evilBarProgressMarker.transform.localPosition = new Vector3(posX, posY);

    }

    private void ActiveProgressMarker()
    {
        Color opaqueMarker = evilBarProgressMarker.color;
        opaqueMarker.a = 255f;
        evilBarProgressMarker.color = opaqueMarker;
    }

    private float CalculateFillPercentage()
    {
        float currentFillPercentage =  evilCurrentAmount - (int) evilCurrentAmount;
        return currentFillPercentage;
    }
    #endregion
}
