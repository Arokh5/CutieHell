using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/CuteAreaDamage")]
public class CuteAreaDamage : StateAction
{
    public float cuteAreaDps = 10.0f;
    public float infoPanelBlendDuration = 0.5f;

    private TextureChangerSource textureChangerSource;

    public override void Act(Player player)
    {
        if (CheckTextureChangerSource())
        {
            bool inCuteArea = true;
            foreach (ITextureChanger textureChanger in textureChangerSource.textureChangers)
            {
                Vector3 playerToChanger = textureChanger.transform.position - player.transform.position;
                float safeRadius = textureChanger.GetEffectMaxRadius();
                if (playerToChanger.sqrMagnitude < safeRadius * safeRadius)
                {
                    inCuteArea = false;
                    break;
                }
            }
            if (inCuteArea)
            {
                player.TakeDamage(cuteAreaDps * Time.deltaTime, AttackType.CUTE_AREA);
                player.cuteGroundsInfoPanel.ShowAnimated(infoPanelBlendDuration);
            }
            else
            {
                player.cuteGroundsInfoPanel.HideAnimated(infoPanelBlendDuration);
            }
        }
    }

    private bool CheckTextureChangerSource()
    {
        if (!textureChangerSource)
            textureChangerSource = FindObjectOfType<TextureChangerSource>();

        return textureChangerSource != null;
    }
}
