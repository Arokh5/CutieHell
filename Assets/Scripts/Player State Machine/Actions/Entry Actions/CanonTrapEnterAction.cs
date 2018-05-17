using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/CanonTrapEnterAction")]
public class CanonTrapEnterAction : StateAction
{
    public float startingCanonDecalDistance;
    [SerializeField]
    public ParticleSystem canonShootDecalPrefabVFX;
    private ParticleSystem canonShootDecalVFX = null;
    private Transform canonTargetDecalGOTransform = null;

    public override void Act(Player player)
    {
        GameManager.instance.SetCrosshairActivate(false);
        player.bulletSpawnPoint.SetParent(player.currentTrap.transform);

        canonTargetDecalGOTransform = player.currentTrap.canonTargetDecal.transform;
        canonTargetDecalGOTransform.gameObject.SetActive(true);
        canonShootDecalVFX = Object.Instantiate(canonShootDecalPrefabVFX, canonTargetDecalGOTransform);
        canonTargetDecalGOTransform.position += player.currentTrap.rotatingHead.transform.forward * startingCanonDecalDistance;
        canonTargetDecalGOTransform.position = new Vector3(canonTargetDecalGOTransform.position.x, 3.20f, canonTargetDecalGOTransform.position.z);
        canonShootDecalVFX.Play();

        player.currentTrap.GetCanonAmmoIndicator().gameObject.SetActive(true);

    }
}
