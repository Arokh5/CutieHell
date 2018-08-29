using UnityEngine;

public class LaunchParticles : ScriptedAnimation
{
    #region
    [SerializeField]
    [Tooltip("The array of ParticleSystems to be launched")]
    private new ParticleSystem particleSystem;
    [SerializeField]
    [Tooltip("Reference used to determine the position where the Particle System will be launched. If none is provided, the Fallback Position is used.")]
    private Transform referenceTransform;
    [SerializeField]
    [Tooltip("This position is only used if no Reference Transform is provided")]
    private Vector3 fallbackPosition;
    #endregion

    #region Protected Methods
    protected override void StartAnimationInternal()
    {
        if (particleSystem != null)
        {
            Vector3 position = referenceTransform ? referenceTransform.position : fallbackPosition;
            ParticlesManager.instance.LaunchParticleSystem(particleSystem, position, particleSystem.transform.rotation);
        }
        
        OnAnimationFinished();
    }
    #endregion
}
