using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/PlaceMeteorite")]
public class PlaceMeteorite : StateAction
{
    public ParticleSystem balckHoleAttack;
    [SerializeField]
    private TutorialEventLauncher tutorialEventLauncher;

    public override void Act(Player player)
    {
        if (InputManager.instance.GetTriangleButtonDown())
        {
            if (player.meteoriteAttackCooldown.timeSinceLastAction >= player.meteoriteAttackCooldown.cooldownTime)
            {
                player.animator.SetTrigger("BlackHole");
                tutorialEventLauncher.LaunchEvent();
                player.meteoriteAttackCooldown.timeSinceLastAction = 0.0f;

            }
            else
                player.meteoriteAttackCooldown.cooldownUI.Flash();
        }
    }
}
