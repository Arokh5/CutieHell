using UnityEngine;

public delegate void EvilLimiterCallback();

public class EvilLimiter : MonoBehaviour
{
    private EvilLimiterCallback callback = null;

    private enum ControlType
    {
        NOT_LESS_THAN,
        NOT_MORE_THAN,
        IN_RANGE
    }

    private enum ActionType
    {
        MAINTAIN,
        COMPENSATE
    }

    #region Fields
    [SerializeField]
    private ControlType controlType;
    [SerializeField]
    private ActionType actionType;
    public int lowLimit;
    public int highLimit;

    private Player player;
    #endregion

    #region MonoBehaviour Methods
    private void Start()
    {
        player = GameManager.instance.GetPlayer1();
    }

    private void Update()
    {
        bool actionTaken = false;
        switch (controlType)
        {
            case ControlType.NOT_LESS_THAN:
                actionTaken |= CheckLowLimit();
                break;
            case ControlType.NOT_MORE_THAN:
                actionTaken |= CheckHighLimit();
                break;
            case ControlType.IN_RANGE:
                actionTaken |= CheckLowLimit();
                actionTaken |= CheckHighLimit();
                break;
            default:
                break;
        }

        if (actionTaken && callback != null)
        {
            callback();
        }
    }
    #endregion

    #region Public Methods
    public void RegisterCallback(EvilLimiterCallback callback)
    {
        this.callback = callback;
    }
    #endregion

    #region Private Methods
    private bool CheckLowLimit()
    {
        float minDif = lowLimit - player.GetEvilLevel();
        if (minDif > 0)
        {
            if (actionType == ActionType.MAINTAIN)
                player.AddEvilPoints(minDif);
            else if (actionType == ActionType.COMPENSATE)
                player.AddEvilPoints(minDif + highLimit - lowLimit);

            return true;
        }
        return false;
    }

    private bool CheckHighLimit()
    {
        float maxDif = player.GetEvilLevel() - highLimit;
        if (maxDif > 0)
        {
            if (actionType == ActionType.MAINTAIN)
                player.AddEvilPoints(-maxDif);
            else if (actionType == ActionType.COMPENSATE)
                player.AddEvilPoints(-(maxDif + highLimit - lowLimit));

            return true;
        }
        return false;
    }
    #endregion
}
