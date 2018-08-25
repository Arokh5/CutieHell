using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/LaunchTutorialEventAction")]
public class LaunchTutorialEventAction : StateAction
{
    [SerializeField]
    private TutorialEventLauncher tutorialEventLauncher;

    public override void Act(Player player)
    {
        tutorialEventLauncher.LaunchEvent();
    }
}
