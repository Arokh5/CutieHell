using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/PlaceMeteorite")]
public class PlaceMeteorite : StateAction
{
    public ParticleSystem meteoritePrefab;
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
                ParticlesManager.instance.LaunchParticleSystem(meteoritePrefab, player.transform.position, meteoritePrefab.transform.rotation);
            }
            else
                player.meteoriteAttackCooldown.cooldownUI.Flash();
        }
    }
}
