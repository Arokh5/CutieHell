using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Decisions/FogEnterDecision")]
public class FogEnterDecision : Decision
{
    [SerializeField]
    private float minEvilToActivateFog;

    public override bool Decide(Player player)
    {
        return Time.time - player.fogStateCooldown > player.fogStateLastTime
           && player.GetEvilLevel() > minEvilToActivateFog
           && InputManager.instance.GetSquareButtonDown();
    }
}
