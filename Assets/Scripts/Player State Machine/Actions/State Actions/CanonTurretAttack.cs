using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct CanonBallInfo
{
    public float canonBallSpeed;
    public int canonBallExplosionDamage;
    public float canonBallExplosionRange;
    public float canonBallParabolaHeight;
    public GameObject canonBallExplosionVFX;
}


[CreateAssetMenu(menuName = "Player State Machine/Actions/CanonTurretAttack")]
public class CanonTurretAttack : StateAction
{
    [Header("CanonBall Fields")]
    public float canonBallshootingSpeed;
    public float canonBallExplosionRange;
    public int canonBallExplosionDamage;
    public float canonBallCooldownTime;
    [Range(0.0f,1.0f)]
    public float maxParabolaHeight;
    public GameObject canonBallPrefab;
    [SerializeField]
    GameObject canonBallLandingVFX;
    private Transform canonBallStartPoint;

    private GameObject currentCanonBall = null;
    private CanonBallMotion currentCanonBallMotion = null;
    private CanonInfo canon = null;

    private Player playerRef = null;

    public override void Act(Player player)
    {
        if (playerRef == null)
            playerRef = player;

        if (currentCanonBall == null)
            CanonAttackFirstUsePreparations(player);

        if (currentCanonBallMotion.canonBallElapsedTime >= canonBallCooldownTime)            
        {
            playerRef.currentTrap.GetCanonAmmoIndicator().gameObject.SetActive(true);
            if (InputManager.instance.GetR2ButtonDown())
                {
                    ShootCanonBall();
                }      
        }
        currentCanonBallMotion.canonBallElapsedTime += Time.deltaTime;
        MoveShootDecal();
    }

    #region Private Methods
    private void CanonAttackFirstUsePreparations(Player player)
    {
        canon = playerRef.currentTrap.rotatingHead.GetComponent<CanonInfo>();
        canonBallStartPoint = playerRef.currentTrap.canonBallStartPoint;
        canon.canonTargetDecalGOTransform = player.currentTrap.canonTargetDecal.transform;

        UnityEngine.Assertions.Assert.IsFalse(canonBallExplosionRange > playerRef.currentTrap.attractionRadius, "ERROR: CanonBall explosion range can't be greater than its owner trap attraction radius");

        PrepareCanonBall();
    }

    private void PrepareCanonBall()
    {
        CanonBallInfo canonBallInfo;

        canonBallInfo.canonBallExplosionDamage = canonBallExplosionDamage;
        canonBallInfo.canonBallExplosionRange = canonBallExplosionRange;
        canonBallInfo.canonBallParabolaHeight = maxParabolaHeight;
        canonBallInfo.canonBallSpeed = canonBallshootingSpeed;
        canonBallInfo.canonBallExplosionVFX = canonBallLandingVFX;

        playerRef.currentTrap.SetCanonBallInfo(canonBallInfo);

        currentCanonBall = Instantiate(canonBallPrefab, canonBallStartPoint);
        currentCanonBallMotion = currentCanonBall.GetComponent<CanonBallMotion>();
    }

    private void checkMoveBackShootDecal(float decalNewDistance, Vector3 movementMagnitude)
    {
        if(decalNewDistance > canon.limitedShootDistance || (decalNewDistance < canon.minimumShootDistance))
        { 
      
            canon.canonTargetDecalGOTransform.localPosition = canon.canonTargetDecalGOTransform.localPosition - movementMagnitude * Time.deltaTime * canon.decalMovementSpeed;
        }
    }

    private void MoveShootDecal()
    {
        if (InputManager.instance.GetRightStickUp())
        {
            canon.canonTargetDecalGOTransform.localPosition += canon.canonTargetDecalGOTransform.forward * Time.deltaTime * canon.decalMovementSpeed;

            float distanceAfterMovement = Vector3.Distance(canon.canonTargetDecalGOTransform.position, playerRef.currentTrap.transform.position);
            checkMoveBackShootDecal(distanceAfterMovement, canon.canonTargetDecalGOTransform.forward);           
        }
        if (InputManager.instance.GetRightStickDown())
        {
            canon.canonTargetDecalGOTransform.localPosition += -canon.canonTargetDecalGOTransform.forward * Time.deltaTime * canon.decalMovementSpeed;

            float distanceAfterMovement = Vector3.Distance(canon.canonTargetDecalGOTransform.position, playerRef.currentTrap.transform.position);
            checkMoveBackShootDecal(distanceAfterMovement, -canon.canonTargetDecalGOTransform.forward);
        }
        if (InputManager.instance.GetRightStickRight())
        {
            canon.canonTargetDecalGOTransform.localPosition += canon.canonTargetDecalGOTransform.right * Time.deltaTime * canon.decalMovementSpeed;

            float distanceAfterMovement = Vector3.Distance(canon.canonTargetDecalGOTransform.position, playerRef.currentTrap.transform.position);
            checkMoveBackShootDecal(distanceAfterMovement, canon.canonTargetDecalGOTransform.right);
        }
        if (InputManager.instance.GetRightStickLeft())
        {
            canon.canonTargetDecalGOTransform.localPosition += -canon.canonTargetDecalGOTransform.right * Time.deltaTime * canon.decalMovementSpeed;

            float distanceAfterMovement = Vector3.Distance(canon.canonTargetDecalGOTransform.position, playerRef.currentTrap.transform.position);
            checkMoveBackShootDecal(distanceAfterMovement, -canon.canonTargetDecalGOTransform.right);
        }
    }

    private void ShootCanonBall()
    {
        Vector3 shootDistance = canonBallStartPoint.position - canon.canonTargetDecalGOTransform.position;
        float shootingDuration = shootDistance.magnitude / canonBallshootingSpeed;

        currentCanonBallMotion.canonBallShootingDuration = shootingDuration;
        currentCanonBallMotion.canonBallShotingDistance = shootDistance;
        currentCanonBallMotion.canonBallVisibleFromProgression = canon.canonWayOutPoint;

        currentCanonBallMotion.SetAlreadyFired(true);

        playerRef.currentTrap.canonBallsList.Add(currentCanonBallMotion);
        playerRef.currentTrap.GetCanonAmmoIndicator().gameObject.SetActive(false);

        //We prepare the following canonBall now the previous has already been fired
        PrepareCanonBall();
    }
    #endregion

}