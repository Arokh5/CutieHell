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
    public override void TakeDamage(float damage, AttackType attacktype)
    {
        base.TakeDamage(damage, attacktype);
        float normalizedDamage = (baseHealth - currentHealth) / baseHealth;
        monumentIndicator.SetFill(normalizedDamage);
        zoneController.InformMonumentDamage(normalizedDamage);
    }
    
    public void OnEnemiesComing()
    {
        if (currentHealth > 0)
            monumentIndicator.RequestOpen();
    }

    public override void BuildingConverted()
    {
        zoneController.OnMonumentTaken();
    }

    public override void BuildingKilled()
    {
        if (protectedMonument)
            protectedMonument.monumentIndicator.RequestOpen();
        monumentIndicator.DeactivateIconConquered();
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