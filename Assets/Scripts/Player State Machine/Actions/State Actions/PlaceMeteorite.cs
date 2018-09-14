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
                tutorialEventLauncher.LaunchEvent();
                player.meteoriteAttackCooldown.timeSinceLastAction = 0.0f;
                ParticlesManager.instance.LaunchParticleSystem(balckHoleAttack, player.transform.position + player.transform.forward * 4 +  Vector3.up * 3, balckHoleAttack.transform.rotation);
            }
            else
                player.meteoriteAttackCooldown.cooldownUI.Flash();
        }
    }
}
