using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/PlaceMine")]
public class PlaceMine : StateAction
{
    [SerializeField]
    private TutorialEventLauncher tutorialEventLauncher;

    public override void Act(Player player)
    {
        if (InputManager.instance.GetXButtonDown())
        {
            if (player.GetAvailableMines() > 0)
            {
                tutorialEventLauncher.LaunchEvent();
                AnimatorClipInfo[] a = player.animator.GetCurrentAnimatorClipInfo(0);
                if (a[0].clip.name != "Attack_Mine" || !player.animator.GetBool("PlaceMine"))
                {
                    player.animator.SetTrigger("PlaceMine");
                }
            }
            else
                player.mineAttackCooldown.cooldownUI.Flash();
        }
    }
}
