using UnityEngine;

public class TimeWarper : MonoBehaviour
{
    #region Fields
    public float timeScale = 1;
    private float previousTimeScale = 1;
    #endregion

    #region MonoBehaviour Methods
    private void OnEnable()
    {
        UpdateTimeScale();
    }

    private void Update()
    {
        if (timeScale != previousTimeScale)
        {
            UpdateTimeScale();
        }
    }

#if UNITY_EDITOR
    private void OnDestroy()
    {
        Time.timeScale = 1.0f;
    }
#endif

    #endregion

    #region Private Methods
    private void UpdateTimeScale()
    {
        previousTimeScale = timeScale;
        Time.timeScale = timeScale;
    }
    #endregion
}
