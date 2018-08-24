using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/LaunchTutorialEventAction")]
public class LaunchTutorialEventAction : StateAction
{
    [SerializeField]
    private TutorialEventLauncher eventLauncher;

    public override void Act(Player player)
    {
        eventLauncher.LaunchEvent();
    }
}
