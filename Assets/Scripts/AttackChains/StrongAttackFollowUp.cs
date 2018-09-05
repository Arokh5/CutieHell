using UnityEngine;
using EZCameraShake;

[CreateAssetMenu(menuName = "Player State Machine/Actions/StrongAttackFollowUp")]
public class StrongAttackFollowUp : StateAction
{
    public int damage;
    public ParticleSystem strongAttackVFX;
    public float timeToGoIn, delay;

    [SerializeField]
    private AudioClip landingSfx;

    public override void Act(Player player)
    {
        switch (player.teleportState)
        {
            case Player.JumpStates.LAND:
                {
                    ParticlesManager.instance.LaunchParticleSystem(strongAttackVFX, player.transform.position, strongAttackVFX.transform.rotation);
                    BulletTime.instance.DoSlowmotion(0.01f, 0.35f);
                    CameraShaker.Instance.ShakeOnce(0.8f, 15.5f, 0.1f, 0.7f);
                    player.cameraState = Player.CameraState.CONEATTACK;
                    player.mainCameraController.y = 10.0f;
                    player.strongAttackTimer = 0.0f;
                    player.teleported = true;
                    player.teleportState = Player.JumpStates.DELAY;
                    player.strongAttackCooldown.timeSinceLastAction = 0.0f;
                    SoundManager.instance.PlaySfxClip(landingSfx);
                }
                break;
            case Player.JumpStates.DELAY:
                if (player.strongAttackTimer >= delay)
                {
                    player.comeBackFromStrongAttack = true;
                }
                break;
            default:
                break;
        }
    }
}
