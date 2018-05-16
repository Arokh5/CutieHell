using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct CanonBallInfo
{
    public float canonBallSpeed;
    public int canonBallExplosionDamage;
    public float canonBallExplosionRange;
    public float canonBallParabolaHeight;
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
    ParticleSystem shootingVFX;
    private Transform canonBallStartPoint;

    [Header("Canon Decal Fields")]
    public float limitedShootDistance;
    public float minimumShootDistance;
    public float decalMovementSpeed;
    [SerializeField]
    ParticleSystem canonBallLandingVFX;

    [Header("Canon Fields")]
    public float canonWayOutPoint;

    private static Transform canonTargetDecalGOTransform = null;

    private Player playerRef = null;

    public override void Act(Player player)
    {
        if (playerRef == null)
        {
            CanonAttackFirstUsePreparations(player);
        }

        if (InputManager.instance.GetR2ButtonDown())
        {
            if(player.currentTrap.canonBallsList.Count == 0 )
            {
                ShootCanonBall();
            }
        }

        MoveShootDecal();
    }

    #region Private Methods
    private void CanonAttackFirstUsePreparations(Player player)
    {
        playerRef = player;
        canonBallStartPoint = playerRef.currentTrap.canonBallStartPoint;
        canonTargetDecalGOTransform = player.currentTrap.canonTargetDecal;

        UnityEngine.Assertions.Assert.IsFalse(canonBallExplosionRange > playerRef.currentTrap.attractionRadius, "ERROR: CanonBall explosion range can't be greater than its owner trap attraction radius");

        CanonBallInfo canonBallInfo;
        canonBallInfo.canonBallExplosionDamage = canonBallExplosionDamage;
        canonBallInfo.canonBallExplosionRange = canonBallExplosionRange;
        canonBallInfo.canonBallParabolaHeight = maxParabolaHeight;
        canonBallInfo.canonBallSpeed = canonBallshootingSpeed;

        playerRef.currentTrap.SetCanonBallInfo(canonBallInfo);
    }

    private void checkMoveBackShootDecal(float decalNewDistance, Vector3 movementMagnitude)
    {
        if(decalNewDistance > limitedShootDistance || (decalNewDistance < minimumShootDistance))
        { 
      
            canonTargetDecalGOTransform.localPosition = canonTargetDecalGOTransform.localPosition - movementMagnitude * Time.deltaTime * decalMovementSpeed;
        }
    }

    private void MoveShootDecal()
    {
        if (InputManager.instance.GetRightStickUp())
        {
            canonTargetDecalGOTransform.localPosition += canonTargetDecalGOTransform.forward * Time.deltaTime * decalMovementSpeed;

            float distanceAfterMovement = Vector3.Distance(canonTargetDecalGOTransform.position, playerRef.currentTrap.transform.position);
            checkMoveBackShootDecal(distanceAfterMovement, canonTargetDecalGOTransform.forward);           
        }
        if (InputManager.instance.GetRightStickDown())
        {
            canonTargetDecalGOTransform.localPosition += -canonTargetDecalGOTransform.forward * Time.deltaTime * decalMovementSpeed;

            float distanceAfterMovement = Vector3.Distance(canonTargetDecalGOTransform.position, playerRef.currentTrap.transform.position);
            checkMoveBackShootDecal(distanceAfterMovement, -canonTargetDecalGOTransform.forward);
        }
        if (InputManager.instance.GetRightStickRight())
        {
            canonTargetDecalGOTransform.localPosition += canonTargetDecalGOTransform.right * Time.deltaTime * decalMovementSpeed;

            float distanceAfterMovement = Vector3.Distance(canonTargetDecalGOTransform.position, playerRef.currentTrap.transform.position);
            checkMoveBackShootDecal(distanceAfterMovement, canonTargetDecalGOTransform.right);
        }
        if (InputManager.instance.GetRightStickLeft())
        {
            canonTargetDecalGOTransform.localPosition += -canonTargetDecalGOTransform.right * Time.deltaTime * decalMovementSpeed;

            float distanceAfterMovement = Vector3.Distance(canonTargetDecalGOTransform.position, playerRef.currentTrap.transform.position);
            checkMoveBackShootDecal(distanceAfterMovement, -canonTargetDecalGOTransform.right);
        }
    }

    private void ShootCanonBall()
    {
        Vector3 shootDistance = canonBallStartPoint.position - canonTargetDecalGOTransform.position;
        float shootingDuration = shootDistance.magnitude/canonBallshootingSpeed;

        GameObject canonBall = Instantiate(canonBallPrefab, canonBallStartPoint);
        CanonBallMotion canonBallMotion = canonBall.GetComponent<CanonBallMotion>();
        canonBall.AddComponent<Rigidbody>();

        canonBall.SetActive(false);
        canonBallMotion.canonBall = canonBall;
        canonBallMotion.canonBallElapsedTime = 0;
        canonBallMotion.canonBallShootingDuration = shootingDuration;
        canonBallMotion.canonBallShotingDistance = shootDistance;
        canonBallMotion.canonBallVisibleFromProgression = canonWayOutPoint;

        playerRef.currentTrap.canonBallsList.Add(canonBallMotion);

    }

    private void DamageEnemiesAffectedByCanonBallExplosion(Transform enemyProjectionTransform)
    {
        //Worth to do it only on enemies attracted by current trap?
        List<AIEnemy> affectedEnemies = playerRef.currentTrap.ObtainEnemiesAffectedByTrapRangedDamage(enemyProjectionTransform, canonBallExplosionRange);
      
        for( int i = 0; i < affectedEnemies.Count; i++)
        {
            affectedEnemies[i].TakeDamage(canonBallExplosionDamage, AttackType.TRAP_AREA);
        }

    }
    #endregion

}