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
    private void OnDisable()
    {
        if (building)
            building.immortal = false;
    }

    private void Update()
    {
        if (building)
        {
            float maxHealth = building.GetMaxHealth();
            float normalizedDamage = (maxHealth - building.GetCurrentHealth()) / maxHealth;
            if (normalizedDamage > normalizedMaxDamage)
                building.immortal = true;
            else
                building.immortal = false;
        }
    }
    #endregion
}
