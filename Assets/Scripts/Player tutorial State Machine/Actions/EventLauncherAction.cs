using UnityEngine;

[CreateAssetMenu(menuName = "Player Tutorial State Machine/Actions/EventLauncherAction")]
public class EventLauncherAction : StateAction
{
    public int eventIndex = -1;

    public override void Act(Player player)
    {
        GameManager.instance.tutorialController.LaunchEvent(eventIndex);
    }
}
