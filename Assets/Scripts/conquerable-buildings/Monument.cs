using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monument : Building
{

    #region Fields
    [Header("Monument attributes")]
    [SerializeField]
    private Texture almostConqueredScreenTintTexture;
    private float showAlmostConqueredScreenTintTexture = 0;
    [Range(0, 1)]
    [SerializeField]
    private float lowHealthScreen;
    [Space]
    public float maxRepairDistance = 5;
    [SerializeField]
    private MonumentIndicator monumentIndicator;
    [SerializeField]
    private Monument protectedMonument;
    #endregion

    #region Public Methods
    // IDamageable
    // If this method is called, it should inform the ZoneController and UIManager
    public override void FullRepair()
    {
        if (!zoneController.isFinalZone || currentHealth != 0)
            base.FullRepair();
    }

    public override void TakeDamage(float damage, AttackType attacktype)
    {
        base.TakeDamage(damage, attacktype);
        monumentIndicator.SetFill((baseHealth - currentHealth) / baseHealth);
    }
    
    public MonumentIndicator GetMonumentIndicator()
    {
        return monumentIndicator;
    }

    public override void BuildingConverted()
    {
        zoneController.OnMonumentTaken();
    }

    public override void BuildingKilled()
    {
        zoneController.OnMonumentTaken();
        if (protectedMonument)
            protectedMonument.monumentIndicator.RequestOpen();
    }

    public override void BuildingRecovered()
    {
        zoneController.OnMonumentRecovered();
        if (protectedMonument)
            protectedMonument.monumentIndicator.RequestClose();
    }
    #endregion

    #region Private Methods
    private void OnGUI()
    {
        if (currentHealth <= (baseHealth * lowHealthScreen) && currentHealth > 0 && zoneController.GetZoneEnemiesCount() > 0)
        {
            if (showAlmostConqueredScreenTintTexture > 1)
            {
                GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), almostConqueredScreenTintTexture);
            }
            if (showAlmostConqueredScreenTintTexture > 2)
            {
                showAlmostConqueredScreenTintTexture = 0;
            }
            showAlmostConqueredScreenTintTexture += Time.deltaTime;
        }
    }
    #endregion
}