using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/ChangeCameraState")]
public class ChangeCameraState : StateAction {

    public Player.CameraState targetCameraState;

    public override void Act(Player player)
    {
        player.cameraState = targetCameraState;
    }
}
