using UnityEngine;
using UnityEngine.UI;

public class MineCounterUI : MonoBehaviour
{
    #region Fields
    [SerializeField]
    private Text currentCount;
    [SerializeField]
    private Text totalCount;
    [SerializeField]
    private Image sprite;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        UnityEngine.Assertions.Assert.IsNotNull(currentCount, "ERROR: Current Count (Text) not assigned for MineCounterUI script in GameObject " + gameObject.name);
        UnityEngine.Assertions.Assert.IsNotNull(totalCount, "ERROR: Total Count (Text) not assigned for MineCounterUI script in GameObject " + gameObject.name);
    }
    #endregion

    #region Public Methods
    public void SetTotalCount(int count)
    {
        totalCount.text = count.ToString();
    }

    public void SetCurrentCount(int count)
    {
        currentCount.text = count.ToString();
    }

    public void SetPercentageToNextMine(float percentage)
    {
        sprite.fillAmount = percentage;   
    }
    #endregion
}
