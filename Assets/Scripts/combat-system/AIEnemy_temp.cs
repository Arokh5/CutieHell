using UnityEngine;

public class AIEnemy_temp : MonoBehaviour, IDamageable
{
    #region Fields

    public int evilnessReward = 1;
    private float health = 3;

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
    public void TakeDamage(float dmg, AttackType attacktype)
    {
        health -= dmg;

        if (health <= 0)
        {
            Player.instance.SetEvilLevel(evilnessReward);
            Destroy(gameObject);
        }
    }

    #endregion

    #region Private Methods

    #endregion
}