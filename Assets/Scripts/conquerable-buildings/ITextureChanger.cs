using UnityEngine;

public interface ITextureChanger
{
    Transform transform { get; }
    float GetNormalizedBlendStartRadius();
    float GetEffectMaxRadius();
}
