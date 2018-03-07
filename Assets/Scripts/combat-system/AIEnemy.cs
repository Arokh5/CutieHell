using UnityEngine;
using Assets.Scripts;

public class AIEnemy : MonoBehaviour, IDamageable
{
    #region Fields

    private int health = 3;

    #endregion

    #region Properties

    #endregion

    #region MonoBehaviour Methods

    #endregion

    #region Public Methods

    public bool IsDead()
    {
        return health <= 0;
    }

    // Called by the AIPlayer or an Attack to damage the AIEnemy
    public bool TakeDamage(float dmg, AttackType attacktype)
    {
        switch (attacktype)
        {
            case AttackType.WEAK:
                health -= 1;
                break;

            case AttackType.STRONG:
                health -= 3;
                break;
        }

        if (IsDead())
            Destroy(gameObject);

        return true;
    }

    #endregion

    #region Private Methods

    #endregion
}