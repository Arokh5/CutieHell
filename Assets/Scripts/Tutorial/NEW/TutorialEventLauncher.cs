using UnityEngine;

[System.Serializable]
public class TutorialEventLauncher
{
    [SerializeField]
    private int eventIndex;

    public void LaunchEvent()
    {
        GameManager.instance.LaunchTutorialEvent(eventIndex);
    }
}
