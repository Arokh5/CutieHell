using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/CanonTurretAttack")]
public class CanonTurretAttack : StateAction
{

    [Header("CanonBall Fields")]
    public float canonBallshootingSpeed;
    GameObject canonBall;
    [SerializeField]
    ParticleSystem shootingVFX;

    [Header("Canon Decal Fields")]
    public float limitedShootDistance;
    public float decalMovementSpeed;
    [SerializeField]
    ParticleSystem canonBallLandingVFX;

    private Transform canonTargetDecalGOTransform = null;

    public override void Act(Player player)
    {
        if(canonTargetDecalGOTransform == null)
        {
            canonTargetDecalGOTransform = player.currentTrap.canonTargetDecal.transform;
        }
        MoveShootDecal(player);
    }
    

    #region Private Methods
    private void MoveShootDecal(Player player)
    {
        if (InputManager.instance.GetLeftStickUp())
        {
            canonTargetDecalGOTransform.localPosition += canonTargetDecalGOTransform.forward * Time.deltaTime * decalMovementSpeed;
            if (Vector3.Distance(canonTargetDecalGOTransform.position, player.currentTrap.transform.position) > limitedShootDistance)
            {
                canonTargetDecalGOTransform.localPosition -= canonTargetDecalGOTransform.forward * Time.deltaTime * decalMovementSpeed;
            }
        }
        if (InputManager.instance.GetLeftStickDown())
        {
            canonTargetDecalGOTransform.localPosition += -canonTargetDecalGOTransform.forward * Time.deltaTime * decalMovementSpeed;
            if (Vector3.Distance(canonTargetDecalGOTransform.position, player.currentTrap.transform.position) > limitedShootDistance)
            {
                canonTargetDecalGOTransform.localPosition += canonTargetDecalGOTransform.forward * Time.deltaTime * decalMovementSpeed;
            }
        }
        if (InputManager.instance.GetLeftStickLeft())
        {
            canonTargetDecalGOTransform.localPosition += -canonTargetDecalGOTransform.right * Time.deltaTime * decalMovementSpeed;
            if (Vector3.Distance(canonTargetDecalGOTransform.position, player.currentTrap.transform.position) > limitedShootDistance)
            {
                canonTargetDecalGOTransform.localPosition += canonTargetDecalGOTransform.right * Time.deltaTime * decalMovementSpeed;
            }
        }
        if (InputManager.instance.GetLeftStickRight())
        {
            canonTargetDecalGOTransform.localPosition += canonTargetDecalGOTransform.right * Time.deltaTime * decalMovementSpeed;
            if (Vector3.Distance(canonTargetDecalGOTransform.position, player.currentTrap.transform.position) > limitedShootDistance)
            {
                canonTargetDecalGOTransform.localPosition += -canonTargetDecalGOTransform.right * Time.deltaTime * decalMovementSpeed;
            }
        }
    }
    #endregion


}

