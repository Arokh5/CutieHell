using UnityEngine;

public class IndicatorsController : MonoBehaviour
{
    #region Fields
    public GameObject[] monumentConqueredIcons = new GameObject[3];
    private MonumentIndicator[] monumentIndicators = null;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        monumentIndicators = GetComponentsInChildren<MonumentIndicator>();

        if (monumentIndicators.Length == 0)
            Debug.LogWarning("WARNING: No MonumentIndicator were found in children of IndicatorsController in gameObject '" + gameObject.name + "'!");
    }
    #endregion

    #region Public Methods
    public void OnNewRoundStarted()
    {
        foreach (MonumentIndicator monumentIndicator in monumentIndicators)
        {
            monumentIndicator.OnNewRoundStarted();
        }
    }

    public void MonumentConquered(int index)
    {
        if (index >= 0 && index < monumentConqueredIcons.Length)
            monumentConqueredIcons[index].SetActive(true);
    }
    #endregion
}
