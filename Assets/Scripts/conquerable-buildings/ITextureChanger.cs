using UnityEngine;

public interface ITextureChanger
{
    Transform transform { get; }
    float GetEffectStartBlendRadius();
    float GetEffectMaxRadius();
}
