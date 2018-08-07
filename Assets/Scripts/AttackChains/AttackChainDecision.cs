using System.Collections.Generic;

public class AttackChainDecision : Decision
{
    private List<ControllerButton> chainButtons = null;
    public override bool Decide(Player player)
    {
        if (chainButtons == null)
        {
            chainButtons = new List<ControllerButton>();
            AttackInfosManager.instance.GetAllInfosButtons(chainButtons);
        }

        foreach (ControllerButton button in chainButtons)
        {
            if (InputManager.instance.GetButtonDown(button))
            {
                AttackInfo info = AttackInfosManager.instance.GetAttackInfo(button);
                return AttackChainsManager.instance.ReportAttackAttempt(info.type);
            }
        }
        return false;
    }
}
