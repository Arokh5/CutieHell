using UnityEngine;

public class TutorialEvents
{
    private delegate void TutorialEvent();

    #region Fields
    private TutorialEvent[] events;
    #endregion

    #region Public Methods
    public TutorialEvents()
    {
        events = new TutorialEvent[1];
        events[0] = TestEvent;
    }

    public void LaunchEvent(int eventIndex)
    {
        if (eventIndex >= 0 && eventIndex < events.Length)
            events[eventIndex]();
    }
    #endregion

    #region Private Methods
    private void TestEvent()
    {
        Debug.Log("Event triggered successfully!");
    }
    #endregion
}
