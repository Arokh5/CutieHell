using UnityEngine;

public class AIEnemy_temp : MonoBehaviour, IDamageable
{
    #region Fields

    private int health = 3;

    #endregion

    #region Properties

    #endregion

    #region MonoBehaviour Methods

    #endregion

    #region Public Methods

    // Called by Player (for AIEnemy) and by AIEnemy (for Building)
    public bool IsDead()
    {
        return true;
    }

    // Called by the AIPlayer or an Attack to damage the AIEnemy
    public void TakeDamage(int dmg, AttackType attacktype)
    {
        health -= dmg;

        if (health <= 0)
            Destroy(gameObject);
    }

    #endregion

    #region Private Methods

    #endregion
}