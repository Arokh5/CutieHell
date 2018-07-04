public interface IDamageable
{
    UnityEngine.Transform transform {get; }

    float GetMaxHealth();
    float GetCurrentHealth();
    // Called by Player (for AIEnemy) and by AIEnemy (for Building)
    bool IsDead();
    // Called by Player (for AIEnemy) and by AIEnemy (for Building)
    void TakeDamage(float damage, AttackType attacktype);
}