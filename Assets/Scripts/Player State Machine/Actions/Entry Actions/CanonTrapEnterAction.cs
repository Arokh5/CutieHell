using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/CanonTrapEnterAction")]
public class CanonTrapEnterAction : StateAction
{
    public float startingCanonDecalDistance;
    [SerializeField]
    private ParticleSystem canonShootDecalPrefabVFX;

    public override void Act(Player player)
    {
        ParticleSystem canonShootDecalVFX = null;

        GameManager.instance.SetCrosshairActivate(false);
        player.bulletSpawnPoint.SetParent(player.currentTrap.transform);

        canonShootDecalVFX = player.currentTrap.canonTargetDecal;
        canonShootDecalVFX.transform.position = player.currentTrap.transform.position;
        canonShootDecalVFX.transform.localEulerAngles = player.currentTrap.rotatingHead.transform.localEulerAngles;
        canonShootDecalVFX.transform.Translate(Vector3.forward * startingCanonDecalDistance);
        canonShootDecalVFX.transform.Translate(Vector3.down * startingCanonDecalDistance);
        canonShootDecalVFX.transform.localPosition = new Vector3(canonShootDecalVFX.transform.localPosition.x, canonShootDecalPrefabVFX.transform.localPosition.y, canonShootDecalVFX.transform.localPosition.z);

        canonShootDecalVFX.gameObject.SetActive(true);
        canonShootDecalVFX.Play();

    }
}
