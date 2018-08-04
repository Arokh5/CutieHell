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
    [Header("Health bar")]
    public bool isFirstEnemyTarget = false;
    [SerializeField]
    private string healthBarTitle = "UNNAMED";
    [SerializeField]
    private Monument protectedMonument;

    private MonumentsHealthBar healthBar;
    #endregion

    #region MonoBehaviour Methods
    private new void Start()
    {
        base.Start();
        healthBar = UIManager.instance.monumentsHealthBar;
        if (isFirstEnemyTarget)
            SetUpHealthBar();    
    }

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
        if (!IsDead())
            healthBar.SetHealthBarFill(currentHealth / baseHealth);

        float normalizedDamage = (baseHealth - currentHealth) / baseHealth;
        zoneController.InformMonumentDamage(normalizedDamage);
    }

    public void SetUpHealthBar()
    {
        healthBar.SetHealthBarTitle(healthBarTitle);
        healthBar.RefillHealthBar();
    }

    public override void BuildingKilled()
    {
        healthBar.SetHealthBarFill(0.0f);
    }

    public override void BuildingConverted()
    {
        zoneController.OnMonumentTaken();
        if (protectedMonument)
            protectedMonument.SetUpHealthBar();
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
                }
                if (showAlmostConqueredScreenTintTexture > 2)
                {
                    showAlmostConqueredScreenTintTexture = 0;
                }
                showAlmostConqueredScreenTintTexture += Time.deltaTime;
            }
        }

        if (shouldShowScreenOverlay != showingScreenOverlay)
        {
            almostConqueredScreenOverlay.gameObject.SetActive(shouldShowScreenOverlay);
            showingScreenOverlay = shouldShowScreenOverlay;
        }
    }
    #endregion
}