using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/CanonTrapEnterAction")]
public class CanonTrapEnterAction : StateAction
{
    public float startingCanonDecalDistance;
    [SerializeField]
    private GameObject canonShootDecalPrefabVFX;
    private CanonInfo canon;
    private GameObject canonShootDecalVFX = null;

    private Player playerRef = null;

    public override void Act(Player player)
    {
        if (playerRef == null)
        {
            playerRef = player;
            canon = playerRef.currentTrap.rotatingHead.GetComponent<CanonInfo>();
        }
        
        GameManager.instance.SetCrosshairActivate(false);
        player.currentTrap.GetCanonAmmunitionImageExchanger().gameObject.SetActive(true);
        player.bulletSpawnPoint.SetParent(player.currentTrap.transform);

        SetUpCanonTargetDecalInitialPosition();
        canonShootDecalVFX.gameObject.SetActive(true);
    }

    private void SetUpCanonTargetDecalInitialPosition()
    {
        canonShootDecalVFX = playerRef.currentTrap.canonTargetDecal;
        canonShootDecalVFX.transform.position = playerRef.currentTrap.transform.position;
        canonShootDecalVFX.transform.localEulerAngles = playerRef.currentTrap.rotatingHead.transform.localEulerAngles;

        canonShootDecalVFX.transform.Translate(Vector3.forward * startingCanonDecalDistance);

        CheckCanonTargetDecalInitialPosition();

        canonShootDecalVFX.transform.localPosition = new Vector3(canonShootDecalVFX.transform.localPosition.x, canonShootDecalPrefabVFX.transform.localPosition.y, canonShootDecalVFX.transform.localPosition.z);
    }

    private void CheckCanonTargetDecalInitialPosition()
    {
        Vector3 offset = canonShootDecalVFX.transform.position - playerRef.currentTrap.transform.position;
        float sqrPositionsOffset = offset.sqrMagnitude;

        if(sqrPositionsOffset < canon.minimumShootDistance * canon.minimumShootDistance)
        {
            float distanceToMinimumInitialPosition = (canon.minimumShootDistance * canon.minimumShootDistance) - sqrPositionsOffset;
            Vector3 canonShootDecalPosition = canonShootDecalVFX.transform.position;
            canonShootDecalPosition.z -= distanceToMinimumInitialPosition + 0.1f; //little offset to not be exactly at minimum distance position
            canonShootDecalVFX.transform.position = canonShootDecalPosition;
        }
    }

    
}
