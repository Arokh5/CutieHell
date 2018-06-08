using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/StrongAttack")]
public class StrongAttack : StateAction
{
    public int evilCost;
    public int damage;
    public ParticleSystem strongAttackVFX;
    public float timeToGoOut, timeToGoIn;

    [SerializeField]
    private AudioClip attackSfx;

    public override void Act(Player player)
    {
        switch (player.teleportState)
        {
            case Player.TeleportStates.OUT:
                if (player.timeSinceLastStrongAttack >= timeToGoOut)
                {
                    player.canMove = true;
                    player.teleportState = Player.TeleportStates.TRAVEL;

                }
                break;
            case Player.TeleportStates.TRAVEL:
                if (InputManager.instance.GetOButtonDown())
                {
                    player.teleportState = Player.TeleportStates.IN;
                    player.timeSinceLastStrongAttack = 0.0f;
                    player.cameraState = Player.CameraState.MOVE;
                    player.mainCameraController.y = 10.0f;
                    ParticlesManager.instance.LaunchParticleSystem(strongAttackVFX, player.transform.position, strongAttackVFX.transform.rotation);
                    player.canMove = false;
                }
                break;
            case Player.TeleportStates.IN:
                if (player.timeSinceLastStrongAttack >= timeToGoIn)
                {
                    player.comeBackFromStrongAttack = true;
                    player.timeSinceLastTeleport = 0.0f;
                    player.teleported = true;
                }

                break;
            default:
                break;
        }
    }
}
