using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonBallDestruction : MonoBehaviour
{

    #region Attributes
    private Player player = null;
    private Trap firingTrap = null;
    private CanonBallMotion canonBallMotion = null;
    #endregion

    #region MonoBehaviour Methods
    private void Start()
    {
        canonBallMotion = gameObject.GetComponent<CanonBallMotion>();
        player = GameManager.instance.GetPlayer1();
        firingTrap = player.currentTrap;
    }
    private void Update()
    {
        if (player != null && player.cameraState != Player.CameraState.CANONTURRET && !canonBallMotion.GetAlreadyFired())
        {
            firingTrap.canonBallsList.Remove(canonBallMotion);
            Destroy(gameObject);
        }
    }
    #endregion
}
