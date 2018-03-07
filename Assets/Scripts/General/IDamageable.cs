public interface IDamageable
{
    // Called by Player (for AIEnemy) and by AIEnemy (for Building)
    bool IsDead();
    // Called by Player (for AIEnemy) and by AIEnemy (for Building)
    bool TakeDamage(int dmg, AttackType attacktype);
}