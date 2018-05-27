using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageLimiter : MonoBehaviour
{
    #region Fields
    [SerializeField]
    private Building building;
    public float normalizedMaxDamage = 1;
    #endregion

    #region MonoBehaviour Methods
    private void Update()
    {
        if (building)
        {
            float maxHealth = building.GetMaxHealth();
            float normalizedDamage = (maxHealth - building.GetCurrentHealth()) / maxHealth;
            if (normalizedDamage > normalizedMaxDamage)
            {
                building.SetHealth(maxHealth * (1 - normalizedMaxDamage));
            }
        }
    }
    #endregion
}
