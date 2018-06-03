using UnityEngine;
using UnityEngine.UI;

public class Monument : Building
{

    #region Fields
    [Header("Monument attributes")]
    [SerializeField]
    private Image almostConqueredScreenOverlay;
    private bool showingScreenOverlay = false;
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

    #region MonoBehaviour Methods
    private new void Update()
    {
        base.Update();
        ShowAlmostConqueredScreenTint();
    }
    #endregion

    #region Public Methods
    // IDamageable
    // If this method is called, it should inform the ZoneController and UIManager
    public override void FullRepair()
    {
        if (!zoneController.isFinalZone || currentHealth != 0)
        {
            base.FullRepair();
            monumentIndicator.SetFill((baseHealth - currentHealth) / baseHealth);
        }
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
        monumentIndicator.DeactivateIconConquered();
    }

    public override void BuildingRecovered()
    {
        zoneController.OnMonumentRecovered();
        if (protectedMonument)
            protectedMonument.monumentIndicator.RequestClose();
    }
    #endregion

    #region Private Methods
    private void ShowAlmostConqueredScreenTint()
    {
        bool shouldShowScreenOverlay = false;
        if (zoneController.GetZoneEnemiesCount() > 0)
        {
            if (currentHealth <= (baseHealth * lowHealthScreen) && currentHealth > 0)
            {
                if (showAlmostConqueredScreenTintTexture > 1)
                {
                    shouldShowScreenOverlay = true;
                    monumentIndicator.ActivateIconConquered();
                }
                if (showAlmostConqueredScreenTintTexture > 2)
                {
                    showAlmostConqueredScreenTintTexture = 0;
                    monumentIndicator.DeactivateIconConquered();
                }
                showAlmostConqueredScreenTintTexture += Time.deltaTime;
            }
        }
        else
        {
            monumentIndicator.DeactivateIconConquered();
        }

        if (shouldShowScreenOverlay != showingScreenOverlay)
        {
            almostConqueredScreenOverlay.gameObject.SetActive(shouldShowScreenOverlay);
            showingScreenOverlay = shouldShowScreenOverlay;
        }
    }
    #endregion
}