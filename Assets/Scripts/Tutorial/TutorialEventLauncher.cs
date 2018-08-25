using UnityEngine;

[System.Serializable]
public class TutorialEventLauncher
{
    [SerializeField]
    private int eventIndex = -1;

    public void LaunchEvent()
    {
        GameManager.instance.LaunchTutorialEvent(eventIndex);
    }
}
