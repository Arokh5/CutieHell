using UnityEngine;

[System.Serializable]
public class TutorialEventLauncher
{
    [SerializeField]
    private int eventIndex = -1;

    public void LaunchEvent()
    {
        if (eventIndex != -1)
        {
            GameManager.instance.LaunchTutorialEvent(eventIndex);
        }
    }
}
