using UnityEngine;

public class RandomTimedTrigger : SoundTrigger
{
    #region Fields
    [Range(0, 100)]
    public int chanceToTrigger;
    public float testingInterval = 1.0f;
    public float triggeredDuration = 2.0f;

    private float pauseTimeLeft = 0;
    private float timeToNextTest = 0;
    private bool isOn;
    #endregion

    #region MonoBehaviour Methods
    private void Update()
    {
        if (pauseTimeLeft > 0)
            pauseTimeLeft -= Time.deltaTime;

        if (pauseTimeLeft <= 0)
        {
            if (isOn)
            {
                isOn = false;
                TriggerOff();
            }

            HandleRandomTriggering();
        }
    }
    #endregion

    #region Private Methods
    private void HandleRandomTriggering()
    {
        if (ShouldTrigger())
        {
            isOn = true;
            pauseTimeLeft = triggeredDuration;
            TriggerOn();
        }
    }

    private bool ShouldTrigger()
    {
        bool result = false;
        timeToNextTest -= Time.deltaTime;
        if (timeToNextTest < 0)
        {
            timeToNextTest += testingInterval;
            result = Random.Range(0, 100) < chanceToTrigger;
        }
        return result;
    }
    #endregion
}
