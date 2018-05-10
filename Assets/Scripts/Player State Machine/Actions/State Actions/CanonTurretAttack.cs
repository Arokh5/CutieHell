using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public float decalMovementSpeed;
    [SerializeField]
    ParticleSystem canonBallLandingVFX;
    private Transform canonTargetDecalGOTransform = null;

    private Player playerRef;

    public override void Act(Player player)
    {
        canonTargetDecalGOTransform = player.currentTrap.canonTargetDecal; // change this two lines of code to assign it just once
        playerRef = player;
        if (InputManager.instance.GetR2ButtonDown())
        {
            if(player.currentTrap.canonBallsList.Count == 0 )
            {
                canonBallStartPoint = playerRef.currentTrap.canonBallStartPoint;
                ShootCanonBall();
                UnityEngine.Assertions.Assert.IsFalse(canonBallExplosionRange > playerRef.currentTrap.attractionRadius, "ERROR: CanonBall explosion range can't be greater than its owner trap attraction radius");
            }
        }

        if(playerRef.currentTrap.canonBallsList.Count > 0) 
        {
            UpdateCanonBallsMotion();
        }

        MoveShootDecal();
    }

    #region Private Methods
    private void MoveShootDecal()
    {
        if (InputManager.instance.GetLeftStickUp())
        {
            canonTargetDecalGOTransform.localPosition += canonTargetDecalGOTransform.forward * Time.deltaTime * decalMovementSpeed;
            if (Vector3.Distance(canonTargetDecalGOTransform.position, playerRef.currentTrap.transform.position) > limitedShootDistance)
            {
                canonTargetDecalGOTransform.localPosition -= canonTargetDecalGOTransform.forward * Time.deltaTime * decalMovementSpeed;
            }
        }
        if (InputManager.instance.GetLeftStickDown())
        {
            canonTargetDecalGOTransform.localPosition += -canonTargetDecalGOTransform.forward * Time.deltaTime * decalMovementSpeed;
            if (Vector3.Distance(canonTargetDecalGOTransform.position, playerRef.currentTrap.transform.position) > limitedShootDistance)
            {
                canonTargetDecalGOTransform.localPosition += canonTargetDecalGOTransform.forward * Time.deltaTime * decalMovementSpeed;
            }
        }
        if (InputManager.instance.GetLeftStickLeft())
        {
            canonTargetDecalGOTransform.localPosition += -canonTargetDecalGOTransform.right * Time.deltaTime * decalMovementSpeed;
            if (Vector3.Distance(canonTargetDecalGOTransform.position, playerRef.currentTrap.transform.position) > limitedShootDistance)
            {
                canonTargetDecalGOTransform.localPosition += canonTargetDecalGOTransform.right * Time.deltaTime * decalMovementSpeed;
            }
        }
        if (InputManager.instance.GetLeftStickRight())
        {
            canonTargetDecalGOTransform.localPosition += canonTargetDecalGOTransform.right * Time.deltaTime * decalMovementSpeed;
            if (Vector3.Distance(canonTargetDecalGOTransform.position, playerRef.currentTrap.transform.position) > limitedShootDistance)
            {
                canonTargetDecalGOTransform.localPosition += -canonTargetDecalGOTransform.right * Time.deltaTime * decalMovementSpeed;
            }
        }
    }

    private void ShootCanonBall()
    {
        Vector3 shootDistance = canonBallStartPoint.position - canonTargetDecalGOTransform.position;
        float shootingDuration = shootDistance.magnitude/canonBallshootingSpeed;

        GameObject canonBall = Instantiate(canonBallPrefab, canonBallStartPoint);
        CanonBallMotion canonBallMotion = canonBall.GetComponent<CanonBallMotion>();
        canonBall.SetActive(false);
        canonBallMotion.canonBall = canonBall;
        canonBallMotion.canonBallElapsedTime = 0;
        canonBallMotion.canonBallShootingDuration = shootingDuration;
        canonBallMotion.canonBallShotingDistance = shootDistance;

        playerRef.currentTrap.canonBallsList.Add(canonBallMotion);

    }

    private void UpdateCanonBallsMotion()
    {
        float motionProgress = 0;
        CanonBallMotion evaluatedCanonBall;
        Vector3 nextPosition;

        for(int i = 0; i < playerRef.currentTrap.canonBallsList.Count; i++)
        {
            evaluatedCanonBall = playerRef.currentTrap.canonBallsList[i];
            evaluatedCanonBall.canonBallElapsedTime += Time.deltaTime;
            motionProgress = evaluatedCanonBall.canonBallElapsedTime / evaluatedCanonBall.canonBallShootingDuration;

            if( !evaluatedCanonBall.canonBall.gameObject.activeSelf && motionProgress >= 0.15 )
            {
                evaluatedCanonBall.canonBall.gameObject.SetActive(true);
            }

            nextPosition = canonBallStartPoint.position - evaluatedCanonBall.canonBallShotingDistance * motionProgress;

            if (motionProgress <= 0.5f) //Ascendent Halfway
            { 
                nextPosition.y += evaluatedCanonBall.canonBallShotingDistance.magnitude * ( motionProgress * maxParabolaHeight);
            }
            else // Descendent Halfway
            {
                nextPosition.y += evaluatedCanonBall.canonBallShotingDistance.magnitude * (( 1 - motionProgress) * maxParabolaHeight);
            }

            evaluatedCanonBall.transform.position = nextPosition;

            if (motionProgress >= 1)
            {
                playerRef.currentTrap.canonBallsList.Remove(evaluatedCanonBall);
                DamageEnemiesAffectedByCanonBallExplosion(evaluatedCanonBall.transform);
                Destroy(evaluatedCanonBall.canonBall.gameObject);
            }
            //Add anticipated explosion in case there's a collision with an enemy before landing
        }
       
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