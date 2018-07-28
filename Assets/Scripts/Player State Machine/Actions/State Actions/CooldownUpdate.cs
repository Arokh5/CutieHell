using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/CooldownUpdate")]
public class CooldownUpdate : StateAction
{
    public override void Act(Player player)
    {
        for (int i = 0; i < player.cooldownInfos.Length; ++i)
        {
            Player.CooldownInfo cooldownInfo = player.cooldownInfos[i];
            if (cooldownInfo.timeSinceLastAction <= cooldownInfo.cooldownTime)
            {
                cooldownInfo.timeSinceLastAction += Time.deltaTime;

                if (cooldownInfo.cooldownUI)
                {
                    float cooldownLeft = cooldownInfo.cooldownTime - cooldownInfo.timeSinceLastAction;

                    cooldownInfo.cooldownUI.SetCountdownLeft(cooldownLeft, cooldownInfo.cooldownTime);
                }
            }
        }
    }
}
