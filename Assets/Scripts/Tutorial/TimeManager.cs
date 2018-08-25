using UnityEngine;

public class TimeManager : MonoBehaviour
{
    #region Fields
    public static TimeManager instance;

    [SerializeField]
    [Range(0.0f, 100.0f)]
    private float defaultTimeScale = 1.0f;

    private float previousTimeScale;
    private float currentTimeScale;
    private int pauseRequests = 0;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);

        currentTimeScale = defaultTimeScale;
        previousTimeScale = currentTimeScale;
        Time.timeScale = currentTimeScale;
    }

    private void OnValidate()
    {
        if (defaultTimeScale < 0.0f)
            defaultTimeScale = 0.0f;
        else if (defaultTimeScale > 100.0f)
            defaultTimeScale = 100.0f;
    }
    #endregion

    #region Public Methods
    public float GetDefaultTimeScale()
    {
        return defaultTimeScale;
    }

    public float GetTimeScale()
    {
        return currentTimeScale;
    }

    public void SetTimeScale(float timeScale)
    {
        if (timeScale == 0.0f)
            Debug.LogWarning("WARNING: TimeManager::SetTimeScale called with an argument of 0.0f. If your intent is to freeze time, you should use TimeManager::FreezeTime()");
        else if (timeScale == 1.0f)
            Debug.LogWarning("WARNING: TimeManager::SetTimeScale called with an argument of 1.0f. If your intent is to resume time, you should use TimeManager::ResumeTime()");
        if (timeScale < 0.0f || timeScale > 100.0f)
        {
            Debug.LogWarning("WARNING: TimeManager::SetTimeScale called with an argument (" + timeScale + ") out of the range [0.0f, 100.0f]. This range is a Unity-imposed limit!");
            timeScale = Mathf.Clamp(timeScale, 0.0f, 100.0f);
        }

        currentTimeScale = timeScale;
        UpdateTimeScale();
    }

    public void RestoreTimeScale()
    {
        currentTimeScale = defaultTimeScale;
        UpdateTimeScale();
    }

    public void FreezeTime()
    {
        ++pauseRequests;
        UpdateTimeScale();
    }

    public void ResumeTime()
    {
        --pauseRequests;
        if (pauseRequests < 0)
        {
            pauseRequests = 0;
            Debug.LogWarning("WARNING: TimeManager::ResumeTime called when there was no active pause request!");
        }
        UpdateTimeScale();
    }

    public bool IsTimeFrozen()
    {
        return pauseRequests > 0;
    }
    #endregion

    #region Private Methods
    private void UpdateTimeScale()
    {
        if (Time.timeScale != previousTimeScale)
            Debug.LogWarning("WARNING: Time.timeScale has been modified without using the TimeManager. You should refrain from directly setting the time scale since this can cause unexpected behaviour!");

        if (pauseRequests > 0)
        {
            Time.timeScale = 0.0f;
            previousTimeScale = 0.0f;
        }
        else
        {
            Time.timeScale = currentTimeScale;
            previousTimeScale = currentTimeScale;
        }
    }
    #endregion
}
